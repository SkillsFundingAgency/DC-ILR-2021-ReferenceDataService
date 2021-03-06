﻿CREATE TABLE [ReferenceInput].[LARS_LARSFrameworkCommonComponent] (
    [Id]                           INT      IDENTITY (1, 1) NOT NULL,
    [CommonComponent]              INT      NOT NULL,
    [EffectiveFrom]                DATETIME NOT NULL,
    [EffectiveTo]                  DATETIME NULL,
    [LARS_LARSFrameworkDesktop_Id] INT      NULL,
    CONSTRAINT [PK_ReferenceInput.LARS_LARSFrameworkCommonComponent] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSFrameworkCommonComponent_ReferenceInput.LARS_LARSFrameworkDesktop_LARSFrameworkDesktop_Id] FOREIGN KEY ([LARS_LARSFrameworkDesktop_Id]) REFERENCES [ReferenceInput].[LARS_LARSFrameworkDesktop] ([Id])
);


