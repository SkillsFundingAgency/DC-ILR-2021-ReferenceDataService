CREATE TABLE [ReferenceInput].[LARS_LARSStandard]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StandardCode] [int] NOT NULL,
	[StandardSectorCode] [nvarchar](2000) NULL,
	[NotionalEndLevel] [nvarchar](2000) NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	CONSTRAINT [PK_ReferenceInput.LARS_LARSStandard] PRIMARY KEY ([Id]),
)
