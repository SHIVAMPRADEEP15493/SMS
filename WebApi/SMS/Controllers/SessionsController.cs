using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Services;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;

namespace SMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class SessionsController : ControllerBase
{
    private readonly SessionService _sessionService;
    public SessionsController(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SessionMstResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SessionMstResponse>> Create([FromBody] CreateSessionMstRequest request, CancellationToken cancellationToken)
    {
        var result = await _sessionService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.SessionId }, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SessionMstResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SessionMstResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(SessionMstResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionMstResponse>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("current")]
    [ProducesResponseType(typeof(SessionMstResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionMstResponse>> GetCurrent(CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetCurrentAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(SessionMstResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionMstResponse>> Update(long id, [FromBody] UpdateSessionMstRequest request, CancellationToken cancellationToken)
    {
        var result = await _sessionService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:long}/activate")]
    [ProducesResponseType(typeof(SessionMstResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionMstResponse>> Activate(long id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.ActivateAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:long}/deactivate")]
    [ProducesResponseType(typeof(SessionMstResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SessionMstResponse>> Deactivate(long id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.DeactivateAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:long}/set-current")]
    [ProducesResponseType(typeof(SessionMstResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SessionMstResponse>> SetCurrent(long id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.SetCurrentAsync(id, cancellationToken);
        return Ok(result);
    }
}
