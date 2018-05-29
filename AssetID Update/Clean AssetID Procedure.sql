/****** Object:  StoredProcedure [dbo].[usp_Arvada_CheckAssetIDs]    Script Date: 05/29/2018 1:44:12 PM ******/
DROP PROCEDURE [dbo].[usp_Arvada_CheckAssetIDs]
GO

/****** Object:  StoredProcedure [dbo].[usp_Arvada_CheckAssetIDs]    Script Date: 05/29/2018 1:44:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_Arvada_CheckAssetIDs]

	@TestMode bit = 0

AS

/**********************************************************************************************************************
* Creator:    Jesse Hutchens  	 
* Date:       07/18/2016  	 
* Title:			 
* Description:  	 
* Examples:    
select * 
from ops.swManhole

begin tran
EXECUTE dbo.[usp_Arvada_CheckAssetIDs] @TestMode = 0
rollback
 commit

--ops.wOutletStructure
					 
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
create table #loop
(
	tablename varchar(50),
	prefix varchar(50),
	rid int,
	SchemaName nvarchar(50)
)

create table #results
(
	tablename varchar(50),
	AssetID nvarchar(50)
)

  --------------------------
 --- Body of Procedure   --
 --------------------------
 
SET NOCOUNT ON


insert into #loop
(
	tablename,
	prefix,
	rid,
	SchemaName
)
select
	t.VersionView,
	t.prefix,
	row_number() over (order by t.tablename) as rid,
	t.SchemaName
from dbo.AssetIDTableMap t
inner join INFORMATION_SCHEMA.TABLES t2
	on t2.TABLE_NAME = t.VersionView


-- set the default tableID, which for versioned tables is actually a view
 
declare @s int,
		@e int,
		@sql nvarchar(max),
		@TableName nvarchar(max),
		@prefix varchar(50),
		@Schema nvarchar(50)


select 
	@s = min(rid),
	@e = max(rid)
from #loop

while @s <= @e
begin

	select  
		@TableName = TableName,
		@prefix = prefix,
		@schema = SchemaName
	from #loop
	where rid = @s

	--check 1: update where empty string:
	set @sql = '
				if object_id(''tempdb.dbo.#obj'') is not null drop table #obj
				create table #obj
				(
					objectid int
				)
				
				insert into #obj
				(
					objectid
				)
				select
					objectid
				from ['+@schema+'].['+@TableName+'] a
				where len(AssetID) = 0
				or AssetId = ''null''
				
				if exists(select 1 from #obj)
				begin	
					update a
					set AssetID = null
					from ['+@schema+'].['+@TableName+'] a
					where objectid in (select objectid from #obj)
				end
				'

	
	if @TestMode = 1 and @TestMode is not null
	begin
		--print @TableName
		print @sql
	end
	else
	begin 
		exec(@sql)
	end

	--this is not a good check when we merge assets
	--check 2: update where it did apply the prefix:			 
	--set @sql = 'update a
	--			set AssetID = null
	--			from ops.'+@TableName+' a
	--			where AssetID not like '''+@prefix+'%'''
	
	--if @TestMode = 1 and @TestMode is not null
	--begin
	--	--print @TableName
	--	print @sql
	--end
	--else
	--begin 
	--	exec(@sql)
	--end

	--check 3: clear where we have duplicates:
	set @sql = '
				if object_id(''tempdb.dbo.#obj'') is not null drop table #obj
				create table #obj
				(
					objectid int
				)
				

				if exists(
							SELECT  
									assetid
								from ['+@schema+'].['+@TableName+']
								where 
									assetID is not null
								group by 
									assetid
								having count(1) > 1
						 )
				begin
					insert into #obj
					(
						objectid
					)
					select
						a.objectid 
					from ['+@schema+'].['+@TableName+'] a
					inner join 
					( 
						select
							a.assetid,
							a.objectid,
							row_number() over (partition by a.assetID order by objectID asc) as rid
						from ['+@schema+'].['+@TableName+'] a
						inner join  (SELECT  
										assetid
									from ['+@schema+'].['+@TableName+']
									where 
										assetID is not null
									group by 
										assetid
									having count(1) > 1
									) as b
								on b.assetid = a.assetid
					) as b
					on b.objectid = a.objectid
					and b.rid > 1 --keep the first one, everything past it we are going to update
				end


				if exists(select 1 from #obj)
				begin	
					update a
					set AssetID = null
					from ['+@schema+'].['+@TableName+'] a 
					where
						a.objectid in (Select objectid from #obj)
				end
				'
	if @TestMode = 1 and @TestMode is not null
	begin
		--print @TableName
		print @sql
	end
	else
	begin 
		exec(@sql)
	end

	

	--set @sql =
	--'
	--	insert into #results
	--	(
	--		tablename,
	--		assetid
	--	)
	--	SELECT 
	--		'''+@TableName+''',
	--		assetid
	--	from ops.'+@TableName+'
	--	group by assetid
	--	having count(1) > 1
	--'


	--print @TableName
	set @s = @s+1
	 
	
end	
 
GO


