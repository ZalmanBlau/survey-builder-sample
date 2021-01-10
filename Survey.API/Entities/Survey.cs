using System.Collections.Generic;

namespace Survey.API.Entities
{
    public class Survey
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<SurveyResponse> SurveyResponses { get; set; }
        public IEnumerable<SurveyQuestion> SurveyQuestions { get; set; }
    }
}