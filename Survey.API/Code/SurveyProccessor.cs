using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Survey.API.Entities;
using Survey.API.Enums;
using Survey.API.ViewModels;

namespace Survey.API.Code
{
    public class SurveyProccessor
    {
        public static void CreateNewForm(SurveyDBContext dbContext, SurveyViewModel surveyViewModel)
        {
            var survey = new Entities.Survey
            {
                Title = surveyViewModel.Title,
                Description = surveyViewModel.Description,
            };

            dbContext.Survey.Add(survey);
            dbContext.SaveChanges();

            foreach (var question in surveyViewModel.Questions)
            {
                var surveyQuestionEntity =
                    CreateNewSurveyQuestionRecord(dbContext, question, survey.ID);

                foreach (var questionAnswer in question.Answers)
                {
                    var questionAnswerEntity = CreateNewQuestionAnswerRecord(
                        dbContext, questionAnswer, surveyQuestionEntity.ID, save: false);

                    questionAnswerEntity.IsOfferedAnswer = true;
                    dbContext.QuestionAnswer.Add(questionAnswerEntity);
                }

                dbContext.SaveChanges();
            }
        }

        public static SurveyViewModel GetForm(SurveyDBContext dbContext, int surveyId)
        {
            var survey = dbContext.Survey
                .Include(s => s.SurveyQuestions)
                .ThenInclude(sq => sq.QuestionAnswers)
                .Select(s => new SurveyViewModel
                {
                    ID = s.ID,
                    Title = s.Title,
                    Description = s.Description,
                    Questions = s.SurveyQuestions
                        .Select(sq => new QuestionViewModel
                        {
                            ID = sq.ID,
                            Title = sq.Title,
                            QuestionType = sq.QuestionType,
                            Answers = sq.QuestionAnswers
                                .Where(qa => qa.IsOfferedAnswer)
                                .Select(qa => new AnswerViewModel
                                {
                                    ID = qa.ID,
                                    Text = qa.Text,
                                }),
                        }),
                }).FirstOrDefault(s => s.ID == surveyId);

            return survey;
        }

        public static SurveyViewModel GetAllResponses(
            SurveyDBContext dbContext,
            int surveyId)
        {
            var survey = dbContext.Survey
                .Include(s => s.SurveyQuestions)
                .ThenInclude(sq => sq.QuestionAnswers)
                .Include(s => s.SurveyResponses)
                .ThenInclude(sr => sr.User)
                .Include(s => s.SurveyResponses)
                .ThenInclude(sr => sr.SurveyQuestionResponses)
                .ThenInclude(srq => srq.SurveyQuestionAnswerResponses)
                .ThenInclude(srqa => srqa.QuestionAnswer)
                .Where(s => s.ID == surveyId)
                .Select(s => new SurveyViewModel
                {
                    ID = s.ID,
                    Title = s.Title,
                    Description = s.Description,
                    Questions = s.SurveyQuestions.Select(
                        sq => new QuestionViewModel
                        {
                            ID = sq.ID,
                            Title = sq.Title,
                            QuestionType = sq.QuestionType,
                            Answers = sq.QuestionAnswers.Select(
                                qa => new AnswerViewModel
                                {
                                    ID = qa.ID,
                                    Text = qa.Text,
                                }),
                        }),
                    SurveyResponses = s.SurveyResponses.Select(sr => new SurveyResponseViewModel
                    {
                        User = new UserViewModel
                        {
                            ID = sr.User.ID,
                            FirstName = sr.User.FirstName,
                            LastName = sr.User.LastName,
                            Email = sr.User.Email,
                        },
                        Questions = sr.SurveyQuestionResponses.Select(srq => new QuestionViewModel
                        {
                            ID = srq.SurveyQuestion.ID,
                            QuestionType = srq.SurveyQuestion.QuestionType,
                            Answers = srq.SurveyQuestionAnswerResponses.Select(srqa => new AnswerViewModel
                            {
                                ID = srqa.QuestionAnswer.ID,
                            }),
                        }),
                    }),
                }).FirstOrDefault();

            //in memory translation to dictionary bc ef core server side translation
            //to dictionary is not supported
            survey.QuestionsByQuestionID = survey.Questions.ToDictionary(
                q => q.ID,
                q => new QuestionViewModel
                {
                    ID = q.ID,
                    Title = q.Title,
                    AnswersByAnswerID = q.Answers.ToDictionary(
                        a => a.ID,
                        a => a),
                });

            survey.Questions = new List<QuestionViewModel>();

            return survey;
        }

