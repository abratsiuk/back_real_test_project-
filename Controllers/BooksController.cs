// Controllers/BooksController.cs
using back_test_project.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public sealed class BooksController : ControllerBase
{
    private readonly IBookService _svc;
    public BooksController(IBookService svc) => _svc = svc;

    // GET /api/books?page=1&size=20&q=abc
    [HttpGet]
    public async Task<ActionResult<object>> Get(int page = 1, int size = 20, string? q = null)
    {
        var (items, total) = await _svc.GetPageAsync(page, size, q);
        return Ok(new { total, page, size, items });
    }

    // GET /api/books/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDetailsDto>> GetById(int id)
    {
        var dto = await _svc.GetByIdReadOnlyAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    // POST /api/books
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateBookDto dto)
    {
        var id = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    // PUT /api/books/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)
    {
        await _svc.UpdateAsync(id, dto);
        return NoContent();
    }

    // DELETE /api/books/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        return NoContent();
    }
}
