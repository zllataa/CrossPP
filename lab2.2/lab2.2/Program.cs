using System;
using System.Threading;

class Program
{
    static Random random = new Random();
    static int totalProcesses = 1000; // Загальна кількість процесів для генерації
    static int destroyedOnFirstCPU = 0; // Лічильник знищених процесів на першому процесорі
    static int destroyedOnSecondCPU = 0; // Лічильник знищених процесів на другому процесорі

    static void Main()
    {
        // Створення об'єктів процесорів
        CPU firstCPU = new CPU("Перший процесор");
        CPU secondCPU = new CPU("Другий процесор");

        // Створення потоків для обробки процесів на кожному процесорі
        Thread firstCPUThread = new Thread(firstCPU.ProcessingLoop);
        Thread secondCPUThread = new Thread(secondCPU.ProcessingLoop);

        // Запуск потоків процесорів
        firstCPUThread.Start();
        secondCPUThread.Start();

        // Генерація процесів та намагань обробки
        for (int i = 0; i < totalProcesses; i++)
        {
            CPUProcess process = new CPUProcess();
            process.Generate();

            // Намагання обробити процес на першому та другому процесорі
            bool processedOnFirstCPU = firstCPU.TryProcess(process);
            bool processedOnSecondCPU = secondCPU.TryProcess(process);

            // Логіка для визначення знищення процесу
            if (!processedOnFirstCPU && !processedOnSecondCPU)
            {
                Console.WriteLine($"Процес {process.Id} був знищений.");
                if (random.Next(2) == 0)
                {
                    destroyedOnFirstCPU++;
                }
                else
                {
                    destroyedOnSecondCPU++;
                }
            }

            // Очікування перед генерацією наступного процесу
            Thread.Sleep(random.Next(100, 500));
        }

        // Завершення роботи потоків процесорів
        firstCPU.Stop();
        secondCPU.Stop();

        // Очікування завершення потоків
        firstCPUThread.Join();
        secondCPUThread.Join();

        // Виведення статистики
        Console.WriteLine($"Відсоток знищених процесів на першому процесорі: {((double)destroyedOnFirstCPU / totalProcesses) * 100:F2}%");
        Console.WriteLine($"Відсоток знищених процесів на другому процесорі: {((double)destroyedOnSecondCPU / totalProcesses) * 100:F2}%");
    }
}

class CPUQueue
{
    private Queue<CPUProcess> processes = new Queue<CPUProcess>();
    private object lockObject = new object();

    public void Enqueue(CPUProcess process)
    {
        lock (lockObject)
        {
            processes.Enqueue(process);
        }
    }

    public CPUProcess Dequeue()
    {
        lock (lockObject)
        {
            if (processes.Count > 0)
            {
                return processes.Dequeue();
            }
            return null;
        }
    }
}

class CPUProcess
{
    public int Id { get; private set; }

    public void Generate()
    {
        Id = Guid.NewGuid().GetHashCode();
    }
}

class CPU
{
    private string name;
    private Thread processingThread;
    private bool isRunning = true;
    private CPUQueue cpuQueue = new CPUQueue();

    public CPU(string name)
    {
        this.name = name;
        processingThread = new Thread(ProcessingLoop);
    }

    public void ProcessingLoop()
    {
        while (isRunning)
        {
            CPUProcess process = cpuQueue.Dequeue();
            if (process != null)
            {
                Console.WriteLine($"{name}: Обробка процесу {process.Id}");
                // Виконайте обробку процесу тут
                Thread.Sleep(new Random().Next(100, 500));
            }
        }
    }

    public bool TryProcess(CPUProcess process)
    {
        if (new Random().Next(2) == 0)
        {
            Console.WriteLine($"{name}: Процес {process.Id} прийнятий для обробки.");
            cpuQueue.Enqueue(process);
            return true;
        }
        return false;
    }

    public void Stop()
    {
        isRunning = false;
    }
}
