USE [MiVet]
GO
/****** Object:  UserDefinedTableType [dbo].[SurveyQuestionsUDT]    Script Date: 1/3/2023 11:37:03 AM ******/
CREATE TYPE [dbo].[SurveyQuestionsUDT] AS TABLE(
	[UserId] [int] NOT NULL,
	[Question] [nvarchar](500) NOT NULL,
	[HelpText] [nvarchar](255) NOT NULL,
	[IsRequired] [bit] NOT NULL,
	[IsMultipleAllowed] [bit] NOT NULL,
	[QuestionTypeId] [int] NOT NULL,
	[SurveyId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[SortOrder] [int] NOT NULL
)
GO
