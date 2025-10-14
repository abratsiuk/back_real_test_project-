using back_test_project.DTO;
using back_test_project.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public sealed class BooksFullController : ControllerBase
{
    private readonly IBookFullService _svc;
    public BooksFullController(IBookFullService svc) => _svc = svc;

    [HttpGet]                          // GET /api/booksfull?page=1&size=20&q=ab
    public async Task<ActionResult<object>> Get(int page = 1, int size = 20, string? q = null)
    {
        var (items, total) = await _svc.GetPageAsync(page, size, q);
        return Ok(new { total, page, size, items });
    }

    [HttpGet("{id:int}")]              // GET /api/booksfull/5
    public async Task<ActionResult<BookFullDto>> GetById(int id)
    {
        var dto = await _svc.GetByIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]                         // POST /api/booksfull
    public async Task<ActionResult<int>> Create([FromBody] CreateBookFullDto dto)
    {
        var id = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]              // PUT /api/booksfull/5
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookFullDto dto)
    {
        await _svc.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]           // DELETE /api/booksfull/5
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        return NoContent();
    }
}
