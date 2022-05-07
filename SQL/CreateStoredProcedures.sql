--Contact Stored Procedures
--
DROP PROCEDURE IF EXISTS [dbo].[uspAddContact]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspAddContact
	@p1 NVARCHAR (25),		-- FirstName
	@p2 NVARCHAR (30),		-- LastName
	@p3 NVARCHAR (40),		-- Address1
	@p4 NVARCHAR (30),		-- Address2
	@p5 NVARCHAR (150),		-- Notes
	@p6 NVARCHAR (10),		-- ZipCode
	@p7 NVARCHAR (10),		-- HomePhone
	@p8 NVARCHAR (10),		-- WorkPhone
	@p9 NVARCHAR (10),		-- CellPhone
	@p10 NVARCHAR (4000),	-- EMail
	@p11 INT				-- CityId
	
AS
BEGIN
	INSERT INTO Contact (FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt)
		VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, 1, GETDATE(), GETDATE()); 
	SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

DROP PROCEDURE IF EXISTS [dbo].[uspUpdateContact]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspUpdateContact
	@p1 NVARCHAR (25),		-- FirstName
	@p2 NVARCHAR (30),		-- LastName
	@p3 NVARCHAR (40),		-- Address1
	@p4 NVARCHAR (30),		-- Address2
	@p5 NVARCHAR (150),		-- Notes
	@p6 NVARCHAR (10),		-- ZipCode
	@p7 NVARCHAR (10),		-- HomePhone
	@p8 NVARCHAR (10),		-- WorkPhone
	@p9 NVARCHAR (10),		-- CellPhone
	@p10 NVARCHAR (4000),	-- EMail
	@p11 INT,				-- CityId
	@pk INT				-- Primary Key
	
AS
BEGIN
	UPDATE Contact SET FirstName=@p1, LastName=@p2, Address1=@p3, Address2=@p4, Notes=@p5, ZipCode=@p6, HomePhone=@p7, WorkPhone=@p8, CellPhone=@p9, EMail=@p10, CityId=@p11, Active=1, ModifiedDt=GETDATE() 
		WHERE Id =@pk AND Active=1
END
GO

DROP PROCEDURE IF EXISTS [dbo].[uspFindAllContactPaged]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspFindAllContactPaged
	@offset	INT,		-- Offset
	@pageSize	INT,		-- PageSize
	@sortColumn INT,	
	@direction INT
	
AS
BEGIN
	SELECT Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt 
	FROM Contact 
	WHERE Active=1 
	ORDER BY
	CASE WHEN @direction = 1 THEN
		CASE
			WHEN @sortColumn = 1 THEN Id 
			ELSE Id					-- Always have a default 
		END
	END ASC,
	CASE WHEN @direction = 2 THEN
		CASE
			WHEN @sortColumn = 1 THEN Id 
			ELSE Id					-- Always have a default 
		END
	END DESC
	OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
END
GO

DROP PROCEDURE IF EXISTS [dbo].[uspFindAllContactViewPaged]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspFindAllContactViewPaged
	@offset	INT,		-- Offset
	@pageSize	INT,		-- PageSize
	@sortColumn INT,	
	@direction INT
	
AS
BEGIN
	SELECT Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId,  CityName, StateId, StateName, Active, ModifiedDt, CreateDt 
	FROM vwFindAllContactView
	ORDER BY
	CASE WHEN @direction = 1 THEN
		CASE
			WHEN @sortColumn = 1 THEN Id 
			ELSE Id					-- Always have a default 
		END
	END ASC,
	CASE WHEN @direction = 2 THEN
		CASE
			WHEN @sortColumn = 1 THEN Id 
			ELSE Id					-- Always have a default 
		END
	END DESC
	OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
END
GO

--City Stored Procedures
--
DROP PROCEDURE IF EXISTS [dbo].[uspAddCity]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspAddCity
	@p1 NVARCHAR (50),		-- Name
	@p2 CHAR (2)			-- StateId
	
AS
BEGIN
	INSERT INTO City (Name, StateId, Active, ModifiedDt, CreateDt)
		VALUES (@p1, @p2, 1, GETDATE(), GETDATE()); 
	SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

DROP PROCEDURE IF EXISTS [dbo].[uspUpdateCity]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspUpdateCity
	@p1 NVARCHAR (50),		-- Name
	@p2 CHAR (2),			-- StateId
	@pk INT				-- Primary Key
	
AS
BEGIN
	UPDATE City SET Name=@p1, StateId=@p2, ModifiedDt=GETDATE() 
		WHERE Id =@pk AND Active=1
END
GO

DROP PROCEDURE IF EXISTS [dbo].[uspFindAllCityPaged]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspFindAllCityPaged
	@offset	INT,		-- Offset
	@pageSize	INT,		-- PageSize
	@sortColumn INT,	
	@direction INT
	
AS
BEGIN
	SELECT Id, Name, StateId, Active, ModifiedDt, CreateDt 
	FROM City 
	WHERE Active=1 
	ORDER BY
	CASE WHEN @direction = 1 THEN
		CASE
			WHEN @sortColumn = 1 THEN Id 
			ELSE Id					-- Always have a default 
		END
	END ASC,
	CASE WHEN @direction = 2 THEN
		CASE
			WHEN @sortColumn = 1 THEN Id 
			ELSE Id					-- Always have a default 
		END
	END DESC
	OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
END
GO

DROP PROCEDURE IF EXISTS [dbo].[uspFindAllCityViewPaged]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspFindAllCityViewPaged
	@offset	INT,		-- Offset
	@pageSize	INT,		-- PageSize
	@sortColumn INT,	
	@direction INT
	
AS
BEGIN
	SELECT Id, Name, StateId, StateName, Active, ModifiedDt, CreateDt 
	FROM vwFindAllCityView 
	ORDER BY
	CASE WHEN @direction = 1 THEN
		CASE
			WHEN @sortColumn = 1 THEN Id 
			ELSE Id					-- Always have a default 
		END
	END ASC,
	CASE WHEN @direction = 2 THEN
		CASE
			WHEN @sortColumn = 1 THEN Id 
			ELSE Id					-- Always have a default 
		END
	END DESC
	OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
END
GO

--State Stored Procedures
--
DROP PROCEDURE IF EXISTS [dbo].[uspAddState]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspAddState
	@p1 NVARCHAR (30),	-- Name
	@pk CHAR(2)			-- Primary Key
	
AS
BEGIN
	INSERT INTO State (Id, Name, Active, ModifiedDt, CreateDt) VALUES (@pk, @p1, 1, GETDATE(), GETDATE())
END
GO

DROP PROCEDURE IF EXISTS [dbo].[uspUpdateState]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspUpdateState
	@p1 NVARCHAR (30),		-- Name
	@pk CHAR(2)			-- Primary Key
	
AS
BEGIN
	UPDATE State SET Name=@p1, Active=1, ModifiedDt=GETDATE() 
		WHERE Id = @pk AND Active=1
END
GO
--Testing Stored Procedures
--

DROP PROCEDURE IF EXISTS [dbo].[uspStoredProc]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspStoredProc
	@pk CHAR(2)			-- Primary Key
	
AS
BEGIN
	UPDATE Contact SET ModifiedDt=GETDATE() 
		WHERE Id = @pk AND Active=1
END
GO

--Stored Procedure for testing ExecJSONQuery
--

DROP PROCEDURE IF EXISTS [dbo].[uspGetStats]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspGetStats
AS
BEGIN
	DECLARE @totalContacts int;
	DECLARE @totalProjects int;
	DECLARE @totalCities int;
	DECLARE @totalStates int;

	SET @totalContacts = (SELECT COUNT(1) FROM Contact WHERE Active=1);
	SET @totalProjects = (SELECT COUNT(1) FROM Project WHERE Active=1);
	SET @totalCities = (SELECT COUNT(1) FROM City WHERE Active=1);
	SET @totalStates = (SELECT COUNT(1) FROM State WHERE Active=1);
	
	SELECT @totalContacts AS TotalContacts, @totalProjects AS TotalProjects, @totalCities AS TotalCities, @totalStates AS TotalStates
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
END
GO