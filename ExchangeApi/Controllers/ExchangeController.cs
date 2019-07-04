using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeApi.Models;
using ExchangeApi.Services;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

/* Есть несколько бирж с их API, ключами (сделать имитацию работы с биржей через API)
В этих биржах есть кошельки разных валют. (для примера USD, RUB, EUR)
На этих кошельках есть определенная сумма. В каждой бирже разная сумма лежит, но в одинаковом процентном соотношении.
пример курса на момент распределения
RUB/USD => 60
EUR/USD => 1.25
USD/USD => 1
Пример соотношения распределения: RUB => 40%, USD => 20%, EUR => 40%
Примеры распределенного баланса
Биржа 1: RUB => 1200, USD => 10, EUR => 25
Биржа 2: RUB => 600, USD => 5, EUR => 12.5
Надо сделать функционал перераспределения денег в биржах в % соотношении, которое указывается на вход программы.
За основной курс брать USD, У каждой биржи есть стоимость валюты по отношению к USD
Api каждой биржи позволяет получать текущий курс пары валют getPrice("RUBUSD") - стоимость рубля по отношению к доллару
совершать обмен валюты exchange("USDRUB", 15) - меняет 15 долларов на рубли, по текущему курсу биржи
getBalances() - Получает баланс всех валют в бирже. возвращает ['RUB' => 30000, 'USD' => 1200, 'EUR' => 4500]
Биржи могут добавляться или удаляться. Надо сделать чтобы добавление или изменение новой биржи было как можно проще
Для работы с биржей нужен apiKey. Желательно этот ключ не светить в коде, только где нибудь в конфиге.
Желательно это сделать с ООП, и чтобы можно было как то запустить(консоль/веб) и посмотреть хотя бы с рандомными данными.
Желательно не использовать фреймворк */

namespace ExchangeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private ExchangeRepository _repository;

        public ExchangeController(ExchangeRepository repository)
        {
            _repository = repository;
        }
        
        // GET api/exchange
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new JsonResult(_repository.GetAll());
        }

        // GET api/exchange/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            return new JsonResult(_repository.GetByApiKey(id));
        }

        // GET api/exchange/price/USDRUB
        [Route("price/{currencies}")]
        public JsonResult GetPrice(string currencies)
        {
            return new JsonResult(new {Kek = "lol"});
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}