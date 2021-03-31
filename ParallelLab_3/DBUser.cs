using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    abstract class DBUser
    {
        internal abstract void AccessDB();
        internal int id;
        internal List<string> MyMessages;
        protected DBUser(int UserId)
        { 
            id = UserId;
            MyMessages = new List<string>();
        }

        internal virtual void Print()
        {
            if (this is Reader) Console.WriteLine($"Reader {id} got {MyMessages.Count} messages:\n{string.Join("\n", MyMessages)}");
            if (this is Writer) Console.WriteLine($"Writer {id} wrote {string.Join(",", MyMessages)}");
        }
    }
}
