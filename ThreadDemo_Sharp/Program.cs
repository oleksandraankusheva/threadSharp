using System;
using System.Collections.Generic;
using System.Threading;

namespace SequenceThreadsDemo
{
    class SequenceThread
    {
        private int id;
        private int step;
        private bool stopRequested = false;
        private Thread thread;

        private long sum = 0;
        private int count = 0;
        private int current = 0;

        public SequenceThread(int id, int step)
        {
            this.id = id;
            this.step = step;
            thread = new Thread(Run);
        }

        public void Start() => thread.Start();
        public void RequestStop() => stopRequested = true;
        public void Join() => thread.Join();

        private void Run()
        {
            while (!stopRequested)
            {
                sum += current;
                current += step;
                count++;
                Thread.Sleep(10);
            }

            Console.WriteLine($"Потік #{id} | Сума: {sum} | Доданків: {count}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Скільки потоків створити? ");
            int n = int.Parse(Console.ReadLine());

            var steps = new int[n];
            var durations = new int[n];
            var threads = new List<SequenceThread>();

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Введіть крок для потоку #{i + 1}: ");
                steps[i] = int.Parse(Console.ReadLine());

                Console.Write($"Введіть час роботи (мс) для потоку #{i + 1}: ");
                durations[i] = int.Parse(Console.ReadLine());
            }

            for (int i = 0; i < n; i++)
            {
                var worker = new SequenceThread(i + 1, steps[i]);
                threads.Add(worker);
                worker.Start();

                // Таймер-потік для кожного
                int index = i;
                new Thread(() =>
                {
                    Thread.Sleep(durations[index]);
                    threads[index].RequestStop();
                }).Start();
            }

            // Очікуємо завершення всіх потоків
            foreach (var t in threads)
                t.Join();

            Console.WriteLine("Усі потоки завершено через незалежні таймери.");
        }
    }
}
