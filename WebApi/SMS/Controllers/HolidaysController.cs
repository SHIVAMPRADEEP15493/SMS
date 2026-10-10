using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;
using SMS.Application.Services;

namespace SMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class HolidaysController : ControllerBase
{
    private readonly HolidayService _holidayService;
    public HolidaysController(HolidayService holidayService)
    {
        _holidayService = holidayService;
    }

    // POST: api/holidays
    [HttpPost]
    [ProducesResponseType(typeof(HolidayDtlResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<HolidayDtlResponse>> Create(
        [FromBody] CreateHolidayDtlRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _holidayService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { holidayId = result.HolidayId },
            result);
    }

    // GET: api/holidays?schoolId=101&sessionId=2
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<HolidayDtlResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<HolidayDtlResponse>>> GetAll(
        [FromQuery] long? schoolId,
        [FromQuery] long? sessionId,
        CancellationToken cancellationToken)
    {
        var result = await _holidayService.GetAllAsync(
            schoolId,
            sessionId,
            cancellationToken);

        return Ok(result);
    }

    // GET: api/holidays/15
    [HttpGet("{holidayId:long}")]
    [ProducesResponseType(typeof(HolidayDtlResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HolidayDtlResponse>> GetById(
        long holidayId,
        CancellationToken cancellationToken)
    {
        var result = await _holidayService.GetByIdAsync(
            holidayId,
            cancellationToken);

        return Ok(result);
    }

    // PUT: api/holidays/15
    [HttpPut("{holidayId:long}")]
    [ProducesResponseType(typeof(HolidayDtlResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<HolidayDtlResponse>> Update(
        long holidayId,
        [FromBody] UpdateHolidayDtlRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _holidayService.UpdateAsync(
            holidayId,
            request,
            cancellationToken);

        return Ok(result);
    }

    // DELETE: api/holidays/15
    [HttpDelete("{holidayId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        long holidayId,
        CancellationToken cancellationToken)
    {
        await _holidayService.DeleteAsync(
            holidayId,
            cancellationToken);

        return NoContent();
    }
}
