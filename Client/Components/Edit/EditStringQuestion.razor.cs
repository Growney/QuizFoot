﻿using Microsoft.AspNetCore.Components;
using QuizFoot.Shared.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Client.Components.Edit
{
    public partial class EditStringQuestion : ComponentBase
    {
        [Parameter]
        public StringQuestion Root { get; set; }

    }
}
