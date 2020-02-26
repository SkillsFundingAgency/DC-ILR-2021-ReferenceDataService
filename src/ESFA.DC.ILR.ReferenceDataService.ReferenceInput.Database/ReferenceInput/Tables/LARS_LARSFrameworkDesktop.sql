CREATE TABLE [ReferenceInput].[LARS_LARSFrameworkDesktop]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FworkCode] [int] NOT NULL,
	[ProgType] [int] NOT NULL,
	[PwayCode] [int] NOT NULL,
	[EffectiveFromNullable] [datetime] NULL,
	[EffectiveTo] [datetime] NULL,
	CONSTRAINT [PK_ReferenceInput.LARS_LARSFrameworkDesktop] PRIMARY KEY ([Id]),
)
