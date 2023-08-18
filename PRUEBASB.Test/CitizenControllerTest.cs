using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PRUEBASB.Api.Controllers;
using PRUEBASB.Application.Interface;
using PRUEBASB.Application.Validator;
using PRUEBASB.Application.ViewModel;
using PRUEBASB.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PRUEBASB.Test
{
    public class CitizenControllerTest
    {
        private readonly Mock<IPruebaSBRepository> _repositoryMock;
        private readonly Mock<IPruebaSBService> _serviceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<CitizenCreateDto>> _validatorCreateMock;
        private readonly Mock<IValidator<CitizenUpdateDto>> _validatorUpdateMock;
        private readonly Mock<IValidator<string>> _validatorCINMock;
        private readonly CitizenController _controller;

        public CitizenControllerTest()
        {
            _repositoryMock = new Mock<IPruebaSBRepository>();
            _serviceMock = new Mock<IPruebaSBService>();
            _mapperMock = new Mock<IMapper>();
            _validatorCreateMock = new Mock<IValidator<CitizenCreateDto>>();
            _validatorUpdateMock = new Mock<IValidator<CitizenUpdateDto>>();
            _validatorCINMock = new Mock<IValidator<string>>();
            _controller = new CitizenController(
                _serviceMock.Object,
                _mapperMock.Object,
                _validatorCreateMock.Object,
                _validatorUpdateMock.Object,
                _validatorCINMock.Object);
        }

        [Fact]
        public async Task GetAllCitizen_ReturnsOkResultWithPagedData()
        {
            //Arrange
            var testData = new List<CitizenVM>
            {
                new CitizenVM { CIN = "40213467912", Name = "Pablo", LastName = "Perez", Age = 20, Gender = "M"},
            };

            var expectedPagedResult = new PagedResponse<CitizenVM>
            {
                Count = testData.Count,
                TotalPage = 1,
                HasNext = false,
                HasPrevious = false,
                Data = testData
            };

            _serviceMock.Setup(service => service.GetPagedCitizen(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expectedPagedResult);

            // Act
            var result = await _controller.GetAllCitizen(page: 1, pageSize: 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualPagedResult = Assert.IsType<PagedResponse<CitizenVM>>(okResult.Value);

            Assert.Equal(expectedPagedResult.Count, actualPagedResult.Count);
            Assert.Equal(expectedPagedResult.TotalPage, actualPagedResult.TotalPage);
            Assert.Equal(expectedPagedResult.HasNext, actualPagedResult.HasNext);
            Assert.Equal(expectedPagedResult.HasPrevious, actualPagedResult.HasPrevious);
            Assert.Equal(expectedPagedResult.Data.Count(), actualPagedResult?.Data?.Count());
        }

        [Fact]
        public async Task GetCitizen_ValidCIN_ExistingCitizen_ReturnsOkResultWithData()
        {
            // Arrange
            string validCIN = "73223202972";

            var expectedCitizen = new Citizen
            {
                CIN = "73223202972",
                Name = "Anitra",
                LastName = "Wellen",
                Age = 45,
                Gender = "M",
                Status = true,
                DtCreate = DateTime.Today,
                DtEdit = DateTime.Today
            };

            var citizenVM = new CitizenVM
            {
                CIN = "73223202972",
                Name = "Anitra",
                LastName = "Wellen",
                Age = 45,
                Gender = "M",
                Status = true,
            };

            var validationResult = new FluentValidation.Results.ValidationResult();

            _validatorCINMock.Setup(validator => validator.ValidateAsync(validCIN, default))
                             .ReturnsAsync(validationResult);

            var successResponse = new SuccessResponse(true, citizenVM, HttpStatusCode.OK, null);

            _serviceMock.Setup(service => service.CitizenExist(validCIN))
                        .ReturnsAsync(true);

            _serviceMock.Setup(service => service.GetCitizen(validCIN))
                .ReturnsAsync(successResponse);

            _repositoryMock.Setup(r => r.GetCitizen(validCIN))
                .ReturnsAsync(expectedCitizen);

            _mapperMock.Setup(m => m.Map<Citizen, CitizenVM>(expectedCitizen));



            //Act
            var result = await _controller.GetCitizen(validCIN);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var successReturn = Assert.IsType<SuccessResponse>(okResult.Value);

            Assert.True(successReturn.IsSuccess);
            Assert.NotNull(successReturn.Data);
            Assert.Equal(HttpStatusCode.OK, successReturn.StatusCode);
            Assert.Null(successReturn.Errors);
        }

        [Fact]
        public async Task GetCitizen_ValidCIN_NonExistingCitizen_ReturnsOkResultWithData()
        {
            // Arrange
            string validCIN = "00000000001";

            _serviceMock.Setup(service => service.CitizenExist(validCIN))
                        .ReturnsAsync(false);

            var validationResult = new FluentValidation.Results.ValidationResult();

            _validatorCINMock.Setup(validator => validator.ValidateAsync(validCIN, default))
                             .ReturnsAsync(validationResult);

            //Act
            var result = await _controller.GetCitizen(validCIN);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequest.Value);

            Assert.False(errorResponse.IsSuccess);
            Assert.Equal("CIN already doesn't exist", errorResponse.Error);
        }

        [Fact]
        public async Task GetCitizen_InvalidCIN_ReturnsBadRequest()
        {
            // Arrange
            string invalidCIN = "INVALID";

            // Validation failure with a specific error message
            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("cin", "CIN must be a valid positive integer.")
            };

            var validationResult = new FluentValidation.Results.ValidationResult(validationErrors);

            _validatorCINMock.Setup(validator => validator.ValidateAsync(invalidCIN, default))
                             .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.GetCitizen(invalidCIN);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);

            Assert.False(errorResponse.IsSuccess);
            Assert.Single(errorResponse.Errors);
        }
    }
}
