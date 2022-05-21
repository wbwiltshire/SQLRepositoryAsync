USE [DCustomer]
GO

--Drop Extended Properties
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'ProjectContact') 
	BEGIN
		EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'ProjectId'
		EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'ContactId'
		EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'Active'
		EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'ModifiedUtcDt'
		EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'CreateUtcDt'
	END
GO

--Drop and Recreate Table
/****** Object:  Table [dbo].[ProjectContact]    Script Date: 1/2/2020 1:48:02 PM ******/
DROP TABLE IF EXISTS [dbo].[ProjectContact]
GO

/****** Object:  Table [dbo].[ProjectContact]    Script Date: 1/2/2020 1:48:02 PM ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[ProjectContact](
	[ProjectId] [INT] NOT NULL,             -- Foreign key
	[ContactId] [INT] NOT NULL,           	-- Foreign key
	[Active] [BIT] NOT NULL,
	[ModifiedUtcDt] [DATETIME] NOT NULL,
	[CreateUtcDt] [DATETIME] NOT NULL,
 CONSTRAINT [PK_ProjectContact] PRIMARY KEY CLUSTERED ([ProjectId] ASC,[ContactId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Add Extended Properties
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Project Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'ProjectId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contact Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'ContactId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Is Active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'Active'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Modified Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'ModifiedUtcDt'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Create Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectContact', @level2type=N'COLUMN',@level2name=N'CreateUtcDt'
GO