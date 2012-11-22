
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

/*******************************************************************
 * Maxient Data Feed 1 - Demographic Data
 *******************************************************************/
CREATE VIEW [dbo].[vw_Maxient_Feed1]
AS
	SELECT
		s.[SID]
		,ISNULL(s.NTUserName, '') AS Username
		,ISNULL(s.LastName, '') AS "Last Name"
		,ISNULL(s.FirstName, '') AS "First Name"
		,ISNULL(s.MiddleInitial, '') AS "Middle Name"
		,s.DOB AS "Date of Birth"
		,CASE s.Sex
			WHEN 'F' THEN 'Female'
			WHEN 'M' THEN 'Male'
			ELSE ''
		 END AS "Gender"
		,ISNULL((SELECT TOP 1 e.Title FROM ODS.dbo.vw_Ethnicity e WHERE e.EthnicityID = s.EthnicityID), '') AS Ethnicity
		,ISNULL(s.EveningPhone, ISNULL(s.DaytimePhone, '')) AS "Local Phone"
		,'' AS "Cell Phone"
		,ISNULL(s.[Address], '') AS "Permanent address"
		,ISNULL(s.City, '') AS "Permanent city"
		,ISNULL(s.[State], '') AS "Permanent state"
		,ISNULL(s.Zip, '') AS "Permanent zip"
		--,s.Address1
		--,s.City1
		--,s.State1
		--,s.Zip1
		,ISNULL(CAST(
			CAST((
				/* GPA for the most recently completed quarter */
				SELECT SUM(g.GradePointValue) / COUNT(t.GradeID)
				FROM ODS.dbo.vw_Transcript t
				INNER JOIN	ODS.dbo.vw_Grade g ON g.GradeID = t.GradeID
				WHERE t.[SID] = s.[SID]
				AND t.YearQuarterID = (
					/* Most recently completed quarter */
					SELECT TOP 1 y.YearQuarterID FROM ODS.dbo.vw_YearQuarter y
					WHERE y.YearQuarterID <> 'Z999' AND y.LastClassDay < GETDATE()
					ORDER BY y.YearQuarterID DESC
				)
			) AS DECIMAL(5,3)) AS VARCHAR), ''
		 ) AS "GPA Most recent term"
		,CAST(s.CumulGPA AS DECIMAL(5,3)) AS "GPA Cumulative"
	FROM ODS.dbo.vw_Student s
	WHERE NOT ISNULL(s.[SID], '') IN (
		''
	)
	AND EXISTS(SELECT * FROM ODS.dbo.vw_Enrollment n WHERE n.YearQuarterID >= 'B122' AND n.[SID] = s.[SID])

GO
