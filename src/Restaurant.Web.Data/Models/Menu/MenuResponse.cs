using System;

namespace Restaurant.Web.Data.Models.Menu
{
    public class MenuResponse
    {
        public string DishName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public Guid CategoryId { get; set; }
        public Guid MenuId { get; set; }
    }
}