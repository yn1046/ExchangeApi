using NumDict = System.Collections.Generic.Dictionary<string, double>;

namespace ExchangeApi.Models
{
    public class AddExchangeRequest
    {
        public int Balance { get; set; }
        public NumDict Rates { get; set; }
    }
}