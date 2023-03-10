USE [MiVet]
GO
/****** Object:  StoredProcedure [dbo].[SurveyQuestions_Delete_ById]    Script Date: 11/18/2022 7:41:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author: Ryan Pabalan
-- Create date: 11/16/2022
-- Description: Deletes Survey Questions by Id
-- Code Reviewer: Simon Dilger

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[SurveyQuestions_Delete_ById]
			@Id int
/*		--TEST CODE--

DECLARE @Id int = 10
EXEC [dbo].[SurveyQuestions_Select_ById] @Id

EXEC [dbo].[SurveyQuestions_Delete_ById] @Id

EXEC [dbo].[SurveyQuestions_Select_ById] @Id
Select * from dbo.SurveyQuestions
Select * from dbo.SurveyQuestionAnswerOptions

*/
AS
BEGIN
DELETE FROM [dbo].[SurveyQuestionAnswerOptions]
      WHERE QuestionId = @Id
DELETE FROM [dbo].[SurveyQuestions]
      WHERE Id = @Id
END
GO
