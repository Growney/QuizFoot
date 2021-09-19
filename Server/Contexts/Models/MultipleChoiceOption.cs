using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Contexts.Models
{
    public class MultipleChoiceOption
    {
        [Key]
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public int OrdinalPosition { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
