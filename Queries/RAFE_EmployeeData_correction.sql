USE [RAFE]
GO


/*According to tblUsers_M */
SELECT [empid] = LTRIM(RTRIM(E.[Employee ID]))
	,[UserName] = LTRIM(RTRIM(A.[SamAccountName]))
	,[FullName] = LTRIM(RTRIM(A.[Name]))
      ,[EmailId]   =  LTRIM(RTRIM(A.[EmailAddress]))
	  ,[RoleID] = 0
      ,[Function_Name] = LTRIM(RTRIM(E.[Function]))
      ,[Group_Name] = LTRIM(RTRIM(E.[Group]))
      ,[Sub_Group] = LTRIM(RTRIM(E.[Sub Group]))
      ,[Legal_Entity] = LTRIM(RTRIM(E.[Legal Entity]))
      ,[DOB] = E.[Date of Birth]
      ,[DOJ] = E.[Date of Joining]
      ,[Location] = LTRIM(RTRIM(E.[Location]))
      ,[DesignationID] = D.DesignationID
      ,[Designation_Title] = LTRIM(RTRIM(E.[Role Title]))
      ,[reportingmanager] = LTRIM(RTRIM(R.[SamAccountName]))
      ,[HOD] = LTRIM(RTRIM(HOD.[SamAccountName]))
      ,[BU_HR] = LTRIM(RTRIM(BUHR.[SamAccountName]))
	  ,0 as iscxo
	  ,0 as isHR
	  ,1 as Acitve
	  ,0 as deleted
	  
	  FROM  EmployeeData E 	  
	INNER JOIN  [dbo].[ADUserdetails] A ON LTRIM(RTRIM(A.emailAddress)) = LTRIM(RTRIM(E.[Email ID]))
	  INNER JOIN tblDesignation_M D ON LTRIM(RTRIM(E.[Raymond Grade])) = LTRIM(RTRIM(D.Designation))
	INNER JOIN  [dbo].[ADUserdetails] R ON LTRIM(RTRIM(R.emailAddress)) = LTRIM(RTRIM(E.[Reporting Manager Email ID]))
  INNER JOIN  [dbo].[ADUserdetails] HOD ON LTRIM(RTRIM(HOD.Name)) = LTRIM(RTRIM(E.[HOD Name]))
  INNER JOIN  [dbo].[ADUserdetails] BUHR ON LTRIM(RTRIM(BUHR.Name)) = LTRIM(RTRIM(E.[BU HR Name]))
  LEFT OUTER JOIN tblusers_M U ON LTRIM(RTRIM(E.[Email ID])) = LTRIM(RTRIM(U.emailID))
  WHERE U.emailid is null 

GO

SELECT * FROM #TTT WHERE empid in (
SELECT empid FROM #TTT GROUP BY empid HAVING COUNT(*)>1)




SELECT E.* INTO #BUHR_Not 
	  FROM  [dbo].[EmployeeData] E 
	  INNER JOIN tblDesignation_M D ON LTRIM(RTRIM(E.[Raymond Grade])) = LTRIM(RTRIM(D.Designation))
	INNER JOIN  [dbo].[ADUserdetails] R ON LTRIM(RTRIM(R.emailAddress)) = LTRIM(RTRIM(E.[Reporting Manager Email ID]))
	INNER  JOIN  [dbo].[ADUserdetails] HOD ON LTRIM(RTRIM(HOD.Name)) = LTRIM(RTRIM(E.[HOD Name]))
  --LEFT OUTER JOIN  [dbo].[ADUserdetails] BUHR ON LTRIM(RTRIM(BUHR.Name)) = LTRIM(RTRIM(E.[BU HR Name]))
  WHERE HOD.Name IS NULL

SELECT * FROM #HOD_Not



SELECT E.* INTO #BUHR_Not 
	  FROM  [dbo].[EmployeeData] E 
	  INNER JOIN tblDesignation_M D ON LTRIM(RTRIM(E.[Raymond Grade])) = LTRIM(RTRIM(D.Designation))
	INNER JOIN  [dbo].[ADUserdetails] R ON LTRIM(RTRIM(R.emailAddress)) = LTRIM(RTRIM(E.[Reporting Manager Email ID]))
	--INNER  JOIN  [dbo].[ADUserdetails] HOD ON LTRIM(RTRIM(HOD.Name)) = LTRIM(RTRIM(E.[HOD Name]))
  LEFT OUTER JOIN  [dbo].[ADUserdetails] BUHR ON LTRIM(RTRIM(BUHR.Name)) = LTRIM(RTRIM(E.[BU HR Name]))
  WHERE BUHR.Name IS NULL

  

SELECT * FROM  #BUHR_Not  








  

SELECT T.* FROM #TTT T
LEFT OUTER JOIN tblusers_M U ON T.empID = U.empID
WHERE  U.username is null AND T.username !='kanarayan'
ORDER BY U.username 

SELECT * FROM tblusers_M WHERE username ='kanarayan'