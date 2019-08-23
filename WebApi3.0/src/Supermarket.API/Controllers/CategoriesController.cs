﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Services;
using Supermarket.API.Extensions;
using Supermarket.API.Resources;

namespace Supermarket.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryResource>> GetAllAsync()
        {
            var categories = await _categoryService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);
            return resources;
        }


        [HttpGet("{index}")]
        public async Task<CategoryResource> GetIndex(int index)
        {
            var category = await _categoryService.GetCategoryByIndex(index);
            var resource = _mapper.Map<Category, CategoryResource>(category);
            return resource;
        }

        [HttpGet("{start}/{end}")]
        public async Task<IEnumerable<CategoryResource>> GetRange(int start, int end)
        {
            var categories = await _categoryService.GetCategoryByRange(start, end);
            var resources = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);
            return resources;
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveCategoryResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var category = _mapper.Map<SaveCategoryResource, Category>(resource);
            var result = await _categoryService.SaveAsync(category);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Category);
            return Ok(categoryResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveCategoryResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());


            //Json example.
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var jsonString = JsonConvert.SerializeObject(resource);
            var backToResource = JsonConvert.DeserializeObject<SaveCategoryResource>(jsonString);
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed for old Json: {stopwatch.ElapsedMilliseconds}ms");



            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            var jsonString2 = System.Text.Json.JsonSerializer.Serialize(resource);
            var backtoResource2 = System.Text.Json.JsonSerializer.Deserialize<SaveCategoryResource>(jsonString2);
            stopwatch2.Stop();
            Console.WriteLine($"Time elapsed for new Json: {stopwatch2.ElapsedMilliseconds}ms");



            var category = _mapper.Map<SaveCategoryResource, Category>(resource);
            var result = await _categoryService.UpdateAsync(id, category);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Category);
            return Ok(categoryResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Category);
            return Ok(categoryResource);
        }
    }
}