using CarPartsShop.API.DTO;
using CarPartsShop.Models.ViewModels;
using CarPartsShop.Services.PartService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarPartsShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartsController : ControllerBase
    {
        private readonly ApiService apiService;

        public PartsController(ApiService apiService)
        {
            this.apiService = apiService;
        }


        [HttpGet]
        public async Task<ActionResult<List<PartDTO>>> GetParts()
        {
            var parts = await apiService.GetAll();
            return Ok(parts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<PartDTO>>> GetPartById(string id)
        {
            var parts = await apiService.GetById(id);
            return Ok(parts);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Uploads a file")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CreatePartDTO>> Create([FromForm] CreatePartDTO model)
        {
            var part = await apiService.CreatePart(model);
            return CreatedAtAction(nameof(GetPartById), new { id = part.Id }, part);
        }
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Uploads a file")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PartDTO>> Update(string id, [FromForm] CreatePartDTO model)
        {
            var part = await apiService.EditPart(model, id);
            if (part == null)
            {
                return NotFound();
            }
            return Ok(part);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await apiService.DeletePart(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
