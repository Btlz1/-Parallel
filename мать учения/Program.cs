Console.WriteLine("Press enter to stop...");

await WorkerFactory.CreateWorkerAndStart();

Console.WriteLine("Program ended.");

static class WorkerFactory
{
    public static Task CreateWorkerAndStart()
    {
        CancellationTokenSource cts = new(1000);
        var worker = Task.Run(async () =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                Console.WriteLine("Do something...");
                await Task.Delay(1000, cts.Token);
            }
            Console.WriteLine("Worker done.");
        });

        return worker;
    }
}