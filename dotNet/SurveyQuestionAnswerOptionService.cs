using Newtonsoft.Json;
using Sabio.Data.Providers;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Models.Requests.SurveyQuestions;
using Sabio.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Sabio.Data;
using Sabio.Models.Domain.Users;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class SurveyQuestionAnswerOptionService : ISurveyQuestionAnswerOptionService
    {
        IDataProvider _data = null;
        public SurveyQuestionAnswerOptionService(IDataProvider data)
        {
            _data = data;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[SurveyQuestionAnswerOptions_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);

            }, returnParameters: null);
        }
        public void Update(SurveyQuestionAnswerOptionUpdateRequest model, int currentUser)
        {
            string procName = "[dbo].[SurveyQuestionAnswerOptions_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@CreatedBy", currentUser);
                col.AddWithValue("@Id", model.Id);
                AddCommonParams(model, col);
            }, returnParameters: null);
        }
        public int Insert(SurveyQuestionAnswerOptionAddRequest model, int currentUser)
        {
            int id = 0;
            string procName = "[dbo].[SurveyQuestionAnswerOptions_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@CreatedBy", currentUser);
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
        public Paged<SurveyQuestionAnswerOption> GetAllByPagination(int pageIndex, int pageSize)
        {
            Paged<SurveyQuestionAnswerOption> pagedResult = null;
            List<SurveyQuestionAnswerOption> result = null;
            int totalCount = 0;
            _data.ExecuteCmd(
                "[dbo].[SurveyQuestionAnswerOptions_SelectAll]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {

                    SurveyQuestionAnswerOption aSQ = MapSingleSurveyQuestionAnswerOption(reader);

                    int index = 0;
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }


                    if (result == null)
                    {
                        result = new List<SurveyQuestionAnswerOption>();
                    }
                    result.Add(aSQ);
                }
            );
            if (result != null)
            {
                pagedResult = new Paged<SurveyQuestionAnswerOption>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }
        public Paged<SurveyQuestionAnswerOption> GetByUserIdPaginated(int pageIndex, int pageSize, string userId)
        {
            Paged<SurveyQuestionAnswerOption> pagedResult = null;
            List<SurveyQuestionAnswerOption> result = null;
            int totalCount = 0;
            _data.ExecuteCmd(
                "[dbo].[SurveyQuestionAnswerOptions_Select_ByCreatedBy]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@UserId", userId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    SurveyQuestionAnswerOption aSQ = MapSingleSurveyQuestionAnswerOption(reader);
                    int index = 0;
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }
                    if (result == null)
                    {
                        result = new List<SurveyQuestionAnswerOption>();
                    }
                    result.Add(aSQ);
                }
            );
            if (result != null)
            {
                pagedResult = new Paged<SurveyQuestionAnswerOption>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }
        public SurveyQuestionAnswerOption GetById(int id)
        {
            string procName = "[dbo].[SurveyQuestionAnswerOptions_Select_ById]";
            SurveyQuestionAnswerOption sQ = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                sQ = MapSingleSurveyQuestionAnswerOption(reader);
            }
            );
            return sQ;
        }
        private static void AddCommonParams(SurveyQuestionAnswerOptionAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@QuestionId", model.QuestionId);
            col.AddWithValue("@Text", model.Text);
            col.AddWithValue("@Value", model.Value);
            col.AddWithValue("@AdditionalInfo", model.AdditionalInfo);
        }
        private static SurveyQuestionAnswerOption MapSingleSurveyQuestionAnswerOption(IDataReader reader)
        {
            SurveyQuestionAnswerOption aSQAO = new SurveyQuestionAnswerOption();
            int startingIndex = 0;
            aSQAO = new SurveyQuestionAnswerOption();
            aSQAO.Id = reader.GetSafeInt32(startingIndex++);
            aSQAO.QuestionId = reader.GetSafeInt32(startingIndex++);
            aSQAO.Text = reader.GetSafeString(startingIndex++);
            aSQAO.Value = reader.GetSafeString(startingIndex++);
            aSQAO.AdditionalInfo = reader.GetSafeString(startingIndex++);

            aSQAO.CreatedBy = reader.DeserializeObject<BaseUserProfile>(startingIndex++);

            aSQAO.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aSQAO.DateModified = reader.GetSafeDateTime(startingIndex++);
            return aSQAO;
        }
    }
}
