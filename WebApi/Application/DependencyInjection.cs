using Microsoft.Extensions.DependencyInjection;
using SMS.Application.Services;
using SMS.Application.Validators;
using FluentValidation;

namespace SMS.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<SchoolService>();
        services.AddValidatorsFromAssemblyContaining<CreateSchoolMstRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateSchoolMstRequestValidator>();
        services.AddScoped<SessionService>();
        services.AddValidatorsFromAssemblyContaining<CreateSessionMstRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateSessionMstRequestValidator>();
        services.AddScoped<TeacherService>();
        services.AddValidatorsFromAssemblyContaining<CreateTeacherMstRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateTeacherMstRequestValidator>();
        services.AddScoped<ClassService>();
        services.AddValidatorsFromAssemblyContaining<CreateClassMstRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateClassMstRequestValidator>();
        return services;
    }
}
