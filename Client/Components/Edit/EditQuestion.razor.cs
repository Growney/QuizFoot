using Microsoft.AspNetCore.Components;
using QuizFoot.Shared.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Client.Components.Edit
{
    public partial class EditQuestion : ComponentBase
    {
        [Parameter]
        public Question Root { get; set; }
    }
}
