using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class LockWriter : Writer
    {
        internal LockWriter(int amountOfMessages, int writerId) : base(amountOfMessages, writerId) { }

        internal override void AccessDB()
        {
            int currentMessageN = 0;
            while (currentMessageN < MyMessages.Count)
            {
                lock ("write")
                {
                    if (Globals.db.bEmpty)
                    {
                        Globals.db.buffer = MyMessages[currentMessageN++];
                        Globals.db.bEmpty = false;
                    }
                }
            }
        }
    }
}
