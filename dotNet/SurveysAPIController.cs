using Amazon.Runtime.Internal.Util;
using Amazon.S3.Model.Internal.MarshallTransformations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests.Surveys;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.CodeDom;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/surveys")]
    [ApiController]
    public class SurveysApiController : BaseApiController
    {
        private ISurveyService _service = null;
        private IAuthenticationService<int> _auth = null;
        public SurveysApiController(ISurveyService service
            ,IAuthenticationService<int> authService
            ,ILogger<SurveysApiController> logger) : base(logger)
        {
            _service = service;
            _auth = authService;
        }
        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Survey survey = _service.GetSurveyById(id);
                if (survey == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found");
                }
                else
                {
                    response = new ItemResponse<Survey>() { Item = survey };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate")]
        public ActionResult GetPaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Survey> survey = _service.GetSurveysPaginated(pageSize, pageIndex);
                if (survey == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Survey>>() { Item = survey };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("search/{creatorId:int}")]
        public ActionResult GetByCreator(int pageSize, int pageIndex, int creatorId)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Survey> survey = _service.GetSurveyByCreator(creatorId, pageSize, pageIndex);
                if (survey == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Survey>>() { Item = survey };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpPost]
        public ActionResult InsertSurvey(SurveyAddRequest request)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int userId = _auth.GetCurrentUserId();
                int id = _service.InsertSurvey(request, userId);
                response = new ItemResponse<int>() { Item = id };
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateSurvey(SurveyUpdateRequest request)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.UpdateSurvey(request);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteSurvey(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.DeleteSurvey(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpGet("analytics/{id:int}")]
        public ActionResult GetAnalyticsById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                SurveyAnalytics survey = _service.GetSurveyAnalyticsById(id);
                if (survey == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found");
                }
                else
                {
                    response = new ItemResponse<SurveyAnalytics>() { Item = survey };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpGet("analytics")]
        public ActionResult GetAnalytics(int pageSize, int pageIndex)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<SurveyAnalytics> survey = _service.GetAnalyticsPaginated(pageSize, pageIndex);
                if (survey == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyAnalytics>>() { Item = survey };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpGet("analytics/search")]
        public ActionResult SearchAnalytics(int pageSize, int pageIndex, string query)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<SurveyAnalytics> survey = _service.SearchAnalyticsPaginated(pageSize, pageIndex, query);
                if (survey == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyAnalytics>>() { Item = survey };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpGet("analytics/averages/{id:int}")]
        public ActionResult GetAveragesById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                SurveyAverages survey = _service.GetSurveyAverages(id);
                if (survey == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found");
                }
                else
                {
                    response = new ItemResponse<SurveyAverages>() { Item = survey };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
    }
}
