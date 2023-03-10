USE [MiVet]
GO
/****** Object:  StoredProcedure [dbo].[SurveyQuestionAnswerOptions_Insert]    Script Date: 11/18/2022 7:41:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Ryan Pabalan
-- Create date: 11/16/2022
-- Description: Inserts Survey Questions Answers Options
-- Code Reviewer: Simon Dilger

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[SurveyQuestionAnswerOptions_Insert]
			@QuestionId int
           ,@Text nvarchar(500)
           ,@Value nvarchar(100)
           ,@AdditionalInfo nvarchar(200)
           ,@CreatedBy int
		   ,@Id int OUTPUT
AS
/* ==TEST CODE==

DECLARE     @QuestionId int = 12
           ,@Text nvarchar(500) = 'The text of the answer option. This may be null if a value is provided instead.'
           ,@Value nvarchar(100) = 'A possible value as the answer option. This may be null if a text is provided.'
           ,@AdditionalInfo nvarchar(200) = 'For one of the Survey types, we have to provide to the user an additional piece...if.'
           ,@CreatedBy int = 35
		   ,@Id int = 0;
EXEC [dbo].[SurveyQuestionAnswerOptions_Insert]
			@QuestionId
           ,@Text
           ,@Value
           ,@AdditionalInfo
           ,@CreatedBy
		   ,@Id OUTPUT
EXEC [dbo].[SurveyQuestionAnswerOptions_Select_ById] @Id

*/
BEGIN
INSERT INTO [dbo].[SurveyQuestionAnswerOptions]
           ([QuestionId]
           ,[Text]
           ,[Value]
           ,[AdditionalInfo]
           ,[CreatedBy])
     VALUES
           (@QuestionId
           ,@Text
           ,@Value
           ,@AdditionalInfo
           ,@CreatedBy)
		SET @Id = SCOPE_IDENTITY();
END
GO
