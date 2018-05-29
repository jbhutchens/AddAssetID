/****** Object:  Table [dbo].[AssetIDTableMap]    Script Date: 03/15/2018 1:34:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AssetIDTableMap](
	[TableName] [nvarchar](128) NOT NULL,
	[PreFix] [varchar](10) NOT NULL,
	[SchemaName] [nvarchar](50) NOT NULL,
	[LastAssetNumber] [int] NULL,
	[rid] [int] IDENTITY(1,1) NOT NULL,
	[Versioned] [int] NOT NULL,
	[VersionView]  AS (case when [Versioned]=(1) then [TableName]+'_evw' else [TableName] end) PERSISTED NOT NULL,
 CONSTRAINT [pkc_AssetIDTableMap_TableName] PRIMARY KEY CLUSTERED 
(
	[TableName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AssetIDTableMap] ADD  CONSTRAINT [df_AssetIDTableMap_Versioned]  DEFAULT ((1)) FOR [Versioned]
GO

--You can set a default on the schema, a lot of times it makes sense to
--ALTER TABLE [dbo].[AssetIDTableMap] ADD  CONSTRAINT [df_AssetIDTableMap_SchemaName]  DEFAULT (N'OPS') FOR [SchemaName]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AssetIDSQL](
	[SqlToRun] [nvarchar](max) NULL,
	[rid] [int] NULL,
	[tableName] [nvarchar](128) NULL,
	[AddedDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AssetIDSQL] ADD  CONSTRAINT [df_AssetIDSQL_AddedDate]  DEFAULT (getdate()) FOR [AddedDate]
GO



