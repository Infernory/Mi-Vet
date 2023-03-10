USE [MiVet]
GO
/****** Object:  StoredProcedure [dbo].[SurveyQuestions_Insert]    Script Date: 11/18/2022 7:41:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author: Ryan Pabalan
-- Create date: 11/16/2022
-- Description: Inserts Survey Questions
-- Code Reviewer: Simon Dilger

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[SurveyQuestions_Insert]
            @UserId int
           ,@Question nvarchar(500)
           ,@HelpText nvarchar(255)
           ,@IsRequired bit
           ,@IsMultipleAllowed bit
           ,@QuestionTypeId int
           ,@SurveyId int
           ,@StatusId int
           ,@SortOrder int
		   ,@Id int OUTPUT
AS
/* ==TEST CODE==

DECLARE     @UserId int = 74
           ,@Question nvarchar(500) = 'The text of the question.'
           ,@HelpText nvarchar(255) = 'Some help to describe further the question or give some hints about the question.'
           ,@IsRequired bit = 1
           ,@IsMultipleAllowed bit = 1
           ,@QuestionTypeId int = 1
           ,@SurveyId int = 1
           ,@StatusId int = 1
           ,@SortOrder int = 1
		   ,@Id int = 0;
EXEC [dbo].[SurveyQuestions_Insert]
			@UserId
           ,@Question
           ,@HelpText
           ,@IsRequired
           ,@IsMultipleAllowed
           ,@QuestionTypeId
           ,@SurveyId
           ,@StatusId
           ,@SortOrder
		   ,@Id OUTPUT
EXEC [dbo].[SurveyQuestions_Select_ById] @Id

*/
BEGIN
INSERT INTO [dbo].[SurveyQuestions]
           ([UserId]
           ,[Question]
           ,[HelpText]
           ,[IsRequired]
           ,[IsMultipleAllowed]
           ,[QuestionTypeId]
           ,[SurveyId]
           ,[StatusId]
           ,[SortOrder])
     VALUES
           (@UserId
           ,@Question
           ,@HelpText
           ,@IsRequired
           ,@IsMultipleAllowed
           ,@QuestionTypeId
           ,@SurveyId
           ,@StatusId
           ,@SortOrder)
		SET @Id = SCOPE_IDENTITY();
END
GO
