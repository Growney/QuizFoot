using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Abstractions
{
    public interface ICodeGenerator
    {
        string Generate(int length);
        string Generate(int length, Func<string, bool> exists);
        Task<string> Generate(int length, Func<string, Task<bool>> exists);
    }
}
