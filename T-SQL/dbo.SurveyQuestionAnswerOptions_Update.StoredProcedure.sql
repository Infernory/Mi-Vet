USE [MiVet]
GO
/****** Object:  StoredProcedure [dbo].[SurveyQuestionAnswerOptions_Update]    Script Date: 11/18/2022 7:41:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Ryan Pabalan
-- Create date: 11/16/2022
-- Description: Updates Survey Questions Answers Options
-- Code Reviewer: Simon Dilger

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[SurveyQuestionAnswerOptions_Update]
			@QuestionId int
           ,@Text nvarchar(500)
           ,@Value nvarchar(100)
           ,@AdditionalInfo nvarchar(200)
           ,@CreatedBy int
		   ,@Id int
AS
/* ==TEST CODE==

DECLARE @Id int = 1;
DECLARE     @QuestionId int = 8
           ,@Text nvarchar(500) = 'The UPDATE of the answer option. This may be null if a value is provided instead.'
           ,@Value nvarchar(100) = 'A possible value as the UPDATED answer option. This may be null if a text is provided.'
           ,@AdditionalInfo nvarchar(200) = 'For one of the Survey types, we have to provide to the user an additional piece...if.'
           ,@CreatedBy int = 35
EXEC [dbo].[SurveyQuestionAnswerOptions_Update]
			@QuestionId
           ,@Text
           ,@Value
           ,@AdditionalInfo
           ,@CreatedBy
		   ,@Id
EXEC [dbo].[SurveyQuestionAnswerOptions_Select_ById] @Id
SELECT *
FROM [dbo].[SurveyQuestionAnswerOptions]
SELECT *
FROM [dbo].[SurveyQuestions]

*/
BEGIN
DECLARE		@DateModified datetime2(7) = GETUTCDATE()
UPDATE		[dbo].[SurveyQuestionAnswerOptions]
SET         [QuestionId]=@QuestionId
           ,[Text]=@Text
           ,[Value]=@Value
           ,[AdditionalInfo]=@AdditionalInfo
           ,[CreatedBy]=@CreatedBy
		   ,[DateModified]=@DateModified
WHERE		Id = @Id
END
GO
