SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* Schedule Data */
CREATE VIEW [dbo].[vw_Maxient_Feed2]
AS
SELECT
	e.[SID]
	,c.CourseID
	,c.Section
	,ISNULL(i.Room, '') AS Room
	,CASE ISNULL(f.AliasName, '')
		WHEN '' THEN ISNULL(f.FullName, '(Staff)')
		ELSE f.AliasName
	 END AS InstructorName
FROM ODS.dbo.vw_Enrollment e
INNER JOIN ODS.dbo.vw_Class c ON c.ClassID = e.ClassID
INNER JOIN ODS.dbo.vw_Instruction i ON i.ClassID = c.ClassID
LEFT OUTER JOIN ODS.dbo.vw_Employee f ON f.[SID] = i.InstructorSID
/* TODO: Dynamically identify "current" quarter */
WHERE e.YearQuarterID IN (
		/* Most recently completed quarter */
		SELECT TOP 1 y.YearQuarterID FROM ODS.dbo.vw_YearQuarter y
		WHERE y.YearQuarterID <> 'Z999' AND y.LastClassDay < GETDATE()
		ORDER BY y.YearQuarterID DESC
)
--ORDER BY e.[SID]
GO
