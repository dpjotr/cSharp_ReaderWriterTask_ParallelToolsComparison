using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ParallelLab_3
{
    internal static class Globals
    {
        internal static Database db = new Database();
        internal static EventWaitHandle evFull;
        internal static EventWaitHandle evEmpty;
        
        internal static Mutex  mut;
        internal static Semaphore sem;
        internal static SemaphoreSlim semS;


        internal static void ThreadManager(List<DBUser> Clients)
        {
            Thread[] clientThreads = new Thread[Clients.Count];

            for (int j = 0; j < Clients.Count; j++)
            {
                clientThreads[j] = new Thread(Clients[j].AccessDB);
                if (Clients[j] is Reader) 
                    clientThreads[j].Name = "Reader" + j;
                if (Clients[j] is Writer) 
                    clientThreads[j].Name = "Writer" + j;
                clientThreads[j].Start();

            }

            

            for (int i = 0; i < Clients.Count; i++)
                if (Clients[i] is Writer wr) clientThreads[i].Join();
                                  
            Globals.db.finish = !Globals.db.finish;

            for (int i = 0; i < Clients.Count; i++)
                if (Clients[i] is Reader)
                {
                    if (Clients.First() is AREReader || Clients.First() is AREWriter )   
                        evFull.Set(); 

                    clientThreads[i].Join();
                }      

        }
    
        internal static void PerformanceCheck(List<DBUser> Clients)
        {

            HashSet<string> UniqueWtittenMessages = new HashSet<string>();
            HashSet<string> UniqueReadMessages = new HashSet<string>();

            foreach (var x in Clients)
            {
                if (x is Writer)
                    foreach (var y in x.MyMessages) UniqueWtittenMessages.Add(y);
                if (x is Reader)
                    foreach (var y in x.MyMessages) UniqueReadMessages.Add(y);
            }
            if (UniqueWtittenMessages.SetEquals(UniqueReadMessages))
                Console.WriteLine("\nAll written messages were read");
            else
            {
                Console.WriteLine($@"
                                 {UniqueWtittenMessages.Count} unique messages were written
                                 {UniqueReadMessages.Count} unique messages were read");
                UniqueWtittenMessages.ExceptWith(UniqueReadMessages);
                string lost = string.Join("\n", UniqueWtittenMessages);
                Console.WriteLine($@" {UniqueWtittenMessages.Count} unique messages were not read:
{lost} "); 
            }
            int counter = 0;
            Console.Write("\nMessages which were read more than once:\n");
            foreach (var x in Clients.SelectMany(x => x.MyMessages).GroupBy(y => y))
                if (x.Count() > 2)
                {
                    Console.WriteLine($"{x.Key} was read {x.Count() - 1} times");
                    counter++;
                }
            if(counter==0) Console.WriteLine(" no messages");

        }


       public static void TimeTest(int r, int wr, int m)
        {
            int amountOfReaders = r; int amountOfWriters = wr; int amountOfMessages = m;
            DateTime dt1, dt2;

            List<DateTime> results = new List<DateTime>();

            Console.Write($"{r,000}\t{wr,000}\t {m,000}\t\t");

            {
                Globals.db = new Database();

                List<DBUser> Clients = new List<DBUser>();

                for (int i = 0; i < amountOfReaders; i++) Clients.Add(new LockReader(i));
                for (int i = 0; i < amountOfWriters; i++) Clients.Add(new LockWriter(amountOfMessages, i));
                dt1 = DateTime.Now;
                Globals.ThreadManager(Clients);
                dt2 = DateTime.Now;
               
                Console.Write(String.Format("{0,12}", (dt2 - dt1).TotalMilliseconds) + "\t\t");
            }

   

            {
                Globals.db = new Database();

                Globals.evFull = new AutoResetEvent(false);
                Globals.evEmpty = new AutoResetEvent(true);

                List<DBUser> Clients = new List<DBUser>();

                for (int i = 0; i < amountOfReaders; i++) Clients.Add(new AREReader(i));
                for (int i = 0; i < amountOfWriters; i++) Clients.Add(new AREWriter(amountOfMessages, i));

                dt1 = DateTime.Now;
                Globals.ThreadManager(Clients);
                dt2 = DateTime.Now;

                
                Console.Write(String.Format("{0,12}", (dt2 - dt1).TotalMilliseconds) + "\t\t");

            }





         

            {
                Globals.db = new Database();
                Globals.mut = new Mutex();
                List<DBUser> Clients = new List<DBUser>();

                for (int i = 0; i < amountOfReaders; i++) Clients.Add(new MutexReader(i));
                for (int i = 0; i < amountOfWriters; i++) Clients.Add(new MutexWriter(amountOfMessages, i));

                dt1 = DateTime.Now;
                Globals.ThreadManager(Clients);
                dt2 = DateTime.Now;

              
                Console.Write(String.Format("{0,12}", (dt2 - dt1).TotalMilliseconds) + "\t\t");

            }



            {
                Globals.db = new Database();

                List<DBUser> Clients = new List<DBUser>();
                Globals.sem = new Semaphore(1, 1);

                for (int i = 0; i < amountOfReaders; i++) Clients.Add(new SemReader(i));
                for (int i = 0; i < amountOfWriters; i++) Clients.Add(new SemWriter(amountOfMessages, i));

                dt1 = DateTime.Now;
                Globals.ThreadManager(Clients);
                dt2 = DateTime.Now;

             
                Console.Write(String.Format("{0,12}", (dt2 - dt1).TotalMilliseconds) + "\t\t");

            }



            {
                Globals.db = new Database();

                List<DBUser> Clients = new List<DBUser>();
                Globals.semS = new SemaphoreSlim(1, 1);

                for (int i = 0; i < amountOfReaders; i++) Clients.Add(new SemReader(i));
                for (int i = 0; i < amountOfWriters; i++) Clients.Add(new SemWriter(amountOfMessages, i));

                dt1 = DateTime.Now;
                Globals.ThreadManager(Clients);
                dt2 = DateTime.Now;

               
                Console.Write(String.Format("{0,12}", (dt2 - dt1).TotalMilliseconds) + "\t\t");

            }

            {
                Globals.db = new Database();

                List<DBUser> Clients = new List<DBUser>();
                Globals.semS = new SemaphoreSlim(1, 1);

                for (int i = 0; i < amountOfReaders; i++) Clients.Add(new InterLockedReader(i));
                for (int i = 0; i < amountOfWriters; i++) Clients.Add(new InterLockedWriter(amountOfMessages, i));

                dt1 = DateTime.Now;
                Globals.ThreadManager(Clients);
                dt2 = DateTime.Now;


                Console.Write(String.Format("{0,12}", (dt2 - dt1).TotalMilliseconds) + "\n");

            }

        }
    }
}
