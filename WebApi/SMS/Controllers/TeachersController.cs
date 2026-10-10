using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Services;
using SMS.Application.DTOs;

namespace SMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeachersController : ControllerBase
{
    private readonly TeacherService _teacherService;
    public TeachersController(TeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(TeacherMstResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TeacherMstResponse>> Create(
        [FromBody] CreateTeacherMstRequest request,
        CancellationToken cancellationToken)
    {
        var response =
            await _teacherService.CreateAsync(
                request,
                cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.TeacherId },
            response);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<TeacherMstResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TeacherMstResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var response =
            await _teacherService.GetAllAsync(
                cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(
        typeof(TeacherMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeacherMstResponse>> GetById(
        long id,
        CancellationToken cancellationToken)
    {
        var response =
            await _teacherService.GetByIdAsync(
                id,
                cancellationToken);

        return Ok(response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(
        typeof(TeacherMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeacherMstResponse>> Update(
        long id,
        [FromBody] UpdateTeacherMstRequest request,
        CancellationToken cancellationToken)
    {
        var response =
            await _teacherService.UpdateAsync(
                id,
                request,
                cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{id:long}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(
        long id,
        CancellationToken cancellationToken)
    {
        await _teacherService.ActivateAsync(
            id,
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:long}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(
        long id,
        CancellationToken cancellationToken)
    {
        await _teacherService.DeactivateAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}
