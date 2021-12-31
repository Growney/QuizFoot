using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class QuestionDto
    {
        public string Text { get; set; }
        public List<QuestionPartDto> Parts { get; set; } = new List<QuestionPartDto>();
    }
}