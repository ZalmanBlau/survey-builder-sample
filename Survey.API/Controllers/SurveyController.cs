using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.API.Code;
using Survey.API.Entities;
using Survey.API.ViewModels;

namespace Survey.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly SurveyDBContext _dbContext;

        public SurveyController(SurveyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/Survey
        [HttpPost]
        public bool POST([FromBody] SurveyViewModel survey)
        {
            SurveyProccessor.CreateNewForm(_dbContext, survey);
            return true;
        }


        [HttpGet("{surveyId}", Name = "GetSurveyFormByID")]
        public SurveyViewModel Get(int surveyId)
        {
            return SurveyProccessor.GetForm(_dbContext, surveyId);
        }

        [HttpGet("{surveyId}/response")]
        public SurveyViewModel GetAllResponses(int surveyId)
        {
            return SurveyProccessor.GetAllResponses(_dbContext, surveyId);
        }

        [HttpPost("response")]
        public void CreateResponse([FromBody] SurveyViewModel survey)
        {
            SurveyProccessor.CreateNewResponse(_dbContext, survey);
        }
    }
}
