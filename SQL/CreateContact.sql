USE [DCustomer]
GO

/****** Object: Table [dbo].[Contact] Script Date: 5/18/2014 9:01:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [dbo].[Contact]
GO

CREATE TABLE [dbo].[Contact] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [FirstName]  NVARCHAR (25)   NOT NULL,
    [LastName]   NVARCHAR (30)   NOT NULL,
    [Address1]   NVARCHAR (40)   NULL,
    [Address2]   NVARCHAR (30)   NULL,
    [Notes]      NVARCHAR (150)  NULL,
    [ZipCode]    NVARCHAR (10)   NULL,
    [HomePhone]  NVARCHAR (10)   NULL,
    [WorkPhone]  NVARCHAR (10)   NULL,
    [CellPhone]  NVARCHAR (10)   NULL,
    [EMail]      NVARCHAR (4000) NULL,
    [CityId]     INT             NOT NULL,
    [Active]     BIT             NOT NULL,
    [ModifiedDt] DATETIME        NOT NULL,
    [CreateDt]   DATETIME        NOT NULL,
	CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED (	[Id] ASC)
) ON [PRIMARY]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'Id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'First Name', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'FirstName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last Name', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'LastName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Address 1', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'Address1'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Address 2', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'Address2'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'Notes'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Zip Code', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'ZipCode'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Home Phone', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'HomePhone'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Work Phone', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'WorkPhone'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cell Phone', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'CellPhone'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'E-mail', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'EMail'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'City Id', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'CityId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Active', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'Active'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Modified Date', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'ModifiedDt'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Create Date', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Contact', @level2type=N'COLUMN',@level2name=N'CreateDt'



