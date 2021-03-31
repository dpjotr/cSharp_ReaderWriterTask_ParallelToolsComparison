using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class InterLockedWriter : Writer
    {
        internal InterLockedWriter(int amountOfMessages, int writerId) : base(amountOfMessages, writerId) { }

        internal override void AccessDB()
        {
            int currentMessageN = 0;
            
            while (currentMessageN < MyMessages.Count)
            {
                if (0 == Interlocked.Exchange(ref Globals.db.writerUsingResource, 1) )
                {
             
                    Globals.db.buffer = MyMessages[currentMessageN++];
                    Interlocked.Exchange(ref Globals.db.readerUsingResource,0);

                }

            }

        }
    }
}
