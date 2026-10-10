using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Features.StudentEnrollments.DTOs;
using SMS.Application.Features.StudentEnrollments.Services;

namespace SMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class StudentEnrollmentsController : ControllerBase
{
    private readonly StudentEnrollmentService _studentEnrollmentService;

    public StudentEnrollmentsController(StudentEnrollmentService studentEnrollmentService)
    {
        _studentEnrollmentService = studentEnrollmentService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(StudentEnrollmentResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<StudentEnrollmentResponse>> Create(
        [FromBody] CreateStudentEnrollmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _studentEnrollmentService.CreateAsync(
            request,
            cancellationToken: cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.StudentEnrollmentId },
            result);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<StudentEnrollmentResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StudentEnrollmentResponse>>>
        GetAll(CancellationToken cancellationToken)
    {
        var result = await _studentEnrollmentService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(
        typeof(StudentEnrollmentResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentEnrollmentResponse>> GetById(
        long id,
        CancellationToken cancellationToken)
    {
        var result = await _studentEnrollmentService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(
        typeof(StudentEnrollmentResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<StudentEnrollmentResponse>> Update(
        long id,
        [FromBody] UpdateStudentEnrollmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _studentEnrollmentService.UpdateAsync(
            id,
            request,
            cancellationToken: cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:long}/status")]
    [ProducesResponseType(
        typeof(StudentEnrollmentResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<StudentEnrollmentResponse>> UpdateStatus(
        long id,
        [FromBody] UpdateStudentEnrollmentStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _studentEnrollmentService.UpdateStatusAsync(
            id,
            request,
            cancellationToken: cancellationToken);

        return Ok(result);
    }
}
