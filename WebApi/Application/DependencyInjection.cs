using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SMS.Application.Features.StudentEnrollments.Services;
using SMS.Application.Services;
using SMS.Application.Validators.CreateValidators;
using SMS.Application.Validators.UpdateValidators;

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
        services.AddScoped<SectionService>();
        services.AddValidatorsFromAssemblyContaining<CreateSectionMstRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateSectionMstRequestValidator>();
        services.AddScoped<StudentService>();
        services.AddValidatorsFromAssemblyContaining<CreateStudentMstRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateStudentMstRequestValidator>();
        //services.AddValidatorsFromAssemblyContaining<UpdateStudentMstStatusRequestValidator>();
        services.AddScoped<StudentEnrollmentService>();
        services.AddValidatorsFromAssemblyContaining<CreateStudentEnrollmentRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateStudentEnrollmentRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateStudentEnrollmentStatusRequestValidator>();
        services.AddScoped<AttendanceTypeService>();
        services.AddValidatorsFromAssemblyContaining<CreateAttendanceTypeMstRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateAttendanceTypeMstRequestValidator>();
        services.AddScoped<HolidayService>();
        services.AddValidatorsFromAssemblyContaining<CreateHolidayDtlRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateHolidayDtlRequestValidator>();

        return services;
    }
}
