using MultiThreadTest.BLL;
using MultiThreadTest.Factory;
using System.Diagnostics;

var fakeRequestList = FakeFactory.CreateList(100);
var numOfTasks = 10;
int taskFailCount = 0;
var timer = new Stopwatch();

RequestLogic requestLogic = new RequestLogic();
requestLogic.RequestUrl = "https://localhost:44319/api/ChargeService";
foreach (var request in fakeRequestList) requestLogic.RequestQueue.Enqueue(request);

CancellationTokenSource cts = new CancellationTokenSource();
CancellationToken ct = cts.Token;

Task[] tasks = new Task[numOfTasks];
for (int i = 0; i < numOfTasks; i++)
{
    tasks[i] = new Task(() =>
    {
        Console.WriteLine($"Task стартовал");
        requestLogic.Work(ct);
    }, ct);
}

timer.Start();
foreach (var task in tasks) task.Start();

//Console.WriteLine("Нажмите любую клавишу для отмены");
//Console.ReadKey();
//cts.Cancel();

try
{
    Task.WaitAll(tasks);
}
catch (AggregateException ae)
{
    foreach (Exception e in ae.InnerExceptions)
    {
        taskFailCount++;
        Console.WriteLine($"Exception {e.GetType()} from {e.Source}.");
    }
}

timer.Stop();
TimeSpan timeTaken = timer.Elapsed;

Console.WriteLine($"Отправка запросов закончилась");
Console.WriteLine($"Успешных запросов: {requestLogic.OkResponseCount}");
Console.WriteLine($"Неуспешных запросов: {requestLogic.NotOkResponseCount}");
Console.WriteLine($"Ошибок запросов HTTP: {requestLogic.HttpErrorCount}");
Console.WriteLine($"Ошибок в Task: {taskFailCount}");
Console.WriteLine($"Времени заняло на отправку {fakeRequestList.Count} запросов: "
                + timeTaken.ToString(@"m\:ss\.fff"));