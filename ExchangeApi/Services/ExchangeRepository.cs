using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeApi.Models;
using LiteDB;
using NumDict = System.Collections.Generic.Dictionary<string, double>;

namespace ExchangeApi.Services
{
    public class ExchangeRepository
    {
        public NumDict Percentages { get; set; }
        
        private ConnectionString connectionString =
            new ConnectionString("exchanges.db")
            {
                Mode = FileMode.ReadOnly
            };
        
        private ConnectionString writeConnectionString =
            new ConnectionString("exchanges.db")
            {
                Mode = FileMode.Exclusive
            };
        
        private string collectionName = "exchanges";

        public ExchangeRepository()
        {
            InitializeDatabase();
        }

        public Exchange GetByApiKey(Guid apiKey)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var collection = db.GetCollection<Exchange>(collectionName);
                var exchange = collection.FindById(apiKey);
                return exchange;
            }
        }

        public List<Exchange> GetAll()
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var collection = db.GetCollection<Exchange>(collectionName);
                var exchanges = collection.FindAll().ToList();
                return exchanges;
            }
        }

        public Guid Add(Exchange exchange)
        {
            using (var db = new LiteDatabase(writeConnectionString))
            {
                var collection = db.GetCollection<Exchange>(collectionName);
                collection.Insert(exchange);
                return exchange.ApiKey;
            }
        }

        public void Remove(Guid id)
        {
            using (var db = new LiteDatabase(writeConnectionString))
            {
                var collection = db.GetCollection<Exchange>(collectionName);
                collection.Delete(id);
            }
        }
        
        private void InitializeDatabase()
        {
            using (var db = new LiteDatabase(writeConnectionString))
            {
                Percentages = new NumDict
                {
                    ["RUB"] = 40,
                    ["USD"] = 20,
                    ["EUR"] = 40
                };
                
                if (db.CollectionExists(collectionName)) return;

                var exchangeOne = new Exchange
                {
                    Rates = new NumDict
                    {
                        ["RUB"] = 60,
                        ["EUR"] = 1.25,
                        ["USD"] = 1
                    }
                };
                var exchangeTwo = new Exchange
                {
                    Rates = new NumDict
                    {
                        ["RUB"] = 65,
                        ["EUR"] = 1.20,
                        ["USD"] = 1
                    }
                };
                var exchangeThree = new Exchange
                {
                    Rates = new NumDict
                    {
                        ["RUB"] = 59,
                        ["EUR"] = 1.24,
                        ["USD"] = 1
                    }
                };

                exchangeOne.SetBalances(Percentages, 100);
                exchangeTwo.SetBalances(Percentages, 75);
                exchangeThree.SetBalances(Percentages, 120);
                
                var exchanges = db.GetCollection<Exchange>(collectionName);
                exchanges.InsertBulk(new []
                {
                    exchangeOne,
                    exchangeTwo,
                    exchangeThree
                });
            }
        }


    }
}