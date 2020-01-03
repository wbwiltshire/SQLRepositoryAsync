USE [DCustomer]
GO

/****** Object: Table [dbo].[City] Script Date: 5/18/2014 9:09:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [dbo].[City]
GO

CREATE TABLE [dbo].[City] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [StateId]    CHAR (2)      NOT NULL,
    [Active]     BIT           NOT NULL,
    [ModifiedDt] DATETIME      NOT NULL,
    [CreateDt]   DATETIME      NOT NULL
	CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ( [Id] ASC)
) ON [PRIMARY]

