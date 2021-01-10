namespace Survey.API.Entities
{
    public class QuestionAnswer
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public bool IsOfferedAnswer { get; set; }
        public int SurveyQuestionID { get; set; }
    }
}