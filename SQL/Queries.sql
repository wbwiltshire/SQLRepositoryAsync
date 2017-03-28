


--Contact View
SELECT c1.Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, c2.Name as CityName, StateId, s.Name as StateName, c1.Active, c1.ModifiedDt, c1.CreateDt 
FROM Contact c1 JOIN City c2 ON (c2.Id = c1.CityId) JOIN State s ON (s.Id = c2.StateId) 
WHERE c1.Active=1


--Testing
DECLARE	@p1 NVARCHAR (25),
		@p2 NVARCHAR (30), 
		@p3 NVARCHAR (40),
		@p4 NVARCHAR (30),
		@p5 NVARCHAR (150),
		@p6 NVARCHAR (10),
		@p7 NVARCHAR (10),
		@p8 NVARCHAR (10),
		@p9 NVARCHAR (10), 
		@p10 NVARCHAR (4000), 
		@p11 INT;

SET @p1 = "New";
SET @p2 = "User";
SET @p3 = "Address1";
SET @p4 = "Address2";
SET @p5 = "Note 1";
SET @p6 = "99999";
SET @p7 = "8005551212";
SET @p8 = "8005551212";
SET @p9 = "8005551212";
SET @p10 = "NewUser@Mail.com";
SET @p11 = 1;

INSERT INTO Contact (FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, Active, ModifiedDt, CreateDt) 
OUTPUT INSERTED.Id 
VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, 1, GETDATE(), GETDATE())