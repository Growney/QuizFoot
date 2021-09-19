using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Contexts.Models
{
    public class StringAnswer
    {
        [Key]
        public Guid Id { get; set; }
        public Guid StringQuestionId { get; set; }
        public int OrdinalPosition { get; set; }
        public string Answer { get; set; }
    }
}
