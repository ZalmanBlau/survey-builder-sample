namespace Survey.API.Entities
{
    public class SurveyQuestionAnswerResponse
    {
        public int ID { get; set; }
        public int SurveyQuestionResponseID { get; set; }
        public int QuestionAnswerID { get; set; }
        public QuestionAnswer QuestionAnswer { get; set; }
        public SurveyQuestionResponse SurveyQuestionResponse { get; set; }
    }
}