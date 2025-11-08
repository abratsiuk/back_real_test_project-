using back_test_project.DTO;
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

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CatDataDto>>> GetAll(CancellationToken ct = default)
        //{
        //    var cats = await _service.GetAllAsync(ct);
        //    return Ok(cats);
        //}

        [HttpGet("page")]
        public async Task<ActionResult<PagedResultDto<CatDataDto>>> GetPage(
           [FromQuery] CatPageQueryDto query,
           CancellationToken ct = default)
        {
            var (items, total) = await _service.GetPageAsync(query, ct);

            var result = new PagedResultDto<CatDataDto>
            {
                Data = items,
                TotalCount = total
            };

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CatDataDto>> GetById(int id, CancellationToken ct = default)
        {
            var cat = await _service.GetReadOnlyByIdAsync(id, ct);
            if (cat == null)
            {
                return NotFound($"Cat with id {id} not found");
            }
            return Ok(cat);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CatCreateDto dto, CancellationToken ct = default)
        {
            var newCatId = await _service.CreateAsync(dto, ct);
            var result = new CatDataDto { Id = newCatId, Name = dto.Name, Age = dto.Age };
            return CreatedAtAction(nameof(GetById), new { id = newCatId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CatUpdateDto dto, CancellationToken ct = default)
        {
            var updated = await _service.UpdateAsync(id, dto, ct);
            if (!updated)
            {
                return NotFound($"Cat with id {id} not found or update failed");
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken ct = default)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            if (!deleted)
            {
                return NotFound($"Cat with id {id} not found or delete failed");
            }
            return NoContent();
        }



    }
}
