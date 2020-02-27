CREATE TABLE [ReferenceInput].[FCS_EsfEligibilityRule]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Benefits] [bit] NULL,
	[CalcMethod] [int] NULL,
	[TenderSpecReference] [nvarchar](2000) NULL,
	[LotReference] [nvarchar](2000) NULL,
	[MinAge] [int] NULL,
	[MaxAge] [int] NULL,
	[MinLengthOfUnemployment] [int] NULL,
	[MaxLengthOfUnemployment] [int] NULL,
	[MinPriorAttainment] [nvarchar](2000) NULL,
	[MaxPriorAttainment] [nvarchar](2000) NULL,
	CONSTRAINT [PK_ReferenceInput.FCS_EsfEligibilityRule] PRIMARY KEY ([Id]),
)
