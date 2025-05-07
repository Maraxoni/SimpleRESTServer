using System.Xml.Serialization;

namespace SimpleRESTServer.Models
{
    [XmlRoot("Message")]
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public Message() { }

        public Message(int id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
