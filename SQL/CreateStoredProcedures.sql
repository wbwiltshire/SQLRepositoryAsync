--Contact Stored Procedures
--
IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspAddContact')
	DROP PROCEDURE [dbo].[uspAddContact]
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

IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspUpdateContact')
	DROP PROCEDURE [dbo].[uspUpdateContact]
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

IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspFindAllContactPaged')
	DROP PROCEDURE [dbo].[uspFindAllContactPaged]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspFindAllContactPaged
	@p1	INT,		-- Offset
	@p2	INT		-- PageSize
	
AS
BEGIN
	SELECT Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt 
	FROM Contact 
	WHERE Active=1 
	ORDER BY Id 
	OFFSET @p1 ROWS FETCH NEXT @p2 ROWS ONLY;
END
GO

IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspFindAllContactViewPaged')
	DROP PROCEDURE [dbo].[uspFindAllContactViewPaged]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspFindAllContactViewPaged
	@p1	INT,		-- Offset
	@p2	INT		-- PageSize
	
AS
BEGIN
	SELECT Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId,  CityName, StateId, StateName, Active, ModifiedDt, CreateDt 
	FROM vwFindAllContactView 
	ORDER BY Id 
	OFFSET @p1 ROWS FETCH NEXT @p2 ROWS ONLY;
END
GO

--City Stored Procedures
--
IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspAddCity')
	DROP PROCEDURE [dbo].[uspAddCity]
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

IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspUpdateCity')
	DROP PROCEDURE [dbo].[uspUpdateCity]
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

IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspFindAllCityPaged')
	DROP PROCEDURE [dbo].[uspFindAllCityPaged]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspFindAllCityPaged
	@p1	INT,		-- Offset
	@p2	INT		-- PageSize
	
AS
BEGIN
	SELECT Id, Name, StateId, Active, ModifiedDt, CreateDt 
	FROM City 
	WHERE Active=1 
	ORDER BY Id 
	OFFSET @p1 ROWS FETCH NEXT @p2 ROWS ONLY;
END
GO

IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspFindAllCityViewPaged')
	DROP PROCEDURE [dbo].[uspFindAllCityViewPaged]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE uspFindAllCityViewPaged
	@p1	INT,		-- Offset
	@p2	INT		-- PageSize
	
AS
BEGIN
	SELECT Id, Name, StateId, StateName, Active, ModifiedDt, CreateDt 
	FROM vwFindAllCityView 
	ORDER BY Id 
	OFFSET @p1 ROWS FETCH NEXT @p2 ROWS ONLY;
END
GO

--State Stored Procedures
--
IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspAddState')
	DROP PROCEDURE [dbo].[uspAddState]
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

IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'uspUpdateState')
	DROP PROCEDURE [dbo].[uspUpdateState]
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
		WHERE Id =@pk AND Active=1
END
GO