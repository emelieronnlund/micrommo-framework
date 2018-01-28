using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMMO.Commands
{
    class HelloWorldCommand : Command
    {
        public override string Name { get => _name; protected set { _name = value; } }

        string _name;
        public override void Run()
        {
            Console.WriteLine("Hello, world!");
        }
    }
}
