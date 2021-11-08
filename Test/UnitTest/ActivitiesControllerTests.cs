﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Services;
using OngProject.Core.Services.AWS;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Helper;

namespace Test.UnitTest
{
    [TestClass]
    public class ActivitiesControllerTest : TestsContext
    {
        private ApplicationDbContext _context;
        private IUnitOfWork unitOfWork;
        private IImageService imageService;
        private IActivitiesServices activitiesServices;
        [TestInitialize]
        public void Init()
        {
            _context = GetTestContext(Guid.NewGuid().ToString());
            unitOfWork = new UnitOfWork(_context);
            imageService = new ImageService();
            SeedActivities(_context);
            activitiesServices = new ActivitiesServices(unitOfWork, imageService);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturn_All_Activities()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new ActivitiesController(activitiesServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.GetAll();
            var expected = response;

            Assert.AreEqual(typeof(List<ActivitiesDTO>), expected.GetType());
            Dispose();
        }

        [TestMethod]
        public async Task GetById_ShouldReturn_OkObject()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new ActivitiesController(activitiesServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Get(1);
            var expected = response as ObjectResult;

            Assert.AreEqual(typeof(OkObjectResult), expected.GetType());
            Dispose();
        }

        [TestMethod]
        public async Task GetById_ShouldReturn_Ok()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new ActivitiesController(activitiesServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Get(0);
            var castedExpected = response as NotFoundResult;

            //Assert
            Xunit.Assert.NotNull(castedExpected);
            Xunit.Assert.Equal(404, castedExpected.StatusCode);
            Dispose();
        }

        [TestMethod]
        public async Task Update_ShouldReturn_Bad_Request()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new ActivitiesController(activitiesServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Update(new ActivityUpdateDTO
            {
                Content = "InsertTest",
                Name = "InsertTest"
            }, 0);
            var castedExpected = response.Result as BadRequestObjectResult;

            //Assert
            Xunit.Assert.NotNull(castedExpected);
            Xunit.Assert.Equal(400, castedExpected.StatusCode);
            Dispose();
        }

        [TestMethod]
        public async Task Update_ShouldReturn_Ok()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new ActivitiesController(activitiesServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Update(new ActivityUpdateDTO
            {
                Content = "InsertTest",
                Name = "InsertTest"
            }, 1);
            var castedExpected = response.Result as OkObjectResult;

            //Assert
            Xunit.Assert.NotNull(castedExpected);
            Xunit.Assert.Equal(200, castedExpected.StatusCode);
            Dispose();
        }

        [TestMethod]
        public async Task Insert_ShouldReturn_Ok()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new ActivitiesController(activitiesServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Insert(new ActivitiyInsertDTO
            {
                Content = "InsertTest",
                Name = "InsertTest"
            });
            var expected = response as OkObjectResult;

            //Assert
            Assert.AreEqual(typeof(OkObjectResult), expected.GetType());
            Dispose();
        }

        public void SeedActivities(ApplicationDbContext context)
        {
            for (int i = 1; i < 11; i++)
            {
                var contact = new Activities()
                {
                    Id = i,
                    Name = "Activity " + i,
                    Image = "ImageActivities" + i + ".jpg",
                    Content = "Content from activity " + i,
                    CreatedAt = DateTime.Now
                };
                context.AddAsync(contact);
            }
            context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
