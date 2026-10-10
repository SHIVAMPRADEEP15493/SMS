using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;
using SMS.Application.Services;

namespace SMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ClassController : ControllerBase
{
    private readonly ClassService _classService;
    public ClassController(
        ClassService classService)
    {
        _classService = classService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(ClassMstResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClassMstResponse>> Create(
        [FromBody] CreateClassMstRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await _classService.CreateAsync(
                request,
                cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { classId = result.ClassId },
            result);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<ClassMstResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ClassMstResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result =
            await _classService.GetAllAsync(
                cancellationToken);

        return Ok(result);
    }

    [HttpGet("{classId:long}")]
    [ProducesResponseType(
        typeof(ClassMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClassMstResponse>> GetById(
        long classId,
        CancellationToken cancellationToken)
    {
        var result =
            await _classService.GetByIdAsync(
                classId,
                cancellationToken);

        return Ok(result);
    }

    [HttpPut("{classId:long}")]
    [ProducesResponseType(
        typeof(ClassMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClassMstResponse>> Update(
        long classId,
        [FromBody] UpdateClassMstRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await _classService.UpdateAsync(
                classId,
                request,
                cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{classId:long}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(
        long classId,
        CancellationToken cancellationToken)
    {
        await _classService.ActivateAsync(
            classId,
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{classId:long}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(
        long classId,
        CancellationToken cancellationToken)
    {
        await _classService.DeactivateAsync(
            classId,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{classId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        long classId,
        CancellationToken cancellationToken)
    {
        await _classService.DeleteAsync(
            classId,
            cancellationToken);

        return NoContent();
    }
}
