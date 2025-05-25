namespace SimpleRESTServer.Models
{
    public class Product
    {
        public double Price { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }

        public Product() { }

        public Product(double price, string name, string producer)
        {
            Price = price;
            Name = name;
            Producer = producer;
        }
    }
}
