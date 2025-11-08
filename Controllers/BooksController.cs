using back_test_project.DTO;
using back_test_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace back_test_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(IBookService service) : ControllerBase
    {
        private readonly IBookService _service = service;

        //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<BookDataDto>>> GetAll(CancellationToken ct)
        //{
        //    var items = await _service.GetAllDataAsync(ct);
        //    return Ok(items);
        //}

        [HttpGet("page")]
        public async Task<ActionResult<PagedResultDto<BookDataDto>>> GetPage(
            [FromQuery] BookPageQueryDto query,
            CancellationToken ct = default)
        {
            var (items, total) = await _service.GetPageAsync(query, ct);
            return Ok(new PagedResultDto<BookDataDto> { Data = items, TotalCount = total });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookReadDto>> GetById(int id, CancellationToken ct)
        {
            var item = await _service.GetReadonlyByIdAsync(id, ct);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] BookCreateDto dto, CancellationToken ct)
        {
            var result = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDto dto, CancellationToken ct)
        {
            bool updated = await _service.UpdateAsync(id, dto, ct);
            if (!updated)
            {
                return NotFound($"Book with id {id} not found or update failed");
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            bool deleted = await _service.DeleteAsync(id, ct);
            if (!deleted)
            {
                return NotFound($"Book with id {id} not found or delete failed");
            }
            return NoContent();
        }

    }
}
