USE [DCustomer]
GO

GRANT EXECUTE ON [dbo].[uspAddContact] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspUpdateContact] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspAddCity] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspUpdateCity] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspAddState] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspUpdateState] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspGetStats] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspFindAllContactViewPaged] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspFindAllContactPaged] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspFindAllCityViewPaged] TO [Seagull\wbw07]
GRANT EXECUTE ON [dbo].[uspFindAllCityPaged] TO [Seagull\wbw07]

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

GO
