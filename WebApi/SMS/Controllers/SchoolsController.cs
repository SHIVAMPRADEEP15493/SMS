using Microsoft.AspNetCore.Mvc;
using SMS.Application.Services;
using SMS.Application.DTOs;

namespace SMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SchoolsController : ControllerBase
{
    private readonly SchoolService _schoolService;
    public SchoolsController( SchoolService schoolService)
    {
        _schoolService = schoolService;
    }

    [HttpPost]
    public async Task<IActionResult> Create( CreateSchoolMstRequest request, CancellationToken cancellationToken)
    {
        var result = await _schoolService.CreateAsync( request, cancellationToken);
        return CreatedAtAction( nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
    {
        var result = await _schoolService.GetAllAsync( cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById( long id, CancellationToken cancellationToken)
    {
        var result = await _schoolService.GetByIdAsync( id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update( long id, UpdateSchoolMstRequest request, CancellationToken cancellationToken)
    {
        var result = await _schoolService.UpdateAsync( id, request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:long}/activate")]
    public async Task<IActionResult> Activate( long id, CancellationToken cancellationToken)
    {
        var result = await _schoolService.ActivateAsync( id, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:long}/deactivate")]
    public async Task<IActionResult> Deactivate( long id, CancellationToken cancellationToken)
    {
        var result = await _schoolService.DeactivateAsync( id, cancellationToken);
        return Ok(result);
    }
}

