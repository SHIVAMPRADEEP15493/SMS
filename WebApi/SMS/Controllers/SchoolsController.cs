using Microsoft.AspNetCore.Mvc;
using SMS.Application.SchoolsMst.Services;
using SMS.Application.SchoolsMst.DTOs;

namespace SMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolsController : ControllerBase
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById( Guid id, CancellationToken cancellationToken)
    {
        var result = await _schoolService.GetByIdAsync( id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update( Guid id, UpdateSchoolMstRequest request, CancellationToken cancellationToken)
    {
        var result = await _schoolService.UpdateAsync( id, request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate( Guid id, CancellationToken cancellationToken)
    {
        var result = await _schoolService.ActivateAsync( id, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate( Guid id, CancellationToken cancellationToken)
    {
        var result = await _schoolService.DeactivateAsync( id, cancellationToken);
        return Ok(result);
    }
}

