CREATE USER [ILRReferenceData_RW_User]
    WITH PASSWORD = N'$(RWUserPassword)';
GO
	GRANT CONNECT TO [ILRReferenceData_RW_User]
GO


