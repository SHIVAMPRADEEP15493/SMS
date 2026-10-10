using Microsoft.EntityFrameworkCore;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<SchoolMst> SchoolsMst => Set<SchoolMst>();
    public DbSet<SessionMst> SessionMst => Set<SessionMst>();
    public DbSet<TeacherMst> TeacherMst => Set<TeacherMst>();
    public DbSet<ClassMst> ClassMst => Set<ClassMst>();
    public DbSet<SectionMst> SectionMst => Set<SectionMst>();
    public DbSet<StudentMst> StudentMst => Set<StudentMst>();
    public DbSet<StudentEnrollmentDtl> StudentEnrollmentDtls => Set<StudentEnrollmentDtl>();
    public DbSet<AttendanceTypeMst> AttendanceTypeMst => Set<AttendanceTypeMst>();
    public DbSet<HolidayDtl> HolidayDtls => Set<HolidayDtl>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
