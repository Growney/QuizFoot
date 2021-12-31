using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Live
{
    public class LobbyInfoDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string HostName { get; set; }
        public QuizDetailsDto Quiz { get; set; } 
    }
}
