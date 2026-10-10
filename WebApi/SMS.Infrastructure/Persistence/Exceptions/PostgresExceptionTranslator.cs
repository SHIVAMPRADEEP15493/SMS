using Npgsql;
using SMS.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Infrastructure.Persistence.Exceptions;
internal static class PostgresExceptionTranslator
{
    public static Exception Translate(
        PostgresException exception)
    {
        return exception.SqlState switch
        {
            PostgresErrorCodes.UniqueViolation =>
                TranslateUniqueViolation(exception),

            PostgresErrorCodes.ForeignKeyViolation =>
                TranslateForeignKeyViolation(exception),

            PostgresErrorCodes.SerializationFailure =>
                new ConflictException(
                    "The operation could not be completed because the data was modified concurrently. Please retry the operation."),

            _ => exception
        };
    }

    private static Exception TranslateUniqueViolation(
        PostgresException exception)
    {
        return exception.ConstraintName switch
        {
            "ux_teacher_school_teacher_code" =>
                new ConflictException(
                    "Teacher code already exists for this school."),

            "ux_teacher_school_employee_code" =>
                new ConflictException(
                    "Employee code already exists for this school."),

            "UX_Class_Code" =>
                new ConflictException(
                    "Class code already exists."),

            "UX_Class_Name" =>
                new ConflictException(
                    "Class name already exists."),

            "UX_Class_DisplayOrder" =>
                new ConflictException(
                    "Display order already exists."),

            _ =>
                new ConflictException(
                    "A record with the same unique value already exists.")
        };
    }

    private static Exception TranslateForeignKeyViolation(
        PostgresException exception)
    {
        return exception.ConstraintName switch
        {
            "fk_teacher_school" =>
                new ConflictException(
                    "The specified school cannot be used because it does not exist."),

            _ =>
                new ConflictException(
                    "The operation violates a data relationship constraint.")
        };
    }
}
