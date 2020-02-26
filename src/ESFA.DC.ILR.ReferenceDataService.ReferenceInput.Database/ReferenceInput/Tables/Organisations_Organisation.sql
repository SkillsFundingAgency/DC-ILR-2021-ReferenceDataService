CREATE TABLE [ReferenceInput].[Organisations_Organisation]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UKPRN] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[PartnerUKPRN] [bit] NULL,
	[LegalOrgType] [nvarchar](max) NULL,
	[LongTermResid] [bit] NULL,
	CONSTRAINT [PK_ReferenceInput.Organisations_Organisation] PRIMARY KEY ([Id]),
)
