CREATE TABLE [ReferenceInput].[McaContracts_McaDevolvedContract]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[McaGlaShortCode] [nvarchar](max) NULL,
	[Ukprn] [int] NOT NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	CONSTRAINT [PK_ReferenceInput.McaContracts_McaDevolvedContract] PRIMARY KEY ([Id]),
)
