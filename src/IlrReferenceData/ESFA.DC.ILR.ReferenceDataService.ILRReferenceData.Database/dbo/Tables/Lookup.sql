CREATE TABLE [dbo].[Lookup]
(
	[Name] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[EffectiveFrom] [date] NULL,
	[EffectiveTo] [date] NULL, 
    CONSTRAINT [PK_Lookup] PRIMARY KEY ([Name], [Code]) 
)

GO