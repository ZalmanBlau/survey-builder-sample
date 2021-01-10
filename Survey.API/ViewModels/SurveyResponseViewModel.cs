using System.Collections.Generic;

namespace Survey.API.ViewModels
{
    public class SurveyResponseViewModel
    {
        public UserViewModel User { get; set; }
        public IEnumerable<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }
}