using Microsoft.AspNetCore.Mvc;
using SimpleRESTServer.Models;
using System.Collections.Generic;

namespace SimpleRESTServer.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        // Ćwiczenie 4 & 5 — domyślnie JSON, XML gdy nagłówek Accept = application/xml
        [HttpGet]
        public ActionResult<List<Message>> Get()
        {
            return new List<Message>
            {
                new Message(1, "Pierwsza wiadomość"),
                new Message(2, "Druga wiadomość")
            };
        }
    }
}