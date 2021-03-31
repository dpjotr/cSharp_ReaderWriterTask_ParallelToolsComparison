using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class AREReader : Reader
    {
        internal AREReader(int readerId) : base(readerId) { }

        internal override void AccessDB()
        {
            while (!Globals.db.finish)
            {

                    Globals.evFull.WaitOne();
                    if(!Globals.db.finish) MyMessages.Add(Globals.db.buffer);                    
                    Globals.evEmpty.Set();                
            }
        }
    }
}
