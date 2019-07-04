using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LiteDB;
using NumDict = System.Collections.Generic.Dictionary<string, double>;

namespace ExchangeApi.Models
{
    public class Exchange
    {
        [BsonId]
        public Guid ApiKey { get; set; } = Guid.NewGuid();
        public NumDict Balances { get; set; }
        public NumDict Rates { get; set; }

        public double GetPrice(string currencies)
        {
            var from = currencies.Substring(0,3);
            var to = currencies.Substring(3);
            return Rates[from] / Rates[to];
        }

        public bool CanExchange(string currencies, double amount)
        {
            var from = currencies.Substring(0, 3);
            return Balances[from] >= amount;
        }

        public NumDict SetBalances(NumDict percentages, double usdAmount)
        {
            Balances = percentages.ToDictionary(
                kv => kv.Key,
                kv => percentages[kv.Key] / 100 * usdAmount * Rates[kv.Key]);

            return Balances;
        }

        public NumDict UpdatePercentages(NumDict percentages)
        {
            Balances = percentages.ToDictionary(
                kv => kv.Key,
                kv => percentages[kv.Key] / 100 * GetFullBalanceInUsd());

            return Balances;
        }

        public double GetFullBalanceInUsd() => Balances.Sum(balance =>
        {
            var (key, value) = balance;
            return value / Rates[key];
        });

        public override int GetHashCode() => ApiKey.GetHashCode();

        public override bool Equals(object obj) =>
            obj is Exchange exchange && exchange.ApiKey == ApiKey;
    }
}