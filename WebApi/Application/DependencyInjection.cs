using Microsoft.Extensions.DependencyInjection;
using SMS.Application.SchoolsMst.Services;
using SMS.Application.SchoolsMst.Validators;
using FluentValidation;

namespace SMS.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<SchoolService>();
        services.AddValidatorsFromAssemblyContaining<CreateSchoolRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateSchoolRequestValidator>();
        return services;
    }
}
