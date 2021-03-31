using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class MutexWriter : Writer
    {
        internal MutexWriter(int amountOfMessages, int writerId) : base(amountOfMessages, writerId) { }

        internal override void AccessDB()
        {
            int currentMessageN = 0;
            while (currentMessageN < MyMessages.Count)
            {
                Globals.mut.WaitOne();
                if (Globals.db.bEmpty)
                {
                    Globals.db.buffer = MyMessages[currentMessageN++];
                    Globals.db.bEmpty = false;
                }
                Globals.mut.ReleaseMutex();
            }
        }
    }
}
