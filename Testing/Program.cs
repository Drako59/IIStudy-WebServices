
using System.Text;
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Runtime.Remoting;
using System.ComponentModel;
using LLstudyWS;

namespace Testing
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            checkInsert();
            //TestBook();
            //TestRegistered();
            //GetCurrenyExchange();
            //GetHolidDays();
            //GetNextHoliday();
            //TranslateSentence("בן אדם ירוק בשיחה איתי");

            Console.ReadLine();
            
        }
        static void checkInsert()
        {
            
            Console.WriteLine( "Insert event name: ");
            string name = Console.ReadLine();
            string date = Console.ReadLine();
            string details = Console.ReadLine();
            DbHelperOledb dbHelperOledb = new DbHelperOledb();
            dbHelperOledb.OpenConnection();
            int result = dbHelperOledb.Insert($"INSERT INTO Events(event_name,date_event,details) VALUES('{name}','{date}','{details}') ");
            Console.WriteLine( result);
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
                //await Console.Out.WriteLineAsync("hi");
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                CurrecnyExchange cur = JsonSerializer.Deserialize<CurrecnyExchange>(body);
                Console.WriteLine($" {cur.query.amount} {cur.query.from} == {cur.result} {cur.query.to}");
            }

        }
        static async Task<List<Holiday>> GetHolidDays(string start_date = "2025-10-01", string end_date = "2025-10-31")
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://israel-holidays-workdays-api.p.rapidapi.com/holidays/range?start_date={start_date}&end_date={end_date}"),
                Headers =
    {
        { "x-rapidapi-key", "4d056610c5msh0d91eab1e9347c6p1269c2jsnb2ec8c0be52f" },
        { "x-rapidapi-host", "israel-holidays-workdays-api.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                List<Holiday> holidays = JsonSerializer.Deserialize<List<Holiday>>(body);
                foreach (Holiday holiday in holidays) //DEBUG
                {
                    Console.WriteLine($"{holiday.name} , {holiday.date} , is work day: {holiday.is_work_day}"); //DEBUG
                } // DEBUG
                return holidays;
            }
        }
        static async Task<Holiday> GetNextHoliday()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://israel-holidays-workdays-api.p.rapidapi.com/next-holiday?from_date=2025-10-02&timezone=Asia%2FJerusalem"),
                Headers =
    {
        { "x-rapidapi-key", "4d056610c5msh0d91eab1e9347c6p1269c2jsnb2ec8c0be52f" },
        { "x-rapidapi-host", "israel-holidays-workdays-api.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Holiday holiday = JsonSerializer.Deserialize<Holiday>(body);
                Console.WriteLine($"{holiday.name} , {holiday.date} , is work day: {holiday.is_work_day}"); //DEBUG
                Console.WriteLine("========================================================================"); //DEBUG
                return holiday;
            }   
        }
        static async Task<Translations> TranslateSentence(string text,string source = "iw", string translate = "en")
        {
            //Console.WriteLine("{\"q\":" + text + ",\"source\" :" + source + ",\"target\":" + translate + "}");
            string jsonBody = "{"
            + "\"q\":\"" + text + "\","
            + "\"source\":\"" + source + "\","
            + "\"target\":\"" + translate + "\""
            + "}";
                    var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://deep-translate1.p.rapidapi.com/language/translate/v2"),
                Headers =
                {
                    { "x-rapidapi-key", "4d056610c5msh0d91eab1e9347c6p1269c2jsnb2ec8c0be52f" },
                    { "x-rapidapi-host", "deep-translate1.p.rapidapi.com" },
                },
                Content = new StringContent(jsonBody, Encoding.UTF8)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(body);
                Translate translate_obj = JsonSerializer.Deserialize<Translate>(body);
                //Console.WriteLine(translate_obj);
                Translations translated_obj2 = translate_obj.data.translations;

                //Console.WriteLine(translated_obj2.translatedText[0]);
                return translated_obj2;
            }
        }
        static void TestRegistered()
        {
            Registered user = new Registered()
            {
                UserName = "A",
                Password = "password",
                Email = "notanemail",
                Role = "Admin",
                Birth = "2000-01-01"
            };
            if (user.HasErrors)
            {

                foreach (KeyValuePair<string, List<string>> keyValuePair in user.AllErrors())
                {
                    Console.WriteLine(keyValuePair.Key);
                    foreach (string str in keyValuePair.Value)
                    {
                        Console.WriteLine($"{str}");
                    }
                    Console.WriteLine("==========================================");
                }
            }
        }
        static void TestBook()
        {
            Book book = new Book() { Author_name = "Hello"
                , Book_ID = "a"
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


