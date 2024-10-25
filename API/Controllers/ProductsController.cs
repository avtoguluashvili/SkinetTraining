using System;
using Azure.Core.Pipeline;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{

    private readonly IProductRepository _repo;
    public ProductsController(IProductRepository repo)
    {
        _repo = repo;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await _repo.GetProductsAsync(brand, type, sort));
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProducts(int id)
    {

        var product = await _repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        _repo.AddProduct(product);
        if (await _repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProducts", new { id = product.Id }, product);
        }
        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("product not found");
        _repo.UpdateProduct(product);
        if (await _repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("problem updating product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _repo.GetProductByIdAsync(id);
        if (product == null) return BadRequest();
        _repo.DeleteProduct(product);
        if (await _repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await _repo.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await _repo.GetTypesAsync());
    }

    private bool ProductExists(int id)
    {
        return _repo.ProductExists(id);
    }
}