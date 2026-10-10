using SMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Update;

namespace IntegrationTests.TestData;
public static class TeacherTestData
{
    public static CreateTeacherMstRequest CreateRequest(
        long schoolId,
        string teacherCode = "TCH001",
        string employeeCode = "EMP001")
    {
        return new CreateTeacherMstRequest
        {
            SchoolId = schoolId,
            TeacherCode = teacherCode,
            EmployeeCode = employeeCode,
            FirstName = "Rahul",
            MiddleName = "Kumar",
            LastName = "Sharma",
            Gender = Gender.Male,
            DateOfBirth = new DateOnly(1990, 5, 15),
            Email = "rahul.sharma@test.com",
            MobileNumber = "9876543210",
            DateOfJoining = new DateOnly(2024, 4, 1),
            Qualification = "M.Sc. Mathematics",
            Designation = "Senior Teacher"
        };
    }

    public static UpdateTeacherMstRequest UpdateRequest()
    {
        return new UpdateTeacherMstRequest
        {
            FirstName = "Amit",
            MiddleName = "Kumar",
            LastName = "Sharma",
            Gender = Gender.Male,
            DateOfBirth = new DateOnly(1991, 6, 10),
            Email = "amit.sharma@test.com",
            MobileNumber = "9876543211",
            DateOfJoining = new DateOnly(2024, 5, 1),
            Qualification = "M.Ed.",
            Designation = "Senior Teacher"
        };
    }
}
