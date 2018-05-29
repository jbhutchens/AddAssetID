/****** Object:  StoredProcedure [dbo].[usp_Arvada_SetAssetID]    Script Date: 05/29/2018 1:44:21 PM ******/
DROP PROCEDURE [dbo].[usp_Arvada_SetAssetID]
GO

/****** Object:  StoredProcedure [dbo].[usp_Arvada_SetAssetID]    Script Date: 05/29/2018 1:44:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_Arvada_SetAssetID]

	@TestMode bit = 0

AS

/**********************************************************************************************************************
* Creator:    Jesse Hutchens  	 
* Date:       07/18/2016  	 
* Title:			 
* Description:  	 
* Examples:    
 

begin tran
EXECUTE dbo.usp_Arvada_SetAssetID @TestMode = 0 
 
commit
 
insert into dbo.AssetIDTableMap
(
	TableName,
	PreFix
)
  SELECT 'wControlValve','wCV'
UNION ALL SELECT 'wMasterMeter','wMM'
UNION ALL SELECT 'wPump','wPU'

 
					 
* Modifications:  	
  Developer Name		Date		Description
  ------------------	----------	----------------------------------------------------------- 
*
**********************************************************************************************************************/

set nocount on
 
 --------------------------
 --- Declare Variables   --
 --------------------------
DECLARE @owner nvarchar(128),
		@table nvarchar(128) 
 
 --------------------------
 --- Create Temp Tables  --
 --------------------------


  --------------------------
 --- Body of Procedure   --
 --------------------------
 
SET NOCOUNT ON
 

 


--clear out the table we will use to run the updates
truncate table dbo.AssetIDSQL

--get the last set assetid, if there is one.  If there is not one, set it to 0.
--then just update prefix + "-" + (rowId number + LastAssestID) where assetid is not already set
--but be sure to capture the max asset id in case somehow assets were deleted we don't populate a different asset with the same assetid
insert into dbo.AssetIDSQL
(
	SqlToRun ,
	rid ,
	tableName
)
select 
	'
	declare @maxAssetId int ,
			@currMaxAssetId int
	
	if object_id(''tempdb.dbo.#hold'') is not null drop table #hold
	create table #hold
	(
		AssetID varchar(50),
		objectid int,
		maxAssetNumber int
	)
	 
	 --we only care to do anything if there are assetIDs needing updated
	 if exists(
				select 1
				from ['+t.SchemaName+'].['+t.VersionView+'] a 
				where a.AssetID is null
				)
	begin
			--get the current max asset id
			select @currMaxAssetId = LastAssetNumber
			from dbo.AssetIDTableMap t
			where t.VersionView = '''+t.VersionView+'''
			
			if @currMaxAssetId is null set @currMaxAssetId = 0

			--get the max asset ID after removing the prefix and getting to the actual number part
			select @maxAssetId = max(convert(int, replace(replace(AssetID, t.Prefix,''''),''-'','''')))
			from ['+t.SchemaName+'].['+t.VersionView+'] a
			inner join dbo.AssetIDTableMap t
				on t.VersionView = '''+t.VersionView+'''
			where AssetID is not null
			and AssetID like t.prefix+''%''

			if @maxAssetId is null set @maxAssetId = 0

			--make sure what we have is the last one so we dont create dupes on accident via deletes
			if @currMaxAssetId > @maxAssetId
				set @maxAssetId = @currMaxAssetId
			
			--set last assetnumber to the max
			update t
			set LastAssetNumber = @maxAssetId
			from dbo.AssetIDTableMap t
			where t.VersionView = '''+t.VersionView+'''

			--this is where we setup the update to tag assetid
			truncate table #Hold

			--get the assetid, objectid and the soon to be max assetid
			insert into #Hold
			(
				AssetID ,
				objectID,
				maxAssetNumber
			)
			select
				 AssetID = t.PreFix + ''-'' + convert(nvarchar(10), @maxAssetId + b.RowId),
				 b.objectID,
				 @maxAssetId + b.RowId
			from ['+t.SchemaName+'].['+t.VersionView+'] a
			inner join
				(select
					OBJECTID,
					 row_number() over (order by OBJECTID) as RowId
				 from ['+t.SchemaName+'].['+t.VersionView+'] b
				 where b.assetID is null
				 ) as b
				 on b.OBJECTID = a.OBJECTID
			inner join dbo.AssetIDTableMap t
				on t.VersionView = '''+t.VersionView+'''
			where a.AssetID is null

			--apply the update, it needed to be done using this method of the query in the select b/c of constraints on update, I forget the specific error
			update a
			set AssetID = (select AssetID from #hold h where h.objectid = a.objectID)
			from ['+t.SchemaName+'].['+t.VersionView+'] a 
			where a.AssetID is null 

			--set last assetnumber to the max
			update t
			set LastAssetNumber = (Select max(maxAssetNumber) from #hold)
			from dbo.AssetIDTableMap t
			where t.VersionView = '''+t.VersionView+'''

	end
	' as SqlToRun,
	row_number() over (order by t.VersionView) as Rid,
	t.VersionView 
from dbo.AssetIDTableMap t
inner join INFORMATION_SCHEMA.TABLES t2
	on t2.TABLE_NAME = t.VersionView 
where
	--the table has to have a column named assetid
	exists(select 1 
			from INFORMATION_SCHEMA.COLUMNS c
			where c.TABLE_NAME = t2.TABLE_NAME
			and c.COLUMN_NAME = 'assetid'
			)


--do some QA on assetID before we set it:
EXECUTE dbo.[usp_Arvada_CheckAssetIDs] @TestMode = 0


--now run the updates
declare @s int,
		@e int,
		@sql nvarchar(max),
		@TableName nvarchar(max)


select 
	@s = min(rid),
	@e = max(rid)
from dbo.AssetIDSQL

while @s <= @e
begin

	select 
		@sql = SqlToRun,
		@TableName = TableName	
	from dbo.AssetIDSQL 
	where rid = @s

	if @TestMode = 1 and @TestMode is not null
	begin
		--print @TableName
		print @sql
	end
	else
	begin
		print @TableName
		--print @sql
		exec(@sql)
	end

	--print @TableName
	set @s = @s+1

	
end	

 
 
GO


