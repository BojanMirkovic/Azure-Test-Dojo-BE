﻿using Application_Layer.Commands.ModuleCommands.CreateModule;
using Application_Layer.DTO_s.Module;
using AutoMapper;
using Domain_Layer.Models.ModulModel;
using FakeItEasy;
using Infrastructure_Layer.Repositories.Module;

namespace Test_Layer.ModuleTest.UnitTest.ModuleCommandTest
{
    [TestFixture]
    public class CreateModuleCommandHandlerTest
    {
        private IModuleRepository _moduleRepository;
        private IMapper _mapper;
        private CreateModuleCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _moduleRepository = A.Fake<IModuleRepository>();
            _mapper = A.Fake<IMapper>();
            _handler = new CreateModuleCommandHandler(_moduleRepository, _mapper);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccessResult_WhenModuleIsSuccessfullyCreated()
        {
            // Arrange
            var moduleDTO = new CreateModuleDTO
            {
                CourseId = "09a09211-1ec0-4626-8c4c-2878472c5859",
                ModulTitle = "Introduction to Testing",
                Description = "A basic module on testing",
                OrderInCourse = 1
            };
            var modulModel = new ModulModel();
            A.CallTo(() => _mapper.Map<ModulModel>(moduleDTO)).Returns(modulModel);
            A.CallTo(() => _moduleRepository.CreateModuleAsync(modulModel)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(new CreateModuleCommand(moduleDTO), default);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.That(result.Message, Is.EqualTo("Module successfully created"));
        }

        [Test]
        public async Task Handle_ShouldReturnFailureResult_WhenModuleCreationFails()
        {
            // Arrange
            var moduleDTO = new CreateModuleDTO
            {
                CourseId = "a5de4c71-c224-4257-b19b-eb47d55e4575",
                ModulTitle = "Faulty Module",
                Description = "This module should fail",
                OrderInCourse = -1
            };
            var modulModel = new ModulModel();
            A.CallTo(() => _mapper.Map<ModulModel>(moduleDTO)).Returns(modulModel);
            A.CallTo(() => _moduleRepository.CreateModuleAsync(modulModel)).Throws(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(new CreateModuleCommand(moduleDTO), default);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.Message, Is.EqualTo("An error occurred: Database error"));
        }

        [Test]
        public async Task Handle_ShouldReturnFailureResult_WhenExceptionIsThrown()
        {
            // Arrange
            var moduleDTO = new CreateModuleDTO();
            A.CallTo(() => _mapper.Map<ModulModel>(A<CreateModuleDTO>.Ignored)).Throws(new Exception("Mapping failed"));

            // Act
            var result = await _handler.Handle(new CreateModuleCommand(moduleDTO), default);

            // Assert
            Assert.IsFalse(result.Success);
            StringAssert.Contains("An error occurred: Mapping failed", result.Message);
        }
    }
}
