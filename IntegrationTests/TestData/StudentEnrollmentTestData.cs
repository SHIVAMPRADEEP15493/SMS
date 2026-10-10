using SMS.Application.Features.StudentEnrollments.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.TestData;
public static class StudentEnrollmentTestData
{
    public static CreateStudentEnrollmentRequest CreateRequest(
        long studentId = 1001,
        long schoolId = 1,
        long sessionId = 2,
        long classId = 5,
        long sectionId = 3,
        string rollNumber = "25")
    {
        return new CreateStudentEnrollmentRequest
        {
            StudentId = studentId,
            SchoolId = schoolId,
            SessionId = sessionId,
            ClassId = classId,
            SectionId = sectionId,
            AdmissionDate = new DateOnly(2026, 4, 1),
            RollNumber = rollNumber
        };
    }

    public static UpdateStudentEnrollmentRequest UpdateRequest(
        long schoolId = 1,
        long classId = 6,
        long sectionId = 4,
        string rollNumber = "12")
    {
        return new UpdateStudentEnrollmentRequest
        {
            SchoolId = schoolId,
            ClassId = classId,
            SectionId = sectionId,
            RollNumber = rollNumber
        };
    }

    public static UpdateStudentEnrollmentStatusRequest StatusRequest(
        string status)
    {
        return new UpdateStudentEnrollmentStatusRequest
        {
            Status = status
        };
    }
}
