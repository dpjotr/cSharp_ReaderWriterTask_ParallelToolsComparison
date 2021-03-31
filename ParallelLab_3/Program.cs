using System;
using System.Collections.Generic;
using System.Threading;

namespace ParallelLab_3
{
    
    class Program
    {
        
        static void Main()
        {
            

            int amountOfWriters = 15;
            int amountOfReaders = 5;
            int amountOfMessages = 10;

            Console.WriteLine("_______________________Task_1_(NO_LOCKS)_________________________");
            {
                {
                    Globals.db = new Database();

                    List<DBUser> clients = new List<DBUser>();

                    for (int i = 0; i < amountOfReaders; i++) clients.Add(new Reader(i));
                    for (int i = 0; i < amountOfWriters; i++) clients.Add(new Writer(amountOfMessages, i));

                    Globals.ThreadManager(clients);
                    Globals.PerformanceCheck(clients);
                }
            }

          Console.WriteLine("_______________________Task_2_(INTERLOCK)_____________________");

            {
                Globals.db = new Database();
                Globals.db.writerUsingResource = 0;
                Globals.db.readerUsingResource = 1;


                List<DBUser> Clients = new List<DBUser>();

                for (int i = 0; i < amountOfReaders; i++) Clients.Add(new InterLockedReader(i));
                for (int i = 0; i < amountOfWriters; i++) Clients.Add(new InterLockedWriter(amountOfMessages, i));

                Globals.ThreadManager(Clients);
                Globals.PerformanceCheck(Clients);
            }

            Console.WriteLine("_______________________Task_2_(LOCK_SECTION)________________________");

            {
                Globals.db = new Database();

                List<DBUser> clients = new List<DBUser>();

                for (int i = 0; i < amountOfReaders; i++) clients.Add(new LockReader(i));
                for (int i = 0; i < amountOfWriters; i++) clients.Add(new LockWriter(amountOfMessages, i));

                Globals.ThreadManager(clients);
                Globals.PerformanceCheck(clients);
            }

            Console.WriteLine("_______________________Task_2_(AutoResetEvent)________________________");

            {
                Globals.db = new Database();

                Globals.evFull = new AutoResetEvent(false);
                Globals.evEmpty = new AutoResetEvent(true);

                List<DBUser> clients = new List<DBUser>();

                for (int i = 0; i < amountOfReaders; i++) clients.Add(new AREReader(i));
                for (int i = 0; i < amountOfWriters; i++) clients.Add(new AREWriter(amountOfMessages, i));

                Globals.ThreadManager(clients);
                Globals.PerformanceCheck(clients);
            }





            Console.WriteLine("_______________________Task_2_(MUTEX)________________________");

            {
                Globals.db = new Database();
                Globals.mut = new Mutex();
                List<DBUser> clients = new List<DBUser>();

                for (int i = 0; i < amountOfReaders; i++) clients.Add(new MutexReader(i));
                for (int i = 0; i < amountOfWriters; i++) clients.Add(new MutexWriter(amountOfMessages, i));

                Globals.ThreadManager(clients);
                Globals.PerformanceCheck(clients);
            }

            Console.WriteLine("_______________________Task_2_(SEMAPHORE)________________________");

            {
                Globals.db = new Database();

                List<DBUser> clients = new List<DBUser>();
                Globals.sem = new Semaphore(1,1);

                for (int i = 0; i < amountOfReaders; i++) clients.Add(new SemReader(i));
                for (int i = 0; i < amountOfWriters; i++) clients.Add(new SemWriter(amountOfMessages, i));

                Globals.ThreadManager(clients);
                Globals.PerformanceCheck(clients);
            }

            Console.WriteLine("_______________________Task_2_(SEMAPHORE_SLIM)________________________");

            {
                Globals.db = new Database();

                List<DBUser> clients = new List<DBUser>();
                Globals.semS = new SemaphoreSlim(1, 1);

                for (int i = 0; i < amountOfReaders; i++) clients.Add(new SemReader(i));
                for (int i = 0; i < amountOfWriters; i++) clients.Add(new SemWriter(amountOfMessages, i));

                Globals.ThreadManager(clients);
                Globals.PerformanceCheck(clients);
            }


            Console.WriteLine("_______________________Task_3_(Efficiency_test)________________________");

            Console.WindowWidth = 180;
            Console.WriteLine("Influence of increasing of readers______________________________________:");
            Console.WriteLine(
 $"Readers Writers  Messages           LOCK_SECTION            AutoResetEvent             MUTEX                SEMAPHORE          SEMAPHORE_SLIM         INTERLOCK");
            
            for (int i = 5; i < 40; i+=5) Globals.TimeTest(i, 5, 5);
            Console.WriteLine("Influence of increasing of writers_____________________________________:");
            Console.WriteLine(
$"Readers Writers  Messages           LOCK_SECTION            AutoResetEvent             MUTEX                SEMAPHORE          SEMAPHORE_SLIM          INTERLOCK");
            for (int i = 5; i < 50; i += 5) Globals.TimeTest(5, i, 5);
            Console.WriteLine("Influence of increasing of messages____________________________________:");
            Console.WriteLine(
$"Readers Writers  Messages           LOCK_SECTION            AutoResetEvent             MUTEX                SEMAPHORE          SEMAPHORE_SLIM          INTERLOCK");
            for (int i = 5; i < 5000; i *= 5) Globals.TimeTest(5, 5, i);


            Console.ReadKey();
        }
    }
}
