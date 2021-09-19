using QuizFoot.Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace QuizFoot.Server.Implementation
{
    public class CodeGenerator : ICodeGenerator
    {
        const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private Random random = new Random();
        public string Generate(int length)
        {
            return new string(Enumerable.Repeat(valid, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string Generate(int length,Func<string,bool> exists)
        {
            string attemptedCode = null;
            do
            {
                attemptedCode = Generate(length);
            }
            while (exists(attemptedCode));

            return attemptedCode;
        }
        public async Task<string> Generate(int length, Func<string, Task<bool>> exists)
        {
            string attemptedCode = null;
            do
            {
                attemptedCode = Generate(length);
            }
            while (await exists(attemptedCode));

            return attemptedCode;
        }

    }
}
