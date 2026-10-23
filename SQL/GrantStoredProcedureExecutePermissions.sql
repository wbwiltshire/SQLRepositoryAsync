USE [DCustomer]
GO

GRANT EXECUTE ON [dbo].[uspAddContact] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspUpdateContact] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspAddCity] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspUpdateCity] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspAddState] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspUpdateState] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspGetStats] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspFindAllContactViewPaged] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspFindAllContactPaged] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspFindAllCityViewPaged] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspFindAllCityPaged] TO [dbuser]

GRANT EXECUTE ON [dbo].[uspAddContact] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspUpdateContact] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspAddCity] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspUpdateCity] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspAddState] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspUpdateState] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspGetStats] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspFindAllContactViewPaged] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspFindAllContactPaged] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspFindAllCityViewPaged] TO [dbuser]
GRANT EXECUTE ON [dbo].[uspFindAllCityPaged] TO [dbuser]

-- Used for regression testing of stored procedures
GRANT EXECUTE ON [dbo].[uspStoredProc] TO [dbuser]

GO

