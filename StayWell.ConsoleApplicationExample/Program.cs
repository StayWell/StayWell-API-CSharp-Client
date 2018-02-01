using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StayWell.ConsoleApplicationExample.Content;

namespace StayWell.ConsoleApplicationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = ConnectionExample.Connect();
            var contentExample = new RetrieveContentExample();
            var articleResponse = contentExample.GetContent(client, "diseases-and-conditions", "absence-seizures");
            Console.ReadLine();
        }
    }
}
