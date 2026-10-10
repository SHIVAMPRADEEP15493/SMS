using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Application.IServices;
public interface IClassService
{
    Task<ClassMstResponse> CreateAsync(
        CreateClassMstRequest request,
        CancellationToken cancellationToken = default);
    Task<ClassMstResponse> GetByIdAsync(
        long classId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ClassMstResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task<ClassMstResponse> UpdateAsync(
        long classId,
        UpdateClassMstRequest request,
        CancellationToken cancellationToken = default);
    Task DeleteAsync(
        long classId,
        CancellationToken cancellationToken = default);
    Task ActivateAsync(
        long classId,
        CancellationToken cancellationToken = default);
    Task DeactivateAsync(
        long classId,
        CancellationToken cancellationToken = default);
}
