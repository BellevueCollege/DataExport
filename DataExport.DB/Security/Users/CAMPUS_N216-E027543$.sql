IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'CAMPUS\N216-E027543$')
CREATE LOGIN [CAMPUS\N216-E027543$] FROM WINDOWS
GO
CREATE USER [CAMPUS\N216-E027543$] FOR LOGIN [CAMPUS\N216-E027543$]
GO
