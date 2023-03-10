USE [MiVet]
GO
/****** Object:  StoredProcedure [dbo].[SurveyQuestions_Select_ById]    Script Date: 11/18/2022 7:41:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Ryan Pabalan
-- Create date: November 16, 2022
-- Description: Returns one Survey Question by it's Id #.
-- Code Reviewer: Simon Dilger

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer: 
-- Note: 
-- =============================================

CREATE PROC [dbo].[SurveyQuestions_Select_ById]
			 @Id int
AS
/*	==TEST CODE==

DECLARE @Id int = 12
EXEC [dbo].[SurveyQuestions_Select_ById] @Id

*/
BEGIN
SELECT [s].[Id]
		  ,[u].[Id] AS UserId
		  ,[s].[Question]
		  ,[s].[HelpText]
		  ,[s].[IsRequired]
		  ,[s].[IsMultipleAllowed]
		  ,[qT].[Id] AS QuestionTypeId
		  ,[qT].[Name] AS QuestionTypeName
		  ,[su].[Id] AS SurveyId
		  ,[sS].[Id] AS SurveyStatusId
		  ,[sS].[Name] AS SurveyStatusName
		  ,[s].[SortOrder]
		  ,[s].[DateCreated]
		  ,[s].[DateModified]
		  ,TotalCount = COUNT(1) OVER()
	  FROM [dbo].[SurveyQuestions] AS s
		INNER JOIN [dbo].[QuestionTypes] AS qT
			ON [s].[QuestionTypeId] = [qT].[Id]
		INNER JOIN [dbo].[SurveyStatus] AS sS
			ON [s].[StatusId] = [sS].[Id]
		INNER JOIN [dbo].[Users] AS u
			ON [s].[UserId] = [u].[Id]
		INNER JOIN [dbo].[Surveys] AS su
			ON [s].[SurveyId] = [su].[Id]
		 WHERE s.Id = @Id
END
GO
