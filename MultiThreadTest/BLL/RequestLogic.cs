using MultiThreadTest.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MultiThreadTest.BLL
{
    public enum RequestResultEnum
    {
        OK = 1,
        NotOK = 2
    }

    public class RequestLogic
    {
        private object padlock = new object();
        
        public Queue<InsertPumpRequest> RequestQueue = new Queue<InsertPumpRequest>();
        public string RequestUrl { get; set; }

        public int OkResponseCount { get; set; } = 0;
        public int NotOkResponseCount { get; set; } = 0;
        public int HttpErrorCount { get; set; } = 0;


        private InsertPumpRequest GetQueueItem()
        {
            InsertPumpRequest result = null;
            lock (padlock)
            {
                if(RequestQueue.Count > 0) result = RequestQueue.Dequeue();
            }
            return result;
        }
        private RequestResultEnum DoRequest(InsertPumpRequest insertPumpRequest)
        {
            var jsonString = JsonSerializer.Serialize(insertPumpRequest);

            using var client = new HttpClient();

            var webRequest = new HttpRequestMessage(HttpMethod.Post, RequestUrl)
            {
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };

            var response = client.Send(webRequest);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RequestResultEnum.OK;
            }
            return RequestResultEnum.NotOK;
        }

        private void IncrementOkResponseCount()
        {
            lock (padlock)
            {
                OkResponseCount++;
            }
        }

        private void IncrementNotOKResponse()
        {
            lock (padlock)
            {
                NotOkResponseCount++;
            }
        }

        private void IncrementHttpError()
        {
            lock (padlock)
            {
                HttpErrorCount++;
            }
        }

        public void Work(CancellationToken ct)
        {           
            while(true)
            {
                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Task принудительно остановлен");
                    break;
                }
                var requestItem = GetQueueItem();
                if (requestItem == null) break;
                try
                {
                    var requestResult = DoRequest(requestItem);
                    if (requestResult == RequestResultEnum.OK)
                    {
                        Console.WriteLine($"Успешно отправлен запрос, requestItem = {requestItem}");
                        IncrementOkResponseCount();
                    }
                    else 
                    {
                        Console.WriteLine($"Запрос неуспешен, requestItem = {requestItem}");
                        IncrementNotOKResponse();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка Http-запроса, message = {ex.Message}");
                    IncrementHttpError();
                }
            }
        }        
    }
}
