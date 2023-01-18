using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Models.Requests.SurveyQuestions;
using Sabio.Models;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Services.Interfaces;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/survey/options")]
    [ApiController]
    public class SurveyQuestionAnswerOptionApiController : BaseApiController
    {
        private ISurveyQuestionAnswerOptionService _service = null;
        private IAuthenticationService<int> _authService = null;
        public SurveyQuestionAnswerOptionApiController(ISurveyQuestionAnswerOptionService service
            , ILogger<SurveyQuestionAnswerOptionApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        #region DeleteById
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> DeleteById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        #endregion
        #region Update
        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(SurveyQuestionAnswerOptionUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int currentUserId = _authService.GetCurrentUserId();
                _service.Update(model, currentUserId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        #endregion
        #region Create
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(SurveyQuestionAnswerOptionAddRequest model)
        {

            ObjectResult result = null;
            try
            {
                int currentUserId = _authService.GetCurrentUserId();
                int id = _service.Insert(model, currentUserId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        #endregion
        #region GetAllByPagination
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<SurveyQuestionAnswerOption>>> GetAllByPagination(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<SurveyQuestionAnswerOption> page = _service.GetAllByPagination(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyQuestionAnswerOption>> { Item = page };
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
        #endregion
        #region GetByUserIdPaginated
        [HttpGet("paginate/user")]
        public ActionResult<ItemResponse<Paged<SurveyQuestionAnswerOption>>> GetByUserIdPaginated(int pageIndex, int pageSize, string userId)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<SurveyQuestionAnswerOption> page = _service.GetByUserIdPaginated(pageIndex, pageSize, userId);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyQuestionAnswerOption>> { Item = page };
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
        #endregion
        #region GetById
        [HttpGet("{id:int}")]
        public ActionResult<ItemsResponse<SurveyQuestionAnswerOption>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                SurveyQuestionAnswerOption friend = _service.GetById(id);
                if (friend == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<SurveyQuestionAnswerOption> { Item = friend };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }
        #endregion
    }
}
