using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LiteDB;

namespace ExchangeApi.Models
{
    public class Exchange
    {
        [BsonId]
        public Guid ApiKey { get; set; }
        public double Balance { get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }
}