--Contact Views
--
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'vwFindAllContactView')
   DROP VIEW [dbo].[vwFindAllContactView]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwFindAllContactView]
AS
	SELECT c1.Id AS Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, 
		c2.Name AS CityName, c2.StateId AS StateId, 
		s.Name AS StateName, 
		c1.Active AS Active, c1.ModifiedUtcDt AS ModifiedUtcDt, c1.CreateUtcDt AS CreateUtcDt 
	FROM Contact c1 
	JOIN City c2 ON (c2.Id = c1.CityId) 
	JOIN State s ON (s.Id = c2.StateId) 
	WHERE c1.Active=1
GO
--City Views
--
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'vwFindAllCityView')
   DROP VIEW [dbo].[vwFindAllCityView]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwFindAllCityView]
AS
	SELECT c.Id AS Id, c.Name AS Name, StateId, 
		s.Name AS StateName, 
		c.Active AS Active, c.ModifiedUtcDt AS ModifiedUtcDt, c.CreateUtcDt AS CreateUtcDt 
	FROM City c
	JOIN State s ON (s.Id = c.StateId) 
	WHERE c.Active=1
GO