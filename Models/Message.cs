using System.Xml.Serialization;

namespace SimpleRESTServer.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public List<Link> Links { get; set; } = new();

        public Message() { }
        public Message(Message message)
        {
            Id = message.Id;
            Content = message.Content;
            Author = message.Author;
            Created = message.Created;
            Comments = message.Comments;
        }

        public Message(long id, string content, string author)
        {
            Id = id;
            Content = content;
            Author = author;
            Created = DateTime.UtcNow;
        }
    }
}
