using Microsoft.AspNetCore.Mvc;
using SimpleRESTServer.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;

namespace SimpleRESTServer.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        // Symulacja bazy danych w pamięci (Dictionary)
        private static readonly Dictionary<long, Message> _messages = new Dictionary<long, Message>
        {
            { 1, new Message(1, "Pierwsza wiadomość", "Jan") },
            { 2, new Message(2, "Druga wiadomość", "Anna") }
        };

        // GET /api/messages - wszystkie wiadomości
        [HttpGet]
        public ActionResult<List<Message>> GetAll()
        {
            return _messages.Values.ToList();
        }

        // GET /api/messages/{id} - jedna wiadomość
        [HttpGet("{id}")]
        public ActionResult<Message> Get(long id)
        {
            if (_messages.TryGetValue(id, out var message))
                return Ok(message);

            return NotFound();
        }

        // POST /api/messages - tworzenie wiadomości
        [HttpPost]
        public ActionResult<Message> Create([FromBody] Message message)
        {
            var newId = _messages.Keys.Max() + 1;
            message.Id = newId;
            message.Created = DateTime.UtcNow;
            _messages[newId] = message;

            return CreatedAtAction(nameof(Get), new { id = newId }, message);
        }

        // PUT /api/messages/{id} - aktualizacja wiadomości
        [HttpPut("{id}")]
        public ActionResult<Message> Update(long id, [FromBody] Message updatedMessage)
        {
            if (!_messages.ContainsKey(id))
                return NotFound();

            updatedMessage.Id = id;
            updatedMessage.Created = _messages[id].Created;
            _messages[id] = updatedMessage;

            return Ok(updatedMessage);
        }

        // DELETE /api/messages/{id} - usuwanie wiadomości
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (_messages.Remove(id))
                return NoContent();

            return NotFound();
        }

        [HttpGet("params/{*any}")]
        public IActionResult TestParams(
        [FromQuery] string author,
        [FromHeader(Name = "User-Agent")] string userAgent)
        {
            var path = Request.Path.Value; // np. "/api/messages/params;tag=test;debug=true"

            // Wyodrębnij matrix parametry ręcznie
            var matrixPart = path?.Split("params", 2).LastOrDefault();
            var matrixParams = matrixPart?.Split(';')
                .Skip(1)
                .Select(p => p.Split('='))
                .Where(p => p.Length == 2)
                .ToDictionary(p => p[0], p => p[1]);

            var absoluteUrl = Request.GetDisplayUrl();

            return Ok(new
            {
                FromQuery = author,
                FromHeader = userAgent,
                MatrixParams = matrixParams,
                AbsoluteUrl = absoluteUrl,
                AllHeaders = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
            });
        }


    }
}
