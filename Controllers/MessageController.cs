using Microsoft.AspNetCore.Mvc;
using SimpleRESTServer.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using System.Globalization;

namespace SimpleRESTServer.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        // Data
        private static readonly Dictionary<long, Message> _messages = new Dictionary<long, Message>
        {
            { 1, new Message(1, "Pierwsza wiadomość", "Jan") },
            { 2, new Message(2, "Druga wiadomość", "Anna") }
        };

        // GET /api/messages
        [HttpGet]
        public ActionResult<List<Message>> GetAll([FromQuery(Name = "startswith")] string? startsWith = null)
        {
            var result = _messages.Values.AsEnumerable();

            if (!string.IsNullOrEmpty(startsWith))
            {
                result = result.Where(m => m.Content.StartsWith(startsWith, true, CultureInfo.InvariantCulture));
            }

            return result.ToList();
        }

        // GET /api/messages/{id}
        [HttpGet("{id}")]
        public ActionResult<Message> Get(long id)
        {
            if (_messages.TryGetValue(id, out var message))
                return Ok(message);

            return NotFound();
        }

        // POST /api/messages
        [HttpPost]
        public ActionResult<Message> Create([FromBody] Message message)
        {
            var newId = _messages.Keys.Max() + 1;
            message.Id = newId;
            message.Created = DateTime.UtcNow;
            _messages[newId] = message;

            return CreatedAtAction(nameof(Get), new { id = newId }, message);
        }

        // PUT /api/messages/{id}
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

        // DELETE /api/messages/{id}
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
