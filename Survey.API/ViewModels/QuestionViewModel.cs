using System.Collections.Generic;
using Survey.API.Enums;

namespace Survey.API.ViewModels
{
    public class QuestionViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public QuestionType QuestionType { get; set; }
        public IEnumerable<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
        public IDictionary<int, AnswerViewModel> AnswersByAnswerID { get; set; } = new Dictionary<int, AnswerViewModel>();
    }
}