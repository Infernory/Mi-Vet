USE [MiVet]
GO
/****** Object:  StoredProcedure [dbo].[SurveyQuestions_Update_Questions]    Script Date: 1/3/2023 11:37:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Ryan Pabalan
-- Create date: 12/15/2022
-- Description: Survey Questions Batch Update Questions
-- Code Reviewer: Joshua Yang

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROCEDURE [dbo].[SurveyQuestions_Update_Questions]
	 @Name nvarchar(100)
	,@Description nvarchar(2000)
	,@StatusId int
	,@SurveyTypeId int
	,@CreatedBy int
	,@Id int
	,@SurveyQuestions dbo.SurveyQuestionsUDT READONLY
/*TESTCODE

DECLARE @Id INT = 126
		,@Name NVARCHAR(100) = 'Test Survey'
		,@Description NVARCHAR(2000) = 'This is a test survey'
		,@StatusId INT = 1
		,@SurveyTypeId INT = 1
		,@CreatedBy INT = 74
		,@testSurveyQuestions dbo.SurveyQuestionsUDT
INSERT INTO @testSurveyQuestions (UserId
							,Question
							,HelpText
							,IsRequired
							,IsMultipleAllowed
							,QuestionTypeId
							,SurveyId
							,StatusId
							,SortOrder)
VALUES
    (@CreatedBy
	,'What is color?'
	,'This is the first question'
	,1
	,0
	,1
	,@Id
	,@StatusId
	,1),
    (@CreatedBy
	,'Question 2'
	,'This is the unupdated second question'
	,0
	,1
	,2
	,@Id
	,@StatusId
	,2),

EXEC SurveyQuestions_Update_Questions @Name
									 ,@Description
									 ,@StatusId
									 ,@SurveyTypeId
									 ,@CreatedBy
									 ,@Id
									 ,@testSurveyQuestions
	EXEC  [dbo].[Surveys_SelectAll_SurveyDetails] 0, 100
	EXEC  [dbo].[SurveyQuestions_SelectAll] 0, 100
*/
AS
BEGIN
    UPDATE dbo.Surveys
    SET
        [Name] = @Name
		,[Description] = @Description
		,StatusId = @StatusId
		,SurveyTypeId = @SurveyTypeId
		,CreatedBy = @CreatedBy
    WHERE
        Id = @Id
    MERGE dbo.SurveyQuestions AS TARGET
    USING (SELECT * FROM @SurveyQuestions) AS SOURCE (UserId
													,Question
													,HelpText
													,IsRequired
													,IsMultipleAllowed
													,QuestionTypeId
													,SurveyId
													,StatusId
													,SortOrder)
    ON (TARGET.Question = SOURCE.Question)
    WHEN MATCHED THEN
        UPDATE SET
            TARGET.UserId = @CreatedBy
			,TARGET.HelpText = SOURCE.HelpText
			,TARGET.IsRequired = SOURCE.IsRequired
			,TARGET.IsMultipleAllowed = SOURCE.IsMultipleAllowed
			,TARGET.QuestionTypeId = SOURCE.QuestionTypeId
			,TARGET.SurveyId = @Id
			,TARGET.StatusId = @StatusId
			,TARGET.SortOrder = SOURCE.SortOrder
    WHEN NOT MATCHED THEN
        INSERT (UserId
			,Question
			,HelpText
			,IsRequired
			,IsMultipleAllowed
			,QuestionTypeId
			,SurveyId
			,StatusId
			,SortOrder)
        VALUES (@CreatedBy
			,SOURCE.Question
			,SOURCE.HelpText
			,SOURCE.IsRequired
			,SOURCE.IsMultipleAllowed
			,SOURCE.QuestionTypeId
			,@Id
			,@StatusId
			,SOURCE.SortOrder);
END

GO
