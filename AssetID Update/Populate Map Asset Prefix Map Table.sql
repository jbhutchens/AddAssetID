insert into dbo.AssetIDTableMap
(
	TableName,
	PreFix,
	SchemaName
)
  SELECT 'wControlValve','wCV', 'OPS'
UNION ALL SELECT 'wMasterMeter','wMM', 'OPS'
UNION ALL SELECT 'wPump','wPU', 'OPS' 