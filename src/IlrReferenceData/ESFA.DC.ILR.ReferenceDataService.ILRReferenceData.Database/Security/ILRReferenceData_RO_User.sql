CREATE USER [ILRReferenceData_RO_User]
    WITH PASSWORD = N'$(ROUserPassword)';
GO
	GRANT CONNECT TO [ILRReferenceData_RO_User]
GO


