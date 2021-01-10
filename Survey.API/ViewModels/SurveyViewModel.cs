using System.Collections.Generic;

namespace Survey.API.ViewModels
{
    public class SurveyViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
        public IDictionary<int, QuestionViewModel> QuestionsByQuestionID { get; set; } = new Dictionary<int, QuestionViewModel>();
        public IEnumerable<SurveyResponseViewModel> SurveyResponses { get; set; } = new List<SurveyResponseViewModel>();
    }
}