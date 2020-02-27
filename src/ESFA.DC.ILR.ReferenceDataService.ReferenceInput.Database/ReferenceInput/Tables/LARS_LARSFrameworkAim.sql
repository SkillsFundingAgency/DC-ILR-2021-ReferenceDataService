CREATE TABLE [ReferenceInput].[LARS_LARSFrameworkAim]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LearnAimRef] [nvarchar](2000) NULL,
	[FrameworkComponentType] [int] NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	[FworkCode] [int] NULL,
	[ProgType] [int] NULL,
	[PwayCode] [int] NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
	CONSTRAINT [PK_ReferenceInput.LARS_LARSFrameworkAim] PRIMARY KEY ([Id]),
)
