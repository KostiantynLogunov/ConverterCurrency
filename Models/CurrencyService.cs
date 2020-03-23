using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace WorkWithBankAPI.Models
{
    public class CurrencyService : BackgroundService
    {
        private readonly IMemoryCache memoryCache;

        public CurrencyService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // виставляем культуру на руский
                    // например при разности культур в валютах может тирятся запитая
                    //Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

                    //если будет нестандартная кодировка для .net core ,например windows-1251 получим исключение
                    //по этому подключаем такой провайдер, что бы не стандартн кодировка была доступна
                    //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                    //поскольку даные будут в формате ХМЛ то мы юзаем XDocument
                    XDocument xml = XDocument.Load("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange");

                    //распарсить документ
                    CurrencyConverter currencyConverter = new CurrencyConverter();
                    //currencyConverter.USD = Convert.ToDecimal(usd1, CultureInfo.InvariantCulture);
                    currencyConverter.USD = Convert.ToDecimal(xml.Elements("exchange").Elements("currency")
                        .FirstOrDefault(x => x.Element("r030").Value=="840").Elements("rate").FirstOrDefault().Value);

                    currencyConverter.EUR = Convert.ToDecimal(xml.Elements("exchange").Elements("currency")
                        .FirstOrDefault(x => x.Element("r030").Value == "978").Elements("rate").FirstOrDefault().Value);

                    currencyConverter.RUB = Convert.ToDecimal(xml.Elements("exchange").Elements("currency")
                        .FirstOrDefault(x => x.Element("r030").Value == "643").Elements("rate").FirstOrDefault().Value);

                    // set in memory RAM currencyConverter for 1440 minutes
                    memoryCache.Set("key currency", currencyConverter, TimeSpan.FromMinutes(1440));
                }
                catch (Exception e)
                {

                    //logs...
                }

                await Task.Delay(3600000, stoppingToken);

            }
        }
    }
}
