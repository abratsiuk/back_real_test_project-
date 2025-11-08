using back_test_project.DTO;
using back_test_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace back_test_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeesController(IEmployeeService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<EmployeeDataDto>>> GetAll(CancellationToken ct)
        {
            var items = await _service.GetAllDataAsync(ct);
            return Ok(items);
        }

        [HttpGet("page")]
        public async Task<ActionResult<PagedResultDto<EmployeeDataDto>>> GetPage(
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sort = "lastName",
        [FromQuery] string order = "asc",
        CancellationToken ct = default)
        {
            var (items, total) = await _service.GetPageAsync(page, pageSize, sort, order, ct);
            return Ok(new PagedResultDto<EmployeeDataDto> { Data = items, TotalCount = total });
        }

        [HttpGet("options")]
        public async Task<ActionResult<IReadOnlyList<EmployeeOptionDto>>> GetOptions(CancellationToken ct)
        {
            var items = await _service.GetOptionsAsync(ct);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeReadDto>> GetById(int id, CancellationToken ct)
        {
            var item = await _service.GetReadonlyByIdAsync(id, ct);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] EmployeeCreateDto dto, CancellationToken ct)
        {
            var result = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto, CancellationToken ct)
        {
            var updated = await _service.UpdateAsync(id, dto, ct);
            if (!updated)
            {
                return NotFound($"Employee with id {id} not found or update failed");
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            if (!deleted)
            {
                return NotFound($"Employee with id {id} not found or delete failed");
            }
            return NoContent();
        }

        [HttpGet("{id:int}/can-delete")]
        public async Task<ActionResult<EmployeeCanDeleteDto>> CanDelete(int id, CancellationToken ct)
        {
            var result = await _service.CanDeleteAsync(id, ct);
            return Ok(result);
        }
    }
}
