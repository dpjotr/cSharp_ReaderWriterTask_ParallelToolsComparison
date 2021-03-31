using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class Reader : DBUser
    {
        internal Reader(int readerId) : base (readerId) { }
        internal override void AccessDB()
        {   
                while (!Globals.db.finish)
                {
                    if (!Globals.db.bEmpty)
                    {
                        MyMessages.Add(Globals.db.buffer);
                        Globals.db.bEmpty = true;
                    }
                }
        }
    }
}
