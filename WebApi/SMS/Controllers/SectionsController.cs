using Microsoft.AspNetCore.Mvc;
using SMS.Application.Services;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;

namespace SMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class SectionsController : ControllerBase
{
    public readonly SectionService _sectionService;
    public SectionsController(SectionService sectionService)
    {
        _sectionService = sectionService;
    }

    [HttpPost]
    [ProducesResponseType(
    typeof(SectionMstResponse),
    StatusCodes.Status201Created)]
    [ProducesResponseType(
    typeof(ValidationProblemDetails),
    StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
    typeof(ProblemDetails),
    StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SectionMstResponse>> Create(
    [FromBody] CreateSectionMstRequest request,
    CancellationToken cancellationToken)
    {
        var response = await _sectionService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { sectionId = response.SectionId },
            response);
    }

    [HttpGet("{sectionId:long}")]
    [ProducesResponseType(
        typeof(SectionMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SectionMstResponse>> GetById(
        long sectionId,
        CancellationToken cancellationToken)
    {
        var response = await _sectionService.GetByIdAsync(
            sectionId,
            cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<SectionMstResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SectionMstResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var response = await _sectionService.GetAllAsync(
            cancellationToken);

        return Ok(response);
    }

    [HttpPut("{sectionId:long}")]
    [ProducesResponseType(
        typeof(SectionMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SectionMstResponse>> Update(
        long sectionId,
        [FromBody] UpdateSectionMstRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sectionService.UpdateAsync(
            sectionId,
            request,
            cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{sectionId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        long sectionId,
        CancellationToken cancellationToken)
    {
        await _sectionService.DeleteAsync(
            sectionId,
            cancellationToken);

        return NoContent();
    }
}
