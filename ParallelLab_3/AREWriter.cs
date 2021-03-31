using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class AREWriter : Writer
    {
        internal AREWriter(int amountOfMessages, int writerId) : base(amountOfMessages, writerId) { }

        internal override void AccessDB()
        {
            int currentMessageN = 0;

            while (currentMessageN < MyMessages.Count)
            {

                Globals.evEmpty.WaitOne();
                        Globals.db.buffer = MyMessages[currentMessageN++];
                Globals.evFull.Set();
            }
        }
    }
}
