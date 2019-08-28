using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{
    public class Product
    {
        
        public int id { get; set; }
        public string name { get; set; }
        public int quantityInPackage { get; set; }
        public string unitOfMeasurement { get; set; }
        public Category category { get; set; }
    }
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
