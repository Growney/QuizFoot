using Microsoft.EntityFrameworkCore;
using QuizFoot.Server.Abstractions;
using QuizFoot.Server.Contexts;
using QuizFoot.Shared;
using QuizFoot.Shared.Edit;
using QuizFoot.Shared.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Implementation
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizDbContext _context;
        private readonly ICodeGenerator _codeGenerator;
        public QuizRepository(QuizDbContext context, ICodeGenerator generator)
        {
            _context = context;
            _codeGenerator = generator;
        }

        public Task<string> GetOwner(Guid quizId)
        {
            if (quizId == Guid.Empty)
            {
                return Task.FromResult<string>(null);
            }
            return (from row in _context.Quizzes
                    where row.Id == quizId
                    select row.OwnerId).SingleOrDefaultAsync();
        }

        public Task<List<QuizGeneralDetails>> GetUserQuizDetails(string ownerId) =>
            (from quiz in _context.Quizzes
             where quiz.OwnerId == ownerId
             select new QuizGeneralDetails()
             {
                 Name = quiz.Name,
                 Code = quiz.Code,
                 Questions = quiz.Questions,
                 Rounds = quiz.Rounds
             }).ToListAsync();

        public Task<QuizGeneralDetails> GetDetails(string code) =>
            (from quiz in _context.Quizzes
            where quiz.Code == code
            select new QuizGeneralDetails()
            {
                Id = quiz.Id,
                Name = quiz.Name,
                Code = quiz.Code,
                Questions = quiz.Questions,
                Rounds = quiz.Rounds
            }).FirstOrDefaultAsync();
        public Task<QuizGeneralDetails> GetDetails(Guid id) =>
            (from quiz in _context.Quizzes
             where quiz.Id == id
             select new QuizGeneralDetails()
             {
                 Id = quiz.Id,
                 Name = quiz.Name,
                 Code = quiz.Code,
                 Questions = quiz.Questions,
                 Rounds = quiz.Rounds
             }).FirstOrDefaultAsync();
        public async Task<Quiz> Get(Guid id)
        {
            var root = await (from quiz in _context.Quizzes
                              where quiz.Id == id
                              select new Quiz() { Id = quiz.Id, Code = quiz.Code, Name = quiz.Name }).FirstOrDefaultAsync();
            if (root != null)
            {
                root.Rounds = await GetRounds(id);
            }

            return root;

        }

        public async Task<Quiz> Get(string code)
        {
            var root = await (from quiz in _context.Quizzes
                              where quiz.Code == code
                              select new Quiz() { Id = quiz.Id, Code = quiz.Code, Name = quiz.Name }).FirstOrDefaultAsync();
            if (root != null)
            {
                root.Rounds = await GetRounds(root.Id);
            }

            return root;
        }

        private async Task<List<Round>> GetRounds(Guid quizId)
        {
            var rounds = await (from round in _context.Rounds
                                where round.QuizId == quizId
                                orderby round.OrdinalPosition
                                select new Round() { Id = round.Id, Name = round.Name }).ToListAsync();

            foreach (var round in rounds)
            {
                var questions = await GetQuestions(round.Id);
                round.Questions = questions;
            }


            return rounds;
        }

        private async Task<List<Question>> GetQuestions(Guid roundId)
        {
            var questions = await (from question in _context.Questions
                                   where question.RoundId == roundId
                                   orderby question.OrdinalPosition
                                   select new Question()
                                   {
                                       Id = question.Id,
                                       Text = question.Text,
                                       Type = (QuestionType)question.Type,
                                   }).ToListAsync();


            foreach (var question in questions)
            {
                switch (question.Type)
                {
                    case QuestionType.MultipleChoice:
                        {
                            var options = await (from option in _context.MultipleChoiceOptions
                                                 where option.QuestionId == question.Id
                                                 orderby option.OrdinalPosition
                                                 select new MultipleChoiceOption()
                                                 {
                                                     Id = option.Id,
                                                     Answer = option.Answer,
                                                     IsCorrect = option.IsCorrect
                                                 }
                                                 ).ToListAsync();


                            question.QuestionObject = new MultipleChoiceQuestion()
                            {
                                Options = options
                            };
                        }
                        break;
                    case QuestionType.String:
                        {
                            var stringQuestion = await (from st in _context.StringQuestions
                                                        where st.QuestionId == question.Id
                                                        select new StringQuestion()
                                                        {
                                                            Id = st.Id,
                                                            IsCaseSensitive = st.IsCaseSensitive,
                                                            IsOrdered = st.IsOrdered
                                                        }).FirstOrDefaultAsync();
                            if (stringQuestion != null)
                            {
                                var answers = await (from sa in _context.StringAnswers
                                                     where sa.StringQuestionId == stringQuestion.Id
                                                     select new StringAnswer()
                                                     {
                                                         Id = sa.Id,
                                                         Answer = sa.Answer
                                                     }).ToListAsync();
                                stringQuestion.Answers = answers;

                                question.QuestionObject = stringQuestion;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return questions;
        }
        private int GetQuestionCount(Quiz quiz)
        {
            int questionCount = 0;
            if (quiz != null && quiz.Rounds != null)
            {
                foreach (var round in quiz.Rounds)
                {
                    questionCount += round.Questions?.Count() ?? 0;
                }
            }
            return questionCount;
        }
        public async Task<string> SaveOrUpdate(Quiz quiz, string ownerId)
        {
            var existingQuiz = await (from storedQuiz in _context.Quizzes where storedQuiz.Id == quiz.Id select storedQuiz).FirstOrDefaultAsync();
            if (existingQuiz != null)
            {
                existingQuiz.Name = quiz.Name;
            }
            else
            {
                if (quiz.Id == Guid.Empty)
                {
                    existingQuiz = new Contexts.Models.Quiz() { Id = Guid.NewGuid(), Name = quiz.Name, OwnerId = ownerId };
                    existingQuiz.Code = await _codeGenerator.Generate(15, x=> _context.Quizzes.AnyAsync(y =>y.Code == x));

                    _context.Quizzes.Add(existingQuiz);
                }
            }

            existingQuiz.Rounds = quiz.Rounds?.Count ?? 0;
            existingQuiz.Questions = GetQuestionCount(quiz);


            if (existingQuiz != null)
            {
                await UpdateRounds(existingQuiz.Id, quiz.Rounds);
            }


            await _context.SaveChangesAsync();

            return existingQuiz.Code;
        }

        public async Task UpdateRounds(Guid quizId, List<Round> rounds)
        {
            var existingRounds = await (from storedRounds in _context.Rounds where storedRounds.QuizId == quizId select storedRounds).ToListAsync();
            foreach (var existingRound in existingRounds)
            {
                if (!rounds.Any(x => x.Id == existingRound.Id))
                {
                    _context.Rounds.Remove(existingRound);
                }
            }

            for (int i = 0; i < rounds.Count; i++)
            {
                Round modelRound = (Round)rounds[i];
                Contexts.Models.Round existingRound = null;
                if (modelRound.Id != Guid.Empty)
                {
                    existingRound = await _context.Rounds.SingleOrDefaultAsync(x => x.QuizId == quizId && x.Id == modelRound.Id);
                }

                if (existingRound == null)
                {
                    existingRound = new Contexts.Models.Round()
                    {
                        QuizId = quizId,
                        Id = Guid.NewGuid(),
                        Name = modelRound.Name,
                        OrdinalPosition = i
                    };

                    await _context.Rounds.AddAsync(existingRound);
                }
                else
                {
                    existingRound.Name = modelRound.Name;
                    existingRound.OrdinalPosition = i;
                }

                await UpdateQuestions(existingRound.Id, modelRound.Questions);
            }
        }
        public async Task UpdateQuestions(Guid roundId, List<Question> questions)
        {
            var existingQuestions = await _context.Questions.Where(x => x.RoundId == roundId).ToListAsync();

            foreach (var existingQuestion in existingQuestions)
            {
                if (!questions.Any(x => x.Id == existingQuestion.Id))
                {
                    await RemoveQuestionTypes(existingQuestion.Id, existingQuestion.Type);
                    _context.Questions.Remove(existingQuestion);
                }
            }

            for (int i = 0; i < questions.Count; i++)
            {
                Question modelQuestion = questions[i];
                Contexts.Models.Question existingQuestion = null;
                if (modelQuestion.Id != Guid.Empty)
                {
                    existingQuestion = await _context.Questions.SingleOrDefaultAsync(x => x.Id == modelQuestion.Id);
                }

                if (existingQuestion == null)
                {
                    existingQuestion = new Contexts.Models.Question()
                    {
                        Id = Guid.NewGuid(),
                        RoundId = roundId,
                        Text = modelQuestion.Text,
                        Type = (int)modelQuestion.Type,
                        OrdinalPosition = i,
                    };
                    await _context.Questions.AddAsync(existingQuestion);
                }
                else
                {
                    existingQuestion.Text = modelQuestion.Text;
                    existingQuestion.OrdinalPosition = i;
                    if (existingQuestion.Type != (int)modelQuestion.Type)
                    {
                        await RemoveQuestionTypes(existingQuestion.Id, existingQuestion.Type);
                    }
                    existingQuestion.Type = (int)modelQuestion.Type;
                    existingQuestion.RoundId = roundId;
                }

                switch ((QuestionType)modelQuestion.Type)
                {
                    case QuestionType.MultipleChoice:
                        if (modelQuestion.QuestionObject is MultipleChoiceQuestion multiChoiceQuestion)
                        {
                            await UpdateMultiChoiceOptions(existingQuestion.Id, multiChoiceQuestion.Options);
                        }
                        break;
                    case QuestionType.String:
                        if (modelQuestion.QuestionObject is StringQuestion stringQuestion)
                        {
                            await UpdateStringQuestions(existingQuestion.Id, stringQuestion);
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        public async Task RemoveQuestionTypes(Guid questionId, int type)
        {
            switch ((QuestionType)type)
            {
                case QuestionType.MultipleChoice:
                    _context.MultipleChoiceOptions.RemoveRange(_context.MultipleChoiceOptions.Where(x => x.QuestionId == questionId));
                    break;
                case QuestionType.String:
                    var toRemoveStringQuestions = await _context.StringQuestions.Where(x => x.QuestionId == questionId).ToListAsync();
                    _context.StringAnswers.RemoveRange(_context.StringAnswers.Where(x => toRemoveStringQuestions.Any(y => x.StringQuestionId == y.Id)));
                    _context.StringQuestions.RemoveRange(toRemoveStringQuestions);
                    break;
                default:
                    break;
            }
        }
        public async Task UpdateStringQuestions(Guid questionId, StringQuestion question)
        {
            var stringQuestion = await _context.StringQuestions.SingleOrDefaultAsync(x => x.QuestionId == questionId);

            if (stringQuestion == null)
            {
                stringQuestion = new Contexts.Models.StringQuestion()
                {
                    IsCaseSensitive = question.IsCaseSensitive,
                    IsOrdered = question.IsOrdered,
                    Id = Guid.NewGuid(),
                    QuestionId = questionId
                };

                await _context.StringQuestions.AddAsync(stringQuestion);
            }
            else
            {
                stringQuestion.QuestionId = questionId;
                stringQuestion.IsCaseSensitive = question.IsCaseSensitive;
                stringQuestion.IsOrdered = question.IsOrdered;
            }

            var existingAnswers = await _context.StringAnswers.Where(x => stringQuestion.Id == x.StringQuestionId).ToListAsync();

            foreach (var existingOption in existingAnswers)
            {
                if (!question.Answers.Any(x => x.Id == existingOption.Id))
                {
                    _context.StringAnswers.Remove(existingOption);
                }
            }

            for (int i = 0; i < question.Answers.Count; i++)
            {
                var modelAnswer = question.Answers[i];
                Contexts.Models.StringAnswer existingAnswer = null;
                if (modelAnswer.Id != Guid.Empty)
                {
                    existingAnswer = await _context.StringAnswers.SingleOrDefaultAsync(x => x.Id == modelAnswer.Id);
                }

                if (existingAnswer == null)
                {
                    existingAnswer = new Contexts.Models.StringAnswer();
                    existingAnswer.Id = Guid.NewGuid();
                    existingAnswer.StringQuestionId = stringQuestion.Id;
                    existingAnswer.Answer = modelAnswer.Answer;
                    existingAnswer.OrdinalPosition = i;

                    await _context.StringAnswers.AddAsync(existingAnswer);
                }
                else
                {
                    existingAnswer.Answer = modelAnswer.Answer;
                    existingAnswer.StringQuestionId = stringQuestion.Id;
                    existingAnswer.OrdinalPosition = i;
                }
            }
        }
        public async Task UpdateMultiChoiceOptions(Guid questionId, List<MultipleChoiceOption> options)
        {
            var existingOptions = await _context.MultipleChoiceOptions.Where(x => questionId == x.QuestionId).ToListAsync();

            foreach (var existingOption in existingOptions)
            {
                if (!options.Any(x => x.Id == existingOption.Id))
                {
                    _context.MultipleChoiceOptions.Remove(existingOption);
                }
            }

            for (int i = 0; i < options.Count; i++)
            {
                MultipleChoiceOption modelOption = options[i];
                Contexts.Models.MultipleChoiceOption existingOption = null;
                if (modelOption.Id != Guid.Empty)
                {
                    existingOption = await _context.MultipleChoiceOptions.SingleOrDefaultAsync(x => x.Id == modelOption.Id);
                }

                if (existingOption == null)
                {
                    existingOption = new Contexts.Models.MultipleChoiceOption();
                    existingOption.IsCorrect = modelOption.IsCorrect;
                    existingOption.Answer = modelOption.Answer;
                    existingOption.QuestionId = questionId;
                    existingOption.OrdinalPosition = i;
                    existingOption.Id = Guid.NewGuid();

                    await _context.MultipleChoiceOptions.AddAsync(existingOption);
                }
                else
                {
                    existingOption.IsCorrect = modelOption.IsCorrect;
                    existingOption.OrdinalPosition = i;
                    existingOption.Answer = modelOption.Answer;
                    existingOption.QuestionId = questionId;
                }
            }
        }


    }
}
