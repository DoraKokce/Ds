using System.Text;
using Ds.Frontend;
using Ds.Frontend.Ast;
using Ds.Runtime;

namespace Ds {
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1) {
                Console.WriteLine("Usage: ds <filename>.ds");
                Environment.Exit(1);
            }

            using FileStream fs = File.OpenRead(args[0]);
            string src = "";
            byte[] b = new byte[1024];
            UTF8Encoding temp = new(true);
            int readLen;
            while ((readLen = fs.Read(b, 0, b.Length)) > 0)
            {
                src += temp.GetString(b, 0, readLen);
            }
            Parser parser = new();
            ProgramNode ast = parser.ProduceAst(src);
            Interpreter interpreter = new();
            interpreter.Eval(ast,new DSEnvironment().CreateGlobalEnv());
        }
    }
}