using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{
    public class ProductsService
    {
        public async Task<List<Product>> GetProducts()
        {
            var url = "http://localhost:5000/api/products/getall";

            try
            {
                using(var client = new HttpClient())
                {
                    using (var res = await client.GetAsync(url))
                    {
                        using(var content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();

                            if (data != null)
                            {
                                var dataObj = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(data);
                                return dataObj;
                            }
                        }
                    }
                }
            }
            catch(Exception e) { Console.WriteLine(e.Message); }
            return new List<Product>();
        }
    }
}
