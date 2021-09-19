using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Contexts.Models
{
    public class StringQuestion
    {
        [Key]
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public bool IsCaseSensitive { get; set; }        
        public bool IsOrdered { get; set; }
    }
}
