using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Contexts.Models
{
    internal class Round
    {
        [Key]
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public int OrdinalPosition { get; set; }
        public string Name { get; set; }
    }
}
