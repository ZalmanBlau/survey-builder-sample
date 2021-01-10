using System.Collections.Generic;

namespace Survey.API.Entities
{
    public class SurveyResponse
    {
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public IEnumerable<SurveyQuestionResponse> SurveyQuestionResponses { get; set; }
    }
}