using System.Text.Json.Serialization;

namespace WebAPI.Model
{
    public class Order
    {
        [JsonIgnore] 
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
