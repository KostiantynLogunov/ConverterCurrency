
namespace WorkWithBankAPI.Models
{
    public class CurrencyConverter
    {
        public decimal USD { get; set; } = 0;
        public decimal ConvertToUSD(decimal priceUA) => priceUA / USD;

        public decimal EUR { get; set; } = 0;
        public decimal ConvertToEUR(decimal priceUA) => priceUA / EUR;

        public decimal RUB { get; set; } = 0;
        public decimal ConvertToRUB(decimal priceUA) => priceUA / RUB;
    }
}
