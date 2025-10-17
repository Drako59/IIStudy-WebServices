
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Testing
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestBook();
            
            GetCurrenyExchange();
            Console.ReadLine();
        }

        static async Task GetCurrenyExchange(string from = "USD", string to ="ILS" , int amount = 100)
            
        {
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://currency-conversion-and-exchange-rates.p.rapidapi.com/convert?from={from}&to={to}&amount={amount}"),
                Headers =
    {
        { "x-rapidapi-key", "4d056610c5msh0d91eab1e9347c6p1269c2jsnb2ec8c0be52f" },
        { "x-rapidapi-host", "currency-conversion-and-exchange-rates.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                await Console.Out.WriteLineAsync("hi");
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                CurrecnyExchange cur = JsonSerializer.Deserialize<CurrecnyExchange>(body);
                Console.WriteLine($" {cur.query.amount} {cur.query.from} == {cur.result} {cur.query.to}");
            }

        }
        static void TestBook()
        {
            Book book = new Book() { Author_name = "Hello"
                , Book_ID = "1"
                , Book_name = "Max"
                , Book_price = 90
                , In_stock = true
                ,  Pdf_url_book ="NOTHINh.png" 
                , SubjectID = "1"  };
            if (book.HasErrors)
            {
                foreach(KeyValuePair<string, List<string>> keyValuePair in book.AllErrors())
                {
                    Console.WriteLine(keyValuePair.Key);
                    foreach(string str in keyValuePair.Value) 
                    {
                        Console.WriteLine($"{str}");
                    }
                    Console.WriteLine("==========================================");
                }
            }
        }
    }
}


