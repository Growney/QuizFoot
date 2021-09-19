using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizFoot.Server.Contexts.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;

namespace QuizFoot.Server.Contexts
{
    public class QuizDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {

        public QuizDbContext(
                DbContextOptions<QuizDbContext> options,
                IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
            Database.EnsureCreated();
        }
        internal DbSet<Quiz> Quizzes { get; set; }
        internal DbSet<Round> Rounds { get; set; }
        internal DbSet<Question> Questions { get; set; }
        internal DbSet<MultipleChoiceOption> MultipleChoiceOptions { get; set; }
        internal DbSet<StringQuestion> StringQuestions { get; set; }
        internal DbSet<StringAnswer> StringAnswers { get; set; }
        internal DbSet<Lobby> Lobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().Property(x => x.PlayerId).HasDefaultValueSql("NEWID()");

            base.OnModelCreating(builder);
        }
    }
}
