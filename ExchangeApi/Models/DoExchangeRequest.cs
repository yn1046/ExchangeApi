using System;

namespace ExchangeApi.Models
{
    public class DoExchangeRequest
    {
        public string Currencies { get; set; }
        public double Amount { get; set; }
    }
}