using System;
using System.Threading;

class Program
{
    static object lock1 = new object();
    static object lock2 = new object();

    static void Thread1()
    {
        lock (lock1)
        {
            Console.WriteLine("блочу 1го");
            Thread.Sleep(2000);

            lock (lock2)
            {
                Console.WriteLine("блочу 2го");
                Thread.Sleep(2000);
            }
        }
        Console.WriteLine("Лок1 отработал");
    }

    static void Thread2()
    {
        lock (lock2)
        {
            Console.WriteLine("блочу 2го");
            Thread.Sleep(2000);

            lock (lock1)
            {
                Console.WriteLine("блочу 1го");
                Thread.Sleep(2000);
            }
        }
        Console.WriteLine("Лок2 отработал");
    }
    static void Main()
    {
        Thread t1 = new Thread(Thread1);
        Thread t2 = new Thread(Thread2);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine("Программа завершена");
    }
}