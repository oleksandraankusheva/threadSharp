using System;
using System.Threading;

namespace SequenceThreadsDemo
{
    class SequenceThread
    {
        private int id;
        private int step;
        private int durationMs;
        private Thread thread;

        public SequenceThread(int id, int step, int durationMs)
        {
            this.id = id;
            this.step = step;
            this.durationMs = durationMs;
            thread = new Thread(Run);
        }

        public void Start()
        {
            thread.Start();
        }

        private void Run()
        {
            long sum = 0;
            int count = 0;
            int current = 0;
            var startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalMilliseconds < durationMs)
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
            int[] steps = { 1, 2, 3, };
            int[] durations = { 5000, 7000, 10000, };

            for (int i = 0; i < steps.Length; i++)
            {
                var worker = new SequenceThread(i + 1, steps[i], durations[i]);
                worker.Start();
            }

            Console.WriteLine("Очікування завершення потоків...");
            Thread.Sleep(11000);

            Console.WriteLine("Усі потоки завершили роботу.");
        }
    }
}
