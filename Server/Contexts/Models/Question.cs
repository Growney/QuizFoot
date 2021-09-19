using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Contexts.Models
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoundId { get; set; }
        public int OrdinalPosition { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
    }
}
