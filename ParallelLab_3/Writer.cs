using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class Writer : DBUser
    {
        internal Writer(int amountOfMessages, int writerId) : base(writerId)
        {             
            for (int i=0; i<amountOfMessages; i++)
            {               
                MyMessages.Add(" Writer_"+base.id+": message_"+i);
            }
        }

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
