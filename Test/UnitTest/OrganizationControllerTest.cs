using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Controllers;
using OngProject.Core.Entities;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Services;
using OngProject.Core.Services.AWS;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Threading.Tasks;
using OngProject.Core.DTOs;
using Test.Helper;

namespace Test.UnitTest
{
    [TestClass]
    public class OrganizationControllerTest : TestsContext
    {
        private ApplicationDbContext _context;
        private IUnitOfWork unitOfWork;
        private IImageService imageService;
        private IOrganizationsServices organizationsServices;
        [TestInitialize]
        public void Init()
        {
            _context = GetTestContext(Guid.NewGuid().ToString());
            unitOfWork = new UnitOfWork(_context);
            imageService = new ImageService();
            organizationsServices = new OrganizationsServices(unitOfWork, imageService);
            SeedOrganizations(_context);

        }
        [TestMethod]
        public async Task GetOrganization_ValidGet_ShouldReturnOk()
        {
            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new OrganizationsController(organizationsServices);
            controller.ControllerContext = controllerContext;

        
            var response = await controller.Get();
            
            var expected = ((OrganizationsDTO)response);
            Assert.AreEqual(typeof(OrganizationsDTO), expected.GetType());
            Dispose();
        }
        [TestMethod]
        public async Task PutOrganization_ValidUpdate_ShouldReturnOk()
        {

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new OrganizationsController(organizationsServices);
            controller.ControllerContext = controllerContext;

            var ongDto = new OrganizationUpdateDTO()
            {
                Id = 1,
                Name = "modificada",
                Image = null,
                Address = "AddressModificada",
                Phone = 381222,
                Email = "EmailModificado",
                WelcomeText = "WelcomeTextModificado",
                AboutUsText = "AboutUsTextModificado",
                FacebookUrl = "FacebookUrlModificado",
                InstagramUrl = "InstagramUrlModificado",
                LinkedinUrl = "InstragramUrlModificado"
            };
            var response = await controller.Update(ongDto);
            var expected = (ObjectResult)response.Result;

           
            Assert.AreEqual(200, expected.StatusCode);
            Dispose();
        }
        [TestMethod]
        public async Task PutOrganization_InvalidUpdate_ShouldReturnError()
        {

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/testPrueba";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            var controller = new OrganizationsController(organizationsServices);
            controller.ControllerContext = controllerContext;

            var ongDto = new OrganizationUpdateDTO()
            {
                Id = 2,
                Name = "modificada",
                Image = null,
                Address = "AddressModificada",
                Phone = 381222,
                Email = "EmailModificado",
                WelcomeText = "WelcomeTextModificado",
                AboutUsText = "AboutUsTextModificado",
                FacebookUrl = "FacebookUrlModificado",
                InstagramUrl = "InstagramUrlModificado",
                LinkedinUrl = "InstragramUrlModificado"
            };
            var response = await controller.Update(ongDto);
            var expected = (ObjectResult)response.Result;

    
            Assert.AreEqual(400, expected.StatusCode);
            Dispose();
        }
        private void SeedOrganizations(ApplicationDbContext dbContext)
        {
            
                var organization = new Organizations
                 {
                     Id = 1,
                     Name = "Organization " + 1,
                     Image = "ImageOrganizations" + 1 + ".jpg",
                     Address = "Address for Organization " + 1,
                     Phone = 381 + 1,
                     Email = "Email for Organization " + 1,
                     WelcomeText = "WelcomeText for Organization " + 1,
                     AboutUsText = "AboutUsText for Organization " + 1,
                     CreatedAt = DateTime.Now,
                     FacebookUrl = "FacebookUrl" + 1,
                     InstagramUrl = "InstagramUrl" + 1,
                     LinkedinUrl = "InstragramUrl" +1
               };
                dbContext.Add(organization);
           
            dbContext.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
