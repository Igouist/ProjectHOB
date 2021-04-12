using Newtonsoft.Json.Linq;
using ProjectHOB.Crawler;
using ProjectHOB.Generator;
using ProjectHOB.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHOB
{
    class Program
    {
        private static ParameterDataModel _parameter;

        static async Task Main(string[] args)
        {
            _parameter = JObject.Parse(File.ReadAllText(@"parameter.json")).ToObject<ParameterDataModel>();

            Console.WriteLine("開始取得網頁內文…");

            var crawler = new CrawlerService();
            var contents = await crawler.FetchData(
                _parameter.MainUrl,
                _parameter.ListQuerySelector,
                _parameter.ContextQuerySelector);

            if (contents is null || contents.Any() is false)
            {
                Console.WriteLine("無法取得網頁內文，即將退出。");
                return;
            }

            Console.WriteLine("網頁內文已取得。開始轉換格式…");

            var generator = new EPubGenerator();
            await generator.GenerateEpub(contents);

            Console.WriteLine("作業已完成。");
            Console.ReadLine();
        }
    }
}
