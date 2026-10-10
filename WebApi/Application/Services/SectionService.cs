using SMS.Application.Common.Exceptions;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;
using SMS.Application.Interfaces;
using SMS.Domain.Entities;

namespace SMS.Application.Services;
public sealed class SectionService
{
    /*
    private readonly ISectionRepository _sectionRepository;
    public SectionService(ISectionRepository sectionRepository)
    {
        _sectionRepository = sectionRepository;
    }

    public async Task<SectionMstResponse> CreateAsync(
        CreateSectionMstRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (await _sectionRepository.ExistsByCodeAsync(
                request.SectionCode,
                cancellationToken))
        {
            throw new ConflictException(
                $"Section code '{request.SectionCode}' already exists.");
        }

        if (await _sectionRepository.ExistsByNameAsync(
                request.SectionName,
                cancellationToken))
        {
            throw new ConflictException(
                $"Section name '{request.SectionName}' already exists.");
        }

        if (await _sectionRepository.ExistsByDisplayOrderAsync(
                request.DisplayOrder,
                cancellationToken))
        {
            throw new ConflictException(
                $"Display order '{request.DisplayOrder}' is already in use.");
        }

        var section = new SectionMst(
            request.SectionCode.Trim(),
            request.SectionName.Trim(),
            request.DisplayOrder);

        await _sectionRepository.AddAsync(
            section,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return MapToResponse(section);
    }

    public async Task<SectionMstResponse> GetByIdAsync(
        long sectionId,
        CancellationToken cancellationToken = default)
    {
        var section = await GetSectionOrThrowAsync(
            sectionId,
            cancellationToken);

        return MapToResponse(section);
    }

    public async Task<IReadOnlyList<SectionMstResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var sections = await _sectionRepository.GetAllAsync(
            cancellationToken);

        return sections
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<SectionMstResponse> UpdateAsync(
        long sectionId,
        UpdateSectionMstRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var section = await GetSectionOrThrowAsync(
            sectionId,
            cancellationToken);

        if (await _sectionRepository.ExistsByNameAsync(
                request.SectionName,
                sectionId,
                cancellationToken))
        {
            throw new ConflictException(
                $"Section name '{request.SectionName}' already exists.");
        }

        if (await _sectionRepository.ExistsByDisplayOrderAsync(
                request.DisplayOrder,
                sectionId,
                cancellationToken))
        {
            throw new ConflictException(
                $"Display order '{request.DisplayOrder}' is already in use.");
        }

        section.Update(
            request.SectionName.Trim(),
            request.DisplayOrder);

        await _context.SaveChangesAsync(cancellationToken);

        return MapToResponse(section);
    }

    public async Task ActivateAsync(
        long sectionId,
        CancellationToken cancellationToken = default)
    {
        var section = await GetSectionOrThrowAsync(
            sectionId,
            cancellationToken);

        section.Activate();

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeactivateAsync(
        long sectionId,
        CancellationToken cancellationToken = default)
    {
        var section = await GetSectionOrThrowAsync(
            sectionId,
            cancellationToken);

        section.Deactivate();

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        long sectionId,
        CancellationToken cancellationToken = default)
    {
        var section = await GetSectionOrThrowAsync(
            sectionId,
            cancellationToken);

        _sectionRepository.Remove(section);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<SectionMst> GetSectionOrThrowAsync(
        long sectionId,
        CancellationToken cancellationToken)
    {
        var section = await _sectionRepository.GetByIdAsync(
            sectionId,
            cancellationToken);

        if (section is null)
        {
            throw new NotFoundException(
                $"Section with ID '{sectionId}' was not found.", sectionId);
        }

        return section;
    }

    private static SectionMstResponse MapToResponse(
        SectionMst section)
    {
        return new SectionMstResponse
        {
            SectionId = section.SectionId,
            SectionCode = section.SectionCode,
            SectionName = section.SectionName,
            DisplayOrder = section.DisplayOrder,
            IsActive = section.IsActive,
            CreatedAt = section.CreatedAt,
            CreatedBy = section.CreatedBy,
            UpdatedAt = section.UpdatedAt,
            UpdatedBy = section.UpdatedBy
        };
    }
    */
    private readonly ISectionRepository _sectionRepository;
    public SectionService(ISectionRepository sectionRepository)
    {
        _sectionRepository = sectionRepository;
    }

    public async Task<SectionMstResponse> CreateAsync(
        CreateSectionMstRequest request,
        CancellationToken cancellationToken = default)
    {
        var sectionCode = request.SectionCode.Trim();
        var sectionName = request.SectionName.Trim();

        if (await _sectionRepository.ExistsByCodeAsync(
                sectionCode,
                cancellationToken))
        {
            throw new ConflictException(
                $"Section code '{sectionCode}' already exists.");
        }

        if (await _sectionRepository.ExistsByNameAsync(
                sectionName,
                cancellationToken))
        {
            throw new ConflictException(
                $"Section name '{sectionName}' already exists.");
        }

        if (await _sectionRepository.ExistsByDisplayOrderAsync(
                request.DisplayOrder,
                cancellationToken))
        {
            throw new ConflictException(
                $"Display order '{request.DisplayOrder}' already exists.");
        }

        var section = new SectionMst(
            sectionCode,
            sectionName,
            request.DisplayOrder);

        await _sectionRepository.AddAsync(
            section,
            cancellationToken);

        return MapToResponse(section);
    }

    public async Task<SectionMstResponse> GetByIdAsync(
        long sectionId,
        CancellationToken cancellationToken = default)
    {
        var section = await _sectionRepository.GetByIdAsync(
            sectionId,
            cancellationToken);

        if (section is null)
        {
            throw new NotFoundException(
                $"Section with ID '{sectionId}' was not found.", sectionId);
        }

        return MapToResponse(section);
    }

    public async Task<IReadOnlyList<SectionMstResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var sections = await _sectionRepository.GetAllAsync(
            cancellationToken);

        return sections
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<SectionMstResponse> UpdateAsync(
        long sectionId,
        UpdateSectionMstRequest request,
        CancellationToken cancellationToken = default)
    {
        var section = await _sectionRepository.GetByIdAsync(
            sectionId,
            cancellationToken);

        if (section is null)
        {
            throw new NotFoundException(
                $"Section with ID '{sectionId}' was not found.", sectionId);
        }

        var sectionName = request.SectionName.Trim();

        if (await _sectionRepository.ExistsByNameAsync(
                sectionName,
                sectionId,
                cancellationToken))
        {
            throw new ConflictException(
                $"Section name '{sectionName}' already exists.");
        }

        if (await _sectionRepository.ExistsByDisplayOrderAsync(
                request.DisplayOrder,
                sectionId,
                cancellationToken))
        {
            throw new ConflictException(
                $"Display order '{request.DisplayOrder}' already exists.");
        }

        section.Update(
            sectionName,
            request.DisplayOrder);

        return MapToResponse(section);
    }

    public async Task DeleteAsync(
        long sectionId,
        CancellationToken cancellationToken = default)
    {
        var section = await _sectionRepository.GetByIdAsync(
            sectionId,
            cancellationToken);

        if (section is null)
        {
            throw new NotFoundException(
                $"Section with ID '{sectionId}' was not found.", sectionId);
        }

        await _sectionRepository.RemoveAsync(section, cancellationToken);
    }

    private static SectionMstResponse MapToResponse(
        SectionMst section)
    {
        return new SectionMstResponse
        {
            SectionId = section.SectionId,
            SectionCode = section.SectionCode,
            SectionName = section.SectionName,
            DisplayOrder = section.DisplayOrder,
            IsActive = section.IsActive,
            CreatedAt = section.CreatedAt,
            CreatedBy = section.CreatedBy,
            UpdatedAt = section.UpdatedAt,
            UpdatedBy = section.UpdatedBy
        };
    }
}
