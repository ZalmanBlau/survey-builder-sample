using System;
using Microsoft.EntityFrameworkCore;

namespace Survey.API.Entities
{
    public class SurveyDBContext : DbContext
    {
        public DbSet<Survey> Survey { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public DbSet<SurveyResponse> SurveyResponse { get; set; }
        public DbSet<SurveyQuestionResponse> SurveyQuestionResponse { get; set; }
        public DbSet<SurveyQuestionAnswerResponse> SurveyQuestionAnswerResponse { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswer { get; set; }
        public DbSet<User> User { get; set; }

        public SurveyDBContext(DbContextOptions<SurveyDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SurveyQuestion>()
                .Property(sq => sq.QuestionType)
                .IsRequired();

            modelBuilder.Entity<SurveyQuestion>()
                .Property(sq => sq.Title)
                .IsRequired();

            modelBuilder.Entity<Survey>()
                .Property(s => s.Title)
                .IsRequired();

            modelBuilder.Entity<QuestionAnswer>()
                .Property(qa => qa.Text)
                .IsRequired();

            modelBuilder.Entity<SurveyQuestionAnswerResponse>()
                .HasOne(sqar => sqar.SurveyQuestionResponse)
                .WithMany(sqr => sqr.SurveyQuestionAnswerResponses)
                .HasForeignKey(sqar => sqar.SurveyQuestionResponseID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SurveyQuestionResponse>()
                .HasOne(sqr => sqr.SurveyResponse)
                .WithMany(sr => sr.SurveyQuestionResponses)
                .HasForeignKey(sqr => sqr.SurveyResponseID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}