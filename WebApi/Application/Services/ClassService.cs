using SMS.Application.Common.Exceptions;
using SMS.Application.IServices;
using SMS.Domain.Entities;
using SMS.Application.Interfaces;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;

namespace SMS.Application.Services;
public sealed class ClassService : IClassService
{
    private readonly IClassRepository _repository;
    public ClassService(
        IClassRepository repository)
    {
        _repository = repository;
    }
    public async Task<ClassMstResponse> CreateAsync(
        CreateClassMstRequest request,
        CancellationToken cancellationToken = default)
    {
        var classCode = request.ClassCode.Trim();
        var className = request.ClassName.Trim();

        if (await _repository.ExistsByCodeAsync(
                classCode,
                cancellationToken))
        {
            throw new ConflictException(
                "Class code already exists.");
        }

        if (await _repository.ExistsByNameAsync(
                className,
                cancellationToken))
        {
            throw new ConflictException(
                "Class name already exists.");
        }

        if (await _repository.ExistsByDisplayOrderAsync(
                request.DisplayOrder,
                cancellationToken))
        {
            throw new ConflictException(
                "Display order already exists.");
        }

        var classMst = new ClassMst(
            classCode,
            className,
            request.DisplayOrder);

        await _repository.AddAsync(
            classMst,
            cancellationToken);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(classMst);
    }
    public async Task<ClassMstResponse> GetByIdAsync(
        long classId,
        CancellationToken cancellationToken = default)
    {
        var classMst =
            await _repository.GetByIdAsync(
                classId,
                cancellationToken);

        if (classMst is null)
        {
            throw new NotFoundException(
                "Class",
                classId);
        }

        return MapToResponse(classMst);
    }
    public async Task<IReadOnlyList<ClassMstResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var classes =
            await _repository.GetAllAsync(
                cancellationToken);

        return classes
            .Select(MapToResponse)
            .ToList();
    }
    public async Task<ClassMstResponse> UpdateAsync(
        long classId,
        UpdateClassMstRequest request,
        CancellationToken cancellationToken = default)
    {
        var classMst =
            await _repository.GetByIdAsync(
                classId,
                cancellationToken);

        if (classMst is null)
        {
            throw new NotFoundException(
                "Class",
                classId);
        }

        var className = request.ClassName.Trim();

        if (await _repository.ExistsByNameAsync(
                className,
                cancellationToken))
        {
            var existingClass =
                await _repository.GetByCodeAsync(
                    classMst.ClassCode,
                    cancellationToken);

            if (existingClass is not null &&
                existingClass.ClassId != classId)
            {
                throw new ConflictException(
                    "Class name already exists.");
            }
        }

        if (await _repository.ExistsByDisplayOrderAsync(
                request.DisplayOrder,
                cancellationToken))
        {
            if (classMst.DisplayOrder !=
                request.DisplayOrder)
            {
                throw new ConflictException(
                    "Display order already exists.");
            }
        }

        classMst.Update(
            className,
            request.DisplayOrder);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(classMst);
    }
    public async Task DeleteAsync(
        long classId,
        CancellationToken cancellationToken = default)
    {
        var classMst =
            await _repository.GetByIdAsync(
                classId,
                cancellationToken);

        if (classMst is null)
        {
            throw new NotFoundException(
                "Class",
                classId);
        }

        _repository.Remove(classMst);

        await _repository.SaveChangesAsync(
            cancellationToken);
    }
    public async Task ActivateAsync(
        long classId,
        CancellationToken cancellationToken = default)
    {
        var classMst =
            await _repository.GetByIdAsync(
                classId,
                cancellationToken);

        if (classMst is null)
        {
            throw new NotFoundException(
                "Class",
                classId);
        }

        classMst.Activate();

        await _repository.SaveChangesAsync(
            cancellationToken);
    }
    public async Task DeactivateAsync(
        long classId,
        CancellationToken cancellationToken = default)
    {
        var classMst =
            await _repository.GetByIdAsync(
                classId,
                cancellationToken);

        if (classMst is null)
        {
            throw new NotFoundException(
                "Class",
                classId);
        }

        classMst.Deactivate();

        await _repository.SaveChangesAsync(
            cancellationToken);
    }
    private static ClassMstResponse MapToResponse(
        ClassMst classMst)
    {
        return new ClassMstResponse(
            classMst.ClassId,
            classMst.ClassCode,
            classMst.ClassName,
            classMst.DisplayOrder,
            classMst.IsActive,
            classMst.CreatedAt,
            classMst.CreatedBy,
            classMst.UpdatedAt,
            classMst.UpdatedBy);
    }
}