        public static void CreateNewResponse(SurveyDBContext dbContext, SurveyViewModel survey)
        {
            var surveyResponse = survey.SurveyResponses.First();
            var userEntity = CreateNewUserRecord(dbContext, surveyResponse.User);
            var surveyResponseEntity = CreateNewSurveyResponseRecord(dbContext, survey.ID, userEntity.ID);
            var surveyQuestionResponseByQuestionId = CreateNewSurveyQuestionResponseRecord(
                    dbContext, surveyResponse.Questions, surveyResponseEntity.ID);

            AnswerViewModel answer;
            foreach (var question in surveyResponse.Questions)
            {
                var surveyQuestionResponseId = surveyQuestionResponseByQuestionId[question.ID].ID;

                switch (question.QuestionType)
                {
                    case QuestionType.Radio:
                        CreateNewSurveyQuestionAnswerResponseRecord(
                            dbContext, surveyQuestionResponseId, question.Answers.Take(1));
                        break;
                    case QuestionType.MultiSelect:
                        CreateNewSurveyQuestionAnswerResponseRecord(
                            dbContext, surveyQuestionResponseId, question.Answers);
                        break;
                    case QuestionType.TextArea:
                    case QuestionType.Text:
                        answer = question.Answers.First();
                        answer.ID = CreateNewQuestionAnswerRecord(dbContext, answer, question.ID).ID;
                        CreateNewSurveyQuestionAnswerResponseRecord(
                            dbContext, surveyQuestionResponseId, new List<AnswerViewModel> { answer });
                        break;
                    default:
                        continue;
                }
            }

        }

        private static void CreateNewSurveyQuestionAnswerResponseRecord(
            SurveyDBContext dbContext,
            int surveyQuestionResponseId,
            IEnumerable<AnswerViewModel> answerViewModels)
        {
            foreach (var answerViewModel in answerViewModels)
            {
                var surveyQuestionAnswerResponse = new SurveyQuestionAnswerResponse
                {
                    SurveyQuestionResponseID = surveyQuestionResponseId,
                    QuestionAnswerID = answerViewModel.ID,
                };

                dbContext.SurveyQuestionAnswerResponse.Add(surveyQuestionAnswerResponse);
            }

            dbContext.SaveChanges();
        }

        private static Dictionary<int, SurveyQuestionResponse> CreateNewSurveyQuestionResponseRecord(
                SurveyDBContext dbContext,
                IEnumerable<QuestionViewModel> questionViewModels,
                int surveyResponseId)
        {
            var responseQuestionByQuestionId = new Dictionary<int, SurveyQuestionResponse>();

            foreach (var questionViewModel in questionViewModels)
            {
                var surveyQuestionResponseEntity = new SurveyQuestionResponse
                {
                    SurveyQuestionID = questionViewModel.ID,
                    SurveyResponseID = surveyResponseId,
                };

                responseQuestionByQuestionId.Add(
                    surveyQuestionResponseEntity.SurveyQuestionID,
                    surveyQuestionResponseEntity);

                dbContext.SurveyQuestionResponse.Add(surveyQuestionResponseEntity);
            }

            dbContext.SaveChanges();

            return responseQuestionByQuestionId;
        }

        private static SurveyResponse CreateNewSurveyResponseRecord(
            SurveyDBContext dbContext,
            int surveyId,
            int userId)
        {
            var surveyResponseEntity = new SurveyResponse
            {
                SurveyID = surveyId,
                UserID = userId,
            };

            dbContext.SurveyResponse.Add(surveyResponseEntity);
            dbContext.SaveChanges();

            return surveyResponseEntity;
        }

        private static SurveyQuestion CreateNewSurveyQuestionRecord(
            SurveyDBContext dbContext,
            QuestionViewModel questionViewModel,
            int surveyId)
        {
            var surveyQuestionEntity = new SurveyQuestion
            {
                ID = questionViewModel.ID,
                QuestionType = questionViewModel.QuestionType,
                Title = questionViewModel.Title,
                SurveyID = surveyId,
            };

            dbContext.SurveyQuestion.Add(surveyQuestionEntity);
            dbContext.SaveChanges();

            return surveyQuestionEntity;
        }

        private static QuestionAnswer CreateNewQuestionAnswerRecord(
            SurveyDBContext dbContext,
            AnswerViewModel answerViewModel,
            int questionId,
            bool save = true)
        {
            var questionAnswerEntity = new QuestionAnswer
            {
                ID = answerViewModel.ID,
                Text = answerViewModel.Text,
                SurveyQuestionID = questionId,
            };

            if (save)
            {
                dbContext.QuestionAnswer.Add(questionAnswerEntity);
                dbContext.SaveChanges();
            }

            return questionAnswerEntity;
        }

        private static User CreateNewUserRecord(
            SurveyDBContext dbContext,
            UserViewModel userViewModel)
        {
            var user = new User
            {
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                Email = userViewModel.Email,
            };

            dbContext.User.Add(user);
            dbContext.SaveChanges();

            return user;
        }
    }
}
