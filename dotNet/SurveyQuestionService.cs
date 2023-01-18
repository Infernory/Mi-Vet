using Sabio.Data.Providers;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Data;
using Sabio.Models;
using Sabio.Models.Requests.SurveyQuestions;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Domain.Users;
using Sabio.Models.Domain;

namespace Sabio.Services
{
    public class SurveyQuestionService : ISurveyQuestionService
    {
        ILookUpService _lookUpService = null;
        IDataProvider _data = null;
        IUserService _userService = null;
        public SurveyQuestionService(IDataProvider data, ILookUpService lookUpService, IUserService user)
        {
            _data = data;
            _lookUpService = lookUpService;
            _userService = user;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[SurveyQuestions_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);

            }, returnParameters: null);
        }
        public void Update(SurveyQuestionUpdateRequest model, int currentUser)
        {
            string procName = "[dbo].[SurveyQuestions_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@UserId", currentUser);
                AddCommonParams(model, col);
            }, returnParameters: null);
        }
        public int Insert(SurveyQuestionAddRequest model, int currentUser)
        {
            int id = 0;
            string procName = "[dbo].[SurveyQuestions_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", currentUser);
                AddCommonParams(model, col);
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);
            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });
            return id;
        }
        public Paged<SurveyQuestion> GetAllByPagination(int pageIndex, int pageSize)
        {
            Paged<SurveyQuestion> pagedResult = null;

            List<SurveyQuestion> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[SurveyQuestions_SelectAllV2]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {

                    SurveyQuestion question = MapSingleSurveyQuestionForAnswersDetailed(reader);

                    int index = 0;
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }


                    if (result == null)
                    {
                        result = new List<SurveyQuestion>();
                    }

                    result.Add(question);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<SurveyQuestion>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }
        public Paged<Survey> GetSurveyByPagination(int pageIndex, int pageSize)
        {
            Paged<Survey> pagedResult = null;
            List<Survey> result = null;
            int totalCount = 0;
            _data.ExecuteCmd("[dbo].[Surveys_SelectAll_SurveyDetails]",
                delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                 delegate (IDataReader reader, short set)
                 {
                     int i = 0;
                     Survey question = MapSingleSurveyQuestion(reader, ref i);
                     if (totalCount == 0)
                     {
                         totalCount = reader.GetSafeInt32(i++);
                     }
                     if (result == null)
                     {
                         result = new List<Survey>();
                     }
                     result.Add(question);
                 }
            );
            if (result != null)
            {
                pagedResult = new Paged<Survey>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }
        public Paged<SurveyQuestion> GetByUserIdPaginated(int pageIndex, int pageSize, string userId)
        {
            Paged<SurveyQuestion> pagedResult = null;
            List<SurveyQuestion> result = null;
            int totalCount = 0;
            _data.ExecuteCmd(
                "[dbo].[SurveyQuestions_Select_ByCreatedBy]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@UserId", userId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    SurveyQuestion question = MapSingleSurveyQuestionForAnswersDetailed(reader);
                    int index = 0;
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }
                    if (result == null)
                    {
                        result = new List<SurveyQuestion>();
                    }
                    result.Add(question);
                }
            );
            if (result != null)
            {
                pagedResult = new Paged<SurveyQuestion>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }
        public SurveyQuestion GetById(int id)
        {
            string procName = "[dbo].[SurveyQuestions_SelectByIdV2]";
            SurveyQuestion question = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                question = MapSingleSurveyQuestionForAnswersDetailed(reader);
            }
            );
            return question;
        }
        public int InsertBatch(SurveyQuestionsBatchAddRequest model, int createdBy)
        {
            int id = 0;
            _data.ExecuteNonQuery(
                "[dbo].[SurveyQuestions_Insert_Questions]",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    AddSurveyParams(model, paramCollection, createdBy);
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    paramCollection.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@Id"].Value.ToString(), out id);
                });
            return id;
        }
        public void UpdateBatch(SurveyQuestionsBatchUpdateRequest model, int modifiedBy)
        {
            _data.ExecuteNonQuery(
                "[dbo].[SurveyQuestions_Update_Questions]",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", model.Id);
                    AddSurveyParams(model, paramCollection, modifiedBy);
                },
                returnParameters: null);
        }
        private static void AddCommonParams(SurveyQuestionAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Question", model.Question);
            col.AddWithValue("@HelpText", model.HelpText);
            col.AddWithValue("@IsRequired", model.IsRequired);
            col.AddWithValue("@IsMultipleAllowed", model.IsMultipleAllowed);
            col.AddWithValue("@QuestionTypeId", model.QuestionTypeId);
            col.AddWithValue("@SurveyId", model.SurveyId);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@SortOrder", model.SortOrder);
        }
        private SurveyQuestion MapSingleSurveyQuestionForAnswersDetailed(IDataReader reader)
        {
            SurveyQuestion forAnswers = new SurveyQuestion();
            forAnswers.Survey = new Survey();
            forAnswers.Survey.Status = new LookUp();
            forAnswers.Survey.SurveyType = new LookUp();
            BaseUserProfile user = new BaseUserProfile();

            int startingIndex = 0;

            forAnswers.Id = reader.GetSafeInt32(startingIndex++);
            forAnswers.UserId = reader.GetSafeInt32(startingIndex++);
            forAnswers.Question = reader.GetSafeString(startingIndex++);
            forAnswers.HelpText = reader.GetSafeString(startingIndex++);
            forAnswers.IsRequired = reader.GetSafeBool(startingIndex++);
            forAnswers.IsMultipleAllowed = reader.GetSafeBool(startingIndex++);
            forAnswers.QuestionTypeId = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            forAnswers.Survey.Id = reader.GetSafeInt32(startingIndex++);
            forAnswers.Survey.Name = reader.GetSafeString(startingIndex++);
            forAnswers.Survey.Description = reader.GetSafeString(startingIndex++);
            forAnswers.Survey.Status = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            forAnswers.Survey.SurveyType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            forAnswers.Survey.CreatedBy = _userService.MapBaseUserProfile(reader, ref startingIndex);
            forAnswers.Survey.DateCreated = reader.GetSafeDateTime(startingIndex++);
            forAnswers.Survey.DateModified = reader.GetSafeDateTime(startingIndex++);
            forAnswers.SurveyStatusId = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            forAnswers.SortOrder = reader.GetSafeInt32(startingIndex++);
            forAnswers.DateCreated = reader.GetSafeDateTime(startingIndex++);
            forAnswers.DateModified = reader.GetSafeDateTime(startingIndex++);
            return forAnswers;
        }
        private Survey MapSingleSurveyQuestion(IDataReader reader, ref int i)
        {
            Survey question = new();
            question.Id = reader.GetSafeInt32(i++);
            question.Name = reader.GetSafeString(i++);
            question.Description = reader.GetSafeString(i++);
            question.Status = _lookUpService.MapSingleLookUp(reader, ref i);
            question.SurveyType = _lookUpService.MapSingleLookUp(reader, ref i);
            question.CreatedBy = _userService.MapBaseUserProfile(reader, ref i);
            question.SurveyQuestions = reader.DeserializeObject<List<Question>>(i++);
            question.DateCreated = reader.GetSafeDateTime(i++);
            question.DateModified = reader.GetSafeDateTime(i++);
            return question;
        }
        private static void AddSurveyParams(SurveyQuestionsBatchAddRequest request, SqlParameterCollection paramCollection, int createdBy)
        {
            DataTable dtQuestions = null;
            if (request.SurveyQuestions != null)
            {
                dtQuestions = MapQuestionsToTable(request.SurveyQuestions);
            }
            paramCollection.AddWithValue("Name", request.Name);
            paramCollection.AddWithValue("Description", request.Description);
            paramCollection.AddWithValue("StatusId", request.StatusId);
            paramCollection.AddWithValue("SurveyTypeId", request.SurveyTypeId);
            paramCollection.AddWithValue("@CreatedBy", createdBy);
            if (dtQuestions != null)
            {
                paramCollection.AddWithValue("@SurveyQuestions", dtQuestions);
            }
        }
        private static DataTable MapQuestionsToTable(List<SurveyQuestionAddRequest> questionToMap)
        {
            DataTable dt = new DataTable();
            if (questionToMap != null)
            {
                dt.Columns.Add("UserId", typeof(int));
                dt.Columns.Add("Question", typeof(string));
                dt.Columns.Add("HelpText", typeof(string));
                dt.Columns.Add("IsRequired", typeof(bool));
                dt.Columns.Add("IsMultipleAllowed", typeof(bool));
                dt.Columns.Add("QuestionTypeId", typeof(int));
                dt.Columns.Add("SurveyId", typeof(int));
                dt.Columns.Add("StatusId", typeof(int));
                dt.Columns.Add("SortOrder", typeof(int));

                foreach (var singleQuestion in questionToMap)
                {
                    DataRow dr = dt.NewRow();
                    dr.SetField("UserId", singleQuestion.UserId);
                    dr.SetField("Question", singleQuestion.Question);
                    dr.SetField("HelpText", singleQuestion.HelpText);
                    dr.SetField("IsRequired", singleQuestion.IsRequired);
                    dr.SetField("IsMultipleAllowed", singleQuestion.IsMultipleAllowed);
                    dr.SetField("QuestionTypeId", singleQuestion.QuestionTypeId);
                    dr.SetField("SurveyId", singleQuestion.SurveyId);
                    dr.SetField("StatusId", singleQuestion.StatusId);
                    dr.SetField("SortOrder", singleQuestion.SortOrder);
                    dt.Rows.Add(dr);
                }
            }
            return dt;

        }
    }
}
