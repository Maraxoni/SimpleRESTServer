namespace SimpleRESTServer.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public Comment() { }

        public Comment(long id, string text, string author)
        {
            Id = id;
            Text = text;
            Author = author;
            Created = DateTime.UtcNow;
        }
    }
}
