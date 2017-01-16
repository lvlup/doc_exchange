using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IGetCurrencyCourse
    {
        decimal GetUsdCurrency();

        decimal GetEuroCurrency();

        decimal GetCurrencyByCode(Currency currency);
    }
}
