using back_test_project.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthorsController : ControllerBase
{
    private readonly IAuthorService _svc;
    public AuthorsController(IAuthorService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<object>> Get(string? q = null, int page = 1, int size = 20)
    {
        var (items, total) = await _svc.GetPageAsync(page, size, q);
        return Ok(new { total, page, size, items });
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<AuthorDto>>> Search(string? q = null, int take = 20)
        => Ok(await _svc.SearchAsync(q, take));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorDto>> GetById(int id)
    {
        var dto = await _svc.GetByIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateAuthorDto dto)
    {
        var id = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateAuthorDto dto)
    {
        await _svc.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        return NoContent();
    }
}
