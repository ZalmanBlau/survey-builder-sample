using System.Collections.Generic;
using Survey.API.Enums;

namespace Survey.API.Entities
{
    public class SurveyQuestion
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public QuestionType QuestionType { get; set; }
        public int SurveyID { get; set; }
        public IEnumerable<QuestionAnswer> QuestionAnswers { get; set; }
    }
}