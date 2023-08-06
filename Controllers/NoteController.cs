using DnsWebApi.Models;
using DnsWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DnsWebApi.Controllers
{
    [ApiController]
    [Route("note")]
    public class NoteController : ControllerBase
    {
        private INoteService productService;

        public NoteController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [Route("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var result = await productService.GetById(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(Product product)
        {
            try
            {
                var result = await productService.Create(product);

                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<ActionResult> Edit(int id, Product product)
        {
            try
            {
                var result = await productService.Edit(id, product);

                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ItemNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await productService.Delete(id);

                return Ok(result);
            }
            catch (ItemNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("all-products")]
        public ActionResult GetAll()
        {
            return Ok(productService.GetAll());
        }


    }
}
