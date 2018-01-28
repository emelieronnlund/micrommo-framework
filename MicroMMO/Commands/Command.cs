using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMMO
{
    abstract class Command
    {
        public abstract string Name { get; protected set; }
        public abstract void Run();
    }
}
