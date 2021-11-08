using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Core.Services;
using OngProject.Core.Services.AWS;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Helper;
using Xunit;

namespace Test.UnitTest
{
    [TestClass]
    public class ContactControllersTest : TestsContext
    {
        #region TestInit and Injections
        private ApplicationDbContext _context;
        private IContactsServices contactsServices;
        private IUnitOfWork unitOfWork;
        private IUriService uriService;
        private IImageService imageService;
        private IMailService mailService;
        private IOrganizationsServices organizationsServices;
        private IConfiguration configurations;

        [TestInitialize]
        public void Init()
        {
            var configcollection = new Dictionary<string, string>()
            {
                {"SendGridAPIKey","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"},
                {"VerifiedAPIMail","ongalkemy@gmail.com" },
                {"Welcomesubject","Bienvenido" },
                {"WelcomeMessage","Usted ha sido registrado exitosamente en nuestra plataforma." },
                {"ContactMessage","Gracias por contactarnos" },
                {"ContactBodyMessage","Nuestro equipo se va a comunicar a la brevedad" }
            };
            configurations = new ConfigurationBuilder()
                .AddInMemoryCollection(configcollection)
                .Build();

            _context = GetTestContext(Guid.NewGuid().ToString());
            unitOfWork = new UnitOfWork(_context);
            imageService = new ImageService();
            uriService = new UriService("http://testUriBase");
            organizationsServices = new OrganizationsServices(unitOfWork, imageService);
            mailService = new SendGridMailService(configurations, organizationsServices);
            contactsServices = new ContactsServices(unitOfWork, mailService, uriService);
            SeedContacts(_context);
        } 
        #endregion

        
        [TestMethod]
        public async Task GetAll_ShouldReturn_10ItemsPerPage()
        {
            using (_context)
            {
                //Arrange
                Init();

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Path = "/testUri";

                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                var controller = new ContactsController(contactsServices);
                controller.ControllerContext = controllerContext;

                //Act

                var response = await controller.GetAll(1);
                var expected = (ObjectResult)response.Result;

                //Assert
                Xunit.Assert.Equal(200, expected.StatusCode);
                Xunit.Assert.IsType<PaginationDTO<ContactDTO>>(expected.Value);
            }

        }
        [TestMethod]
        public async Task GetById_ShouldReturn_ContactOk()
        {
            using (_context)
            {
                //Arrange
                Init();
                var controller = new ContactsController(contactsServices);

                //Act
                var response = await controller.Get(1);
                var expected = (ObjectResult)response.Result;

                //Assert
                Xunit.Assert.Equal(200, expected.StatusCode);
                Xunit.Assert.IsType<ContactDTO>(expected.Value);
            }
        }
       
        [TestMethod]
        public async Task Insert_ShouldReturnOk_WhenInserted()
        {
            //Assert
            Init();

            var controller = new ContactsController(contactsServices);
            var newInsertContact = new ContactInsertDTO()
            {
                Name = "PruebaInsert",
                Email = "flewfarfaster@gmail.com",
                Phone = 103085255,
                Message = "Mensaje Prueba Insert"
            };

            //Act
            var response = await controller.Insert(newInsertContact);
            var castedExpected = response as OkObjectResult;

            //Assert
            Xunit.Assert.NotNull(castedExpected);
            Xunit.Assert.Equal(200, castedExpected.StatusCode);

        }
        private void SeedContacts(ApplicationDbContext context)
        {
            for (int i = 1; i < 11; i++)
            {
                var contact = new Contacts
                {
                    Id = i,
                    Name = "Contact " + i,
                    Phone = 381 + i,
                    Email = "Email for contact " + i,
                    Message = "Message from contact " + i,
                    CreatedAt = DateTime.Now
                };

                var organization = new Organizations
                {
                    Id = i,
                    Name = "Organization " + i,
                    Image = "ImageOrganizations" + i + ".jpg",
                    Address = "Address for Organization " + i,
                    Phone = 381 + i,
                    Email = "Email for Organization " + i,
                    WelcomeText = "WelcomeText for Organization " + i,
                    AboutUsText = "AboutUsText for Organization " + i,
                    CreatedAt = DateTime.Now,
                    FacebookUrl = "FacebookUrl" + i,
                    InstagramUrl = "InstagramUrl" + i,
                    LinkedinUrl = "InstragramUrl" + i
                };
                context.Add(organization);
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
