using back_test_project.Models;
using back_test_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace back_test_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatsController : ControllerBase
    {
        private readonly ICatService _service;
        public CatsController(ICatService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cat>>> GetAll(CancellationToken ct = default)
        {
            var cats = await _service.GetAllAsync(ct);
            return Ok(cats);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cat>> GetById(int id, CancellationToken ct = default)
        {
            var cat = await _service.GetByIdAsync(id, ct);
            if (cat == null)
            {
                return NotFound($"Cat`s with id {id} not found");
            }
            return Ok(cat);
        }

    }
}
