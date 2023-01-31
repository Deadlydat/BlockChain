using IronPython.Hosting;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using static IronPython.Modules._ast;


namespace BCandSC_CSharp
{
    public class PythonScriptGanache
    {
        public static void RunPython()
        {

            var engine = Python.CreateEngine();
            //reading code from file
            var source = engine.CreateScriptSourceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "main.py"));
            var scope = engine.CreateScope();
            //executing script in scope
            source.Execute(scope);
            var function = scope.GetVariable("test");
            //initializing class
            var testInstance = engine.Operations.CreateInstance(function);
            Console.WriteLine("From Iron Python");

            Console.WriteLine("print on console:" + testInstance.print_hi("peter"));


            Console.WriteLine("");
            Console.WriteLine("calculate:"+ testInstance.calculate(2, 6));

        }
        //instance of python engine



    }
}
