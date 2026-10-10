using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs;
using SMS.Application.Services;

namespace SMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AttendanceTypesController : ControllerBase
{
    private readonly AttendanceTypeService _attendanceTypeService;

    public AttendanceTypesController(
        AttendanceTypeService attendanceTypeService)
    {
        _attendanceTypeService = attendanceTypeService;
    }

    // POST: api/attendance-types
    [HttpPost]
    [ProducesResponseType(
        typeof(AttendanceTypeMstResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AttendanceTypeMstResponse>> Create(
        [FromBody] CreateAttendanceTypeMstRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _attendanceTypeService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.AttendanceTypeId },
            response);
    }

    // GET: api/attendance-types?activeOnly=true
    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<AttendanceTypeMstResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AttendanceTypeMstResponse>>>
        GetAll(
            [FromQuery] bool activeOnly = false,
            CancellationToken cancellationToken = default)
    {
        var response = await _attendanceTypeService.GetAllAsync(
            activeOnly,
            cancellationToken);

        return Ok(response);
    }

    // GET: api/attendance-types/1
    [HttpGet("{id:long}")]
    [ProducesResponseType(
        typeof(AttendanceTypeMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AttendanceTypeMstResponse>> GetById(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        var response = await _attendanceTypeService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(response);
    }

    // PUT: api/attendance-types/1
    [HttpPut("{id:long}")]
    [ProducesResponseType(
        typeof(AttendanceTypeMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AttendanceTypeMstResponse>> Update(
        [FromRoute] long id,
        [FromBody] UpdateAttendanceTypeMstRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _attendanceTypeService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(response);
    }

    // PATCH: api/attendance-types/1/activate?updatedBy=101
    [HttpPatch("{id:long}/activate")]
    [ProducesResponseType(
        typeof(AttendanceTypeMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AttendanceTypeMstResponse>> Activate(
        [FromRoute] long id,
        [FromQuery] long? updatedBy = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _attendanceTypeService.ActivateAsync(
            id,
            updatedBy,
            cancellationToken);

        return Ok(response);
    }

    // PATCH: api/attendance-types/1/deactivate?updatedBy=101
    [HttpPatch("{id:long}/deactivate")]
    [ProducesResponseType(
        typeof(AttendanceTypeMstResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AttendanceTypeMstResponse>> Deactivate(
        [FromRoute] long id,
        [FromQuery] long? updatedBy = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _attendanceTypeService.DeactivateAsync(
            id,
            updatedBy,
            cancellationToken);

        return Ok(response);
    }
}
