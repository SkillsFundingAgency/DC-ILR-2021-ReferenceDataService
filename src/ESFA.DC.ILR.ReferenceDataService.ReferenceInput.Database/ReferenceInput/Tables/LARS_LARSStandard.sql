CREATE TABLE [ReferenceInput].[LARS_LARSStandard]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StandardCode] [int] NOT NULL,
	[StandardSectorCode] [nvarchar](max) NULL,
	[NotionalEndLevel] [nvarchar](max) NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	CONSTRAINT [PK_ReferenceInput.LARS_LARSStandard] PRIMARY KEY ([Id]),
)
