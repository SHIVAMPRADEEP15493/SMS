using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using SMS.Application.Common.Exceptions;
using SMS.Application.SchoolsMst.DTOs;
using SMS.Application.SchoolsMst.Interfaces;
using SMS.Application.SchoolsMst.Services;
using SMS.Domain.Entities;

namespace UnitTests.Schools;
public class SchoolServiceTests
{
    private readonly Mock<ISchoolRepository> _repositoryMock;
    private readonly SchoolService _service;
    public SchoolServiceTests()
    {
        _repositoryMock = new Mock<ISchoolRepository>();
        _service = new SchoolService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldCreateSchool()
    {
        // Arrange
        var request = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "Delhi Public School",
            SchoolAddress = "Sector 12",
            City = "Noida",
            State = "Uttar Pradesh",
            Country = "India",
            PinCode = "201301",
            PhoneNumber = "9876543210",
            Email = "school@example.com"
        };

        _repositoryMock
            .Setup(x => x.ExistsByCodeAsync( request.SchoolCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        SMS_School_Mst? capturedSchool = null;

        _repositoryMock
            .Setup(x => x.AddAsync( It.IsAny<SMS_School_Mst>(), It.IsAny<CancellationToken>()))
            .Callback<SMS_School_Mst, CancellationToken>( (school, _) => capturedSchool = school);

        // Act
        var result = await _service.CreateAsync( request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal( request.SchoolCode, result.SchoolCode);
        Assert.Equal( request.SchoolName, result.SchoolName);
        Assert.Equal( request.SchoolAddress, result.SchoolAddress);
        Assert.Equal( request.City, result.City);
        Assert.Equal( request.State, result.State);
        Assert.Equal( request.Country, result.Country);
        Assert.Equal( request.PinCode, result.PinCode);
        Assert.Equal( request.PhoneNumber, result.PhoneNumber);
        Assert.Equal( request.Email, result.Email);
        Assert.True( result.IsActive);
        Assert.NotNull(capturedSchool);

        _repositoryMock.Verify(
            x => x.AddAsync( It.IsAny<SMS_School_Mst>(), It.IsAny<CancellationToken>()), Times.Once);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync( It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenCodeAlreadyExists_ShouldThrowConflictException()
    {
        // Arrange
        var request = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "Delhi Public School"
        };

        _repositoryMock
            .Setup(x => x.ExistsByCodeAsync( request.SchoolCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var exception =
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.CreateAsync( request, CancellationToken.None));

        // Assert
        Assert.Contains( request.SchoolCode, exception.Message);
        _repositoryMock.Verify(
            x => x.AddAsync( It.IsAny<SMS_School_Mst>(), It.IsAny<CancellationToken>()), Times.Never);
        _repositoryMock.Verify(
            x => x.SaveChangesAsync( It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSchoolExists_ShouldReturnSchool()
    {
        // Arrange
        var school = new SMS_School_Mst(
            "SCH001",
            "Delhi Public School",
            "Sector 12",
            "Noida",
            "Uttar Pradesh",
            "India",
            "201301",
            "9876543210",
            "school@example.com");

        _repositoryMock
            .Setup(x => x.GetByIdAsync( school.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(school);

        // Act
        var result = await _service.GetByIdAsync( school.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal( school.Id, result.Id);
        Assert.Equal( "SCH001", result.SchoolCode);
        Assert.Equal( "Delhi Public School", result.SchoolName);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSchoolDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var schoolId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(  schoolId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SMS_School_Mst?)null);

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByIdAsync( schoolId, CancellationToken.None));

        Assert.Contains( schoolId.ToString(), exception.Message);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSchools()
    {
        // Arrange
        var schools = new List<SMS_School_Mst>
        {
            new SMS_School_Mst(
                "SCH001",
                "Delhi Public School",
                null,
                "Noida",
                "Uttar Pradesh",
                "India",
                null,
                null,
                null),

            new SMS_School_Mst(
                "SCH002",
                "ABC Public School",
                null,
                "Delhi",
                "Delhi",
                "India",
                null,
                null,
                null)
        };

        _repositoryMock
            .Setup(x => x.GetAllAsync( It.IsAny<CancellationToken>()))
            .ReturnsAsync(schools);

        // Act
        var result = await _service.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains( result, x => x.SchoolCode == "SCH001");
        Assert.Contains( result, x => x.SchoolCode == "SCH002");
    }

    [Fact]
    public async Task UpdateAsync_WhenSchoolExists_ShouldUpdateSchool()
    {
        // Arrange
        var school = new SMS_School_Mst(
            "SCH001",
            "Old School Name",
            "Old Address",
            "Noida",
            "Uttar Pradesh",
            "India",
            "201301",
            "9876543210",
            "old@example.com");

        var request = new UpdateSchoolMstRequest
        {
            SchoolName = "New School Name",
            SchoolAddress = "New Address",
            City = "Delhi",
            State = "Delhi",
            Country = "India",
            PinCode = "110001",
            PhoneNumber = "9999999999",
            Email = "new@example.com"
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync( school.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(school);

        // Act
        var result = await _service.UpdateAsync( school.Id, request, CancellationToken.None);

        // Assert
        Assert.Equal( "SCH001", result.SchoolCode);
        Assert.Equal( "New School Name", result.SchoolName);
        Assert.Equal( "New Address", result.SchoolAddress);
        Assert.Equal( "Delhi", result.City);
        Assert.Equal( "new@example.com", result.Email);
        _repositoryMock.Verify( x => x.Update(school), Times.Once);
        _repositoryMock.Verify(x => x.SaveChangesAsync( It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenSchoolDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var schoolId = Guid.NewGuid();

        var request = new UpdateSchoolMstRequest
        {
            SchoolName = "Updated School"
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync( schoolId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SMS_School_Mst?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.UpdateAsync(schoolId, request, CancellationToken.None));

        _repositoryMock.Verify(
            x => x.Update( It.IsAny<SMS_School_Mst>()), Times.Never);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync( It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeactivateAsync_WhenSchoolExists_ShouldDeactivateSchool()
    {
        // Arrange
        var school = new SMS_School_Mst(
            "SCH001",
            "Delhi Public School",
            null,
            "Noida",
            "Uttar Pradesh",
            "India",
            null,
            null,
            null);

        _repositoryMock
            .Setup(x => x.GetByIdAsync( school.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(school);

        // Act
        var result = await _service.DeactivateAsync( school.Id, CancellationToken.None);

        // Assert
        Assert.False(result.IsActive);
        _repositoryMock.Verify( x => x.Update(school), Times.Once);
        _repositoryMock.Verify( x => x.SaveChangesAsync( It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ActivateAsync_WhenSchoolExists_ShouldActivateSchool()
    {
        // Arrange
        var school = new SMS_School_Mst(
            "SCH001",
            "Delhi Public School",
            null,
            "Noida",
            "Uttar Pradesh",
            "India",
            null,
            null,
            null);

        school.Deactivate();

        _repositoryMock
            .Setup(x => x.GetByIdAsync( school.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(school);

        // Act
        var result = await _service.ActivateAsync( school.Id, CancellationToken.None);

        // Assert
        Assert.True(result.IsActive);
        _repositoryMock.Verify( x => x.Update(school), Times.Once);
        _repositoryMock.Verify( x => x.SaveChangesAsync( It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeactivateAsync_WhenSchoolDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var schoolId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync( schoolId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SMS_School_Mst?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.DeactivateAsync( schoolId, CancellationToken.None));
        _repositoryMock.Verify( x => x.SaveChangesAsync( It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ActivateAsync_WhenSchoolDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var schoolId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync( schoolId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SMS_School_Mst?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.ActivateAsync( schoolId, CancellationToken.None));
        _repositoryMock.Verify( x => x.SaveChangesAsync( It.IsAny<CancellationToken>()), Times.Never);
    }
}
