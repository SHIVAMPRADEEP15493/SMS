using SMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.TestData;
public static class ClassTestData
{
    public static CreateClassMstRequest CreateRequest(
        string classCode = "CLASS-01",
        string className = "Class 1",
        int displayOrder = 1)
    {
        return new CreateClassMstRequest(
            classCode,
            className,
            displayOrder);
    }
    public static UpdateClassMstRequest UpdateRequest(
        string className = "Updated Class 1",
        int displayOrder = 10)
    {
        return new UpdateClassMstRequest(
            className,
            displayOrder);
    }
}
