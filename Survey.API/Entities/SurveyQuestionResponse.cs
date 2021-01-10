using System.Collections.Generic;

namespace Survey.API.Entities
{
    public class SurveyQuestionResponse
    {
        public int ID { get; set; }
        public int SurveyResponseID { get; set; }
        public int SurveyQuestionID { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
        public List<SurveyQuestionAnswerResponse> SurveyQuestionAnswerResponses { get; set; }
        public SurveyResponse SurveyResponse { get; set; }
    }
}