USE [MiVet]
GO
/****** Object:  StoredProcedure [dbo].[SurveyQuestions_Update]    Script Date: 11/18/2022 7:41:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author: Ryan Pabalan
-- Create date: 11/16/2022
-- Description: Updates Survey Questions
-- Code Reviewer: Simon Dilger

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[SurveyQuestions_Update]
            @UserId int
           ,@Question nvarchar(500)
           ,@HelpText nvarchar(255)
           ,@IsRequired bit
           ,@IsMultipleAllowed bit
           ,@QuestionTypeId int
           ,@SurveyId int
           ,@StatusId int
           ,@SortOrder int
		   ,@Id int
AS
/* ==TEST CODE==

DECLARE @Id int = 8;
DECLARE     @UserId int = 30
           ,@Question nvarchar(500) = 'The text of the updated question.'
           ,@HelpText nvarchar(255) = 'Some update to describe further the question or give some hints about the question.'
           ,@IsRequired bit = 1
           ,@IsMultipleAllowed bit = 0
           ,@QuestionTypeId int = 1
           ,@SurveyId int = 1
           ,@StatusId int = 1
           ,@SortOrder int = 1
EXEC [dbo].[SurveyQuestions_Update]
			@UserId
           ,@Question
           ,@HelpText
           ,@IsRequired
           ,@IsMultipleAllowed
           ,@QuestionTypeId
           ,@SurveyId
           ,@StatusId
           ,@SortOrder
		   ,@Id
EXEC [dbo].[SurveyQuestions_Select_ById] @Id

*/
BEGIN
DECLARE  @DateModified datetime2(7) = GETUTCDATE()
UPDATE	[dbo].[SurveyQuestions]
SET         [UserId]=@UserId
           ,[Question]=@Question
           ,[HelpText]=@HelpText
           ,[IsRequired]=@IsRequired
           ,[IsMultipleAllowed]=@IsMultipleAllowed
           ,[QuestionTypeId]=@QuestionTypeId
           ,[SurveyId]=@SurveyId
           ,[StatusId]=@StatusId
           ,[SortOrder]=@SortOrder
		   ,[DateModified]=@DateModified
WHERE	Id = @Id
END
GO
