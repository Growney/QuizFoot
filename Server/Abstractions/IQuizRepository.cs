using QuizFoot.Shared.Edit;
using QuizFoot.Shared.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Abstractions
{
    public interface IQuizRepository
    {
        Task<List<QuizGeneralDetails>> GetUserQuizDetails(string ownerId);
        Task<QuizGeneralDetails> GetDetails(string code);
        Task<QuizGeneralDetails> GetDetails(Guid id);
        Task<Quiz> Get(Guid id);
        Task<Quiz> Get(string code);
        Task<string> SaveOrUpdate(Quiz quizCreator,string ownerId);
        Task<string> GetOwner(Guid quizId); 
    }
}
