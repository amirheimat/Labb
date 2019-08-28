using Newtonsoft.Json;
using Supermarket.API.Domain.Models; 
using Supermarket.API.Domain.Repositories;
using Supermarket.API.Domain.Services;
using Supermarket.API.Domain.Services.Communication;
using Supermarket.API.Extensions;
using Supermarket.API.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Supermarket.API.Services
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;


        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> ListAsync()
        {
            return await _productRepository.ListAsync();
        }

        public async Task<IEnumerable<Product>> ListNewAsync()
        {
            var products = new List<Product>();

            await foreach (var product in _productRepository.ListNewAsync())
            {
                Console.WriteLine($"Received {product.Name}.");
                // work
                // 
                if (IsFourGPhone(product))
                {
                    Console.WriteLine($"{product.Name.ToString()} has 4G!.");
                    products.Add(product);

                }
                else if (IsWeight(product))
                {
                    Console.WriteLine($"{product.Name} has has weight!.");
                    products.Add(product);
                }
                ////
                //// ....
                JsonExample(product);

            }

            return products;

        }

        private bool IsFourGPhone(Product product)
        {
            // Show Recursive Patterns!
            if (product is Phone  { Band: EBand.FourG })
            {
                return true;
                //if (phone.Band == EBand.FourG)
                //{
                //    return true;
                //}
            }
            return false;
        }

        private bool IsWeight(Product product)
        {
            if (product.UnitOfMeasurement.IsWeight())
            {
                return true;
            }
            return false;
        }

        private void JsonExample(Product product)
        {

            //Json example.
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var jsonString = JsonConvert.SerializeObject(product);
            var backToResource = JsonConvert.DeserializeObject<SaveCategoryResource>(jsonString);
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed for old Json {jsonString}: {stopwatch.ElapsedMilliseconds}ms");


            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            var jsonString2 = System.Text.Json.JsonSerializer.Serialize(product);
            var backtoResource2 = System.Text.Json.JsonSerializer.Deserialize<SaveCategoryResource>(jsonString2);
            stopwatch2.Stop();
            Console.WriteLine($"Time elapsed for new Json {jsonString2}: {stopwatch2.ElapsedMilliseconds}ms");
        }

        public async Task<ProductResponse> DeleteAsync(int id)
        {
            var existingProduct = await _productRepository.FindByIdAsync(id);

            if (existingProduct == null)
                return new ProductResponse("Product not found.");

            try
            {
                _productRepository.Remove(existingProduct);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(existingProduct);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ProductResponse($"An error occurred when deleting the product: {ex.Message}");
            }
        }

        public async Task<ProductResponse> SaveAsync(Product product)
        {
            try
            {
                await _productRepository.AddAsync(product);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(product);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ProductResponse($"An error occurred when saving the product: {ex.Message}");
            }
        }

        public async Task<ProductResponse> UpdateAsync(int id, Product product)
        {
            var existingProduct = await _productRepository.FindByIdAsync(id);

            if (existingProduct == null)
                return new ProductResponse("Product was not found.");

            existingProduct.Name = product.Name;

            try
            {
                _productRepository.Update(existingProduct);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(existingProduct);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ProductResponse($"An error occurred when updating the product: {ex.Message}");
            }
        }
    }
}