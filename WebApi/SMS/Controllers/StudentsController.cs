using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;
using SMS.Application.Services;

namespace SMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class StudentsController : ControllerBase
{
    private readonly StudentService _studentService;
    public StudentsController(StudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StudentMstResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StudentMstResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var students = await _studentService.GetAllAsync(cancellationToken);

        return Ok(students);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(StudentMstResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentMstResponse>> GetById(
        long id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than zero."
            });
        }

        var student = await _studentService.GetByIdAsync(
            id,
            cancellationToken);

        if (student is null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(StudentMstResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<StudentMstResponse>> Create(
        [FromBody] CreateStudentMstRequest request,
        CancellationToken cancellationToken)
    {
        var student = await _studentService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = student.StudentId },
            student);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(
        typeof(StudentMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentMstResponse>> Update(
        long id,
        [FromBody] UpdateStudentMstRequest request,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than zero."
            });
        }

        var student = await _studentService.UpdateAsync(
            id,
            request,
            cancellationToken);

        if (student is null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPatch("{id:long}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateStatus(
        long id,
        [FromBody] UpdateStudentStatusRequest request,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than zero."
            });
        }

        var updated = await _studentService.UpdateStatusAsync(
            id,
            request.Status,
            request.UpdatedBy,
            cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}
