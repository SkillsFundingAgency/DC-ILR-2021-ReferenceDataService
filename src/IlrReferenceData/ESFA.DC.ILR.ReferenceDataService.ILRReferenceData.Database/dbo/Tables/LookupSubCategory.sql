CREATE TABLE [dbo].[LookupSubCategory]
(
	[ParentName] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](400) NULL,
	[EffectiveFrom] [date] NULL,
	[EffectiveTo] [date] NULL, 
    CONSTRAINT [PK_LookupSubCategory] PRIMARY KEY ([ParentName], [Name], [Code]), 
    CONSTRAINT [FK_LookupSubCategory_ToLookup] FOREIGN KEY ([ParentName], [Name]) REFERENCES [Lookup]([Name], [Code]) ON DELETE CASCADE,
)
