using System.Net;
using Marktguru.API.Models;
using Marktguru.Application.Products.Commands;
using Marktguru.Application.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marktguru.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductsController : ApiControllerBase
{
    // GET: api/products
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ProductsViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductsViewModel>>> GetProducts()
    {
        var products = await Mediator.Send(new GetProductsQuery());
        return Ok(Mapper.Map<IEnumerable<ProductsViewModel>>(products));
    }

    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ProductViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductViewModel>> GetProduct(Guid id)
    {
        var product = await Mediator.Send(new GetProductQuery(id));
        return Ok(Mapper.Map<ProductViewModel>(product));
    }

    // POST: api/products
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> CreateProduct(CreateProductViewModel product)
    {
        await Mediator.Send(new CreateProductCommand(product.Name, product.Availability, product.Price,
            product.Description));
        return Created();
    }

    // PUT: api/products/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductViewModel product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(new UpdateProductCommand(product.Id, product.Name, product.Availability, product.Price,
            product.Description));

        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await Mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}