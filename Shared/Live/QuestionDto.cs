using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Live
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public Guid QuestionStatusId { get; set; }
        public string Text { get; set; }
        public List<QuestionPartDto> Parts { get; set; }
    }
}
