using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
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
    public class TesimonialsControllerTest : TestsContext
    {
        private ApplicationDbContext _context;
        private IUnitOfWork unitOfWork;
        private IImageService imageService;
        private ITestimonialsServices testimonialsServices;
        private IUriService uriService;
        [TestInitialize]
        public void Init()
        {
            _context = GetTestContext(Guid.NewGuid().ToString());
            unitOfWork = new UnitOfWork(_context);
            imageService = new ImageService();
            SeedTestimonials(_context);
            uriService = new UriService("http://testUriBase");
            testimonialsServices = new TestimonialsServices(unitOfWork, imageService, uriService);
        }

        [TestMethod]
        public async Task GetFirstPage_Should_Return_Ok()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new TestimonialsController(testimonialsServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.GetAll(1);
            var expected = response.Result as ObjectResult;

            Assert.AreEqual(typeof(OkObjectResult), expected.GetType());
            Dispose();
        }


        [TestMethod]
        public async Task Update_Id_0_ShouldReturn_Ok()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new TestimonialsController(testimonialsServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Update(0, new TestimonialsUpdateDTO
            {
                Content = "InsertTest",
                Name = "InsertTest"
            });
            var castedExpected = response.Result as OkObjectResult;

            //Assert
            Xunit.Assert.NotNull(castedExpected);
            Xunit.Assert.Equal(200, castedExpected.StatusCode);
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
            var controller = new TestimonialsController(testimonialsServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Update(0, new TestimonialsUpdateDTO
            {
                Content = "InsertTest",
                Name = "InsertTest"
            });
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
            var controller = new TestimonialsController(testimonialsServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Insert(new TestimonialsCreateDTO
            {
                Content = "InsertTest",
                Name = "InsertTest"
            });
            var expected = response.Result as OkObjectResult;

            //Assert
            Assert.AreEqual(typeof(OkObjectResult), expected.GetType());
            Dispose();
        }

        [TestMethod]
        public async Task Delete_Id_0_ShouldReturn_Ok()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new TestimonialsController(testimonialsServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Delete(0);
            var castedExpected = response.Result as OkObjectResult;

            //Assert
            Xunit.Assert.NotNull(castedExpected);
            Xunit.Assert.Equal(200, castedExpected.StatusCode);
            Dispose();
        }

        [TestMethod]
        public async Task Delete_ShouldReturn_Ok()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new TestimonialsController(testimonialsServices);
            controller.ControllerContext = controllerContext;


            var response = await controller.Delete(1);
            var castedExpected = response.Result as OkObjectResult;

            //Assert
            Xunit.Assert.NotNull(castedExpected);
            Xunit.Assert.Equal(200, castedExpected.StatusCode);
            Dispose();
        }

        public void SeedTestimonials(ApplicationDbContext context)
        {
            for (int i = 1; i < 11; i++)
            {
                var contact = new Testimonials()
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
