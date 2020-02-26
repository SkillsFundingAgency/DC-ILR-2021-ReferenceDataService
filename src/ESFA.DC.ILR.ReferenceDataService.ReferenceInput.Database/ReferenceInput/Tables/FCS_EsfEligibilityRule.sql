CREATE TABLE [ReferenceInput].[FCS_EsfEligibilityRule]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Benefits] [bit] NULL,
	[CalcMethod] [int] NULL,
	[TenderSpecReference] [nvarchar](max) NULL,
	[LotReference] [nvarchar](max) NULL,
	[MinAge] [int] NULL,
	[MaxAge] [int] NULL,
	[MinLengthOfUnemployment] [int] NULL,
	[MaxLengthOfUnemployment] [int] NULL,
	[MinPriorAttainment] [nvarchar](max) NULL,
	[MaxPriorAttainment] [nvarchar](max) NULL,
	CONSTRAINT [PK_ReferenceInput.FCS_EsfEligibilityRule] PRIMARY KEY ([Id]),
)
