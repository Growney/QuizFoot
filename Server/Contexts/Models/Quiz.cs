using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Contexts.Models
{
    internal class Quiz
    {
        [Key]
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public int Rounds { get; set; }
        public int Questions { get; set; }
    }
}
