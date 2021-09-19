using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Live
{
    public class LobbyInfo
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string HostName { get; set; }
        public QuizGeneralDetails Quiz { get; set; } 
    }
}
