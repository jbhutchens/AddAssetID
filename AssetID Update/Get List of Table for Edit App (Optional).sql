if exists(Select 1 from sys.objects where name = 'usp_Arvada_GetEditableTables')
drop procedure usp_Arvada_GetEditableTables

go

create procedure dbo.usp_Arvada_GetEditableTables
	@ReturnType int = 0,
	@TableName nvarchar(255) = null
as

set nocount on

/*
exec dbo.usp_Arvada_GetEditableTables 0
exec dbo.usp_Arvada_GetEditableTables @returnType = 1, @TableName = 'dbo.AssetIDTableMap'
exec dbo.usp_Arvada_GetEditableTables @returnType = 1, @TableName = 'dbo.AssetIDSQL'
 
*/

if @ReturnType = 0
begin

	select distinct
		c.TABLE_SCHEMA +'.'+ c.TABLE_NAME as TableName 
	from INFORMATION_SCHEMA.COLUMNS c
	inner join  INFORMATION_SCHEMA.TABLES t
		on t.TABLE_NAME = c.TABLE_NAME
		and t.TABLE_SCHEMA = c.TABLE_SCHEMA
	left join
		(
			select 
				object_name(object_id) as tablename,
				name as ColName
			from sys.columns
			where 
				  is_identity = 1
			) as b
			on b.tablename= c.table_name
			and b.ColName =c.COLUMN_NAME
	inner join
			(
				SELECT
					 sc.name +'.'+ ta.name TableName,
					 SUM(pa.rows) RowCnt
				FROM sys.tables ta
				INNER JOIN sys.partitions pa
					ON pa.OBJECT_ID = ta.OBJECT_ID
				INNER JOIN sys.schemas sc
					ON ta.schema_id = sc.schema_id
				WHERE 
					ta.is_ms_shipped = 0 
					AND pa.index_id IN (1,0)
				GROUP BY sc.name,ta.name 
			) as RowCnt
			on RowCnt.TableName = c.TABLE_SCHEMA +'.'+ c.TABLE_NAME
	where  
		 b.ColName is null --don't return identity columns
	and RowCnt.RowCnt < 5000 --
	and t.table_type = 'BASE TABLE' 
	 and t.TABLE_NAME not like 'SDE%'
	 and t.TABLE_NAME not like 'GDB%'
	 and  not exists(Select 1 from INFORMATION_SCHEMA.COLUMNS c2 where c2.TABLE_NAME = c.TABLE_NAME and c2.TABLE_SCHEMA = c.TABLE_SCHEMA and c2.COLUMN_NAME in ('OBJECTID', 'SHAPE','REL_OBJECTID','SDE_STATE_ID','num_ids'))
	order by
		TableName 
end
else if @ReturnType = 1
begin
	create table #col
	(
		colName nvarchar(255),
		ordinalpos int

	)

	insert into #col
	(
		colName,
		ordinalpos
	)
	select 
		c.column_name ,
		c.ORDINAL_POSITION
	from INFORMATION_SCHEMA.COLUMNS c
	inner join  INFORMATION_SCHEMA.TABLES t
		on t.TABLE_NAME = c.TABLE_NAME
		and t.TABLE_SCHEMA = c.TABLE_SCHEMA
	left join
		(
			select 
				object_name(object_id) as tablename,
				name as ColName
			from sys.columns
			where 
				  is_identity = 1
			) as b
			on b.tablename= c.table_name
			and b.ColName =c.COLUMN_NAME
	inner join
			(
				SELECT
					 sc.name +'.'+ ta.name TableName,
					 SUM(pa.rows) RowCnt
				FROM sys.tables ta
				INNER JOIN sys.partitions pa
					ON pa.OBJECT_ID = ta.OBJECT_ID
				INNER JOIN sys.schemas sc
					ON ta.schema_id = sc.schema_id
				WHERE 
					ta.is_ms_shipped = 0 
					AND pa.index_id IN (1,0)
				GROUP BY sc.name,ta.name 
			) as RowCnt
			on RowCnt.TableName = c.TABLE_SCHEMA +'.'+ c.TABLE_NAME
	where  
		 b.ColName is null --don't return identity columns
	and c.TABLE_SCHEMA +'.'+ c.TABLE_NAME = @TableName
	
	select
		'SELECT ' +
		STUFF((select ', ' + colName from #col c order by ordinalpos 
            FOR XML PATH('')
            ), 1, 1, '') +
		' FROM ' + @TableName + '
		 ORDER BY ' + case when @TableName = 'dbo.AssetIDTableMap' then 'TableName' else '1' end

end