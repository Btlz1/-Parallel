var semaphore = new SemaphoreSlim(1, 1);
var tasks = Enumerable.Range(0, 100).Select(WriteInFile);

await Task.WhenAll(tasks);

async Task WriteInFile(int i)
{
    await semaphore.WaitAsync(); 
    try
    {
        await using StreamWriter writer = new("ouptput.txt", append: true);
        await writer.WriteLineAsync(i.ToString());
    }
    finally
    {
        semaphore.Release(); 
    }
}