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
using OngProject.Core.Helper;
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
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Test.Helper;
using Xunit;

namespace Test.UnitTest
{
    [TestClass]
    public class UsersControllersTest : TestsContext
    {
        #region TestInit and Injections
        private ApplicationDbContext _context;
        private IUserServices UserServices;
        private IUnitOfWork unitOfWork;
        private IUriService uriService;
        private IImageService imageService;
        private IMailService mailService;
        private IOrganizationsServices organizationsServices;
        private IConfiguration configurations;
        private IRoleServices RoleServices;
        private JwtHelper jwtHelper;

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
                {"ContactBodyMessage","Nuestro equipo se va a comunicar a la brevedad" },
                {"JWT:Secret","123456789123456789" }
            };
            configurations = new ConfigurationBuilder()
                .AddInMemoryCollection(configcollection)
                .Build();

            _context = GetTestContext(Guid.NewGuid().ToString());
            unitOfWork = new UnitOfWork(_context);
            imageService = new ImageService();
            uriService = new UriService("http://testUriBase");
            UserServices = new UserServices(unitOfWork,mailService,configurations, imageService);
            jwtHelper = new JwtHelper(configurations);
            SeedMember(_context);
        } 
        #endregion

        [TestMethod]
        public async Task GetAll_ShouldReturn_10ItemsPerPage()
        {
                //Arrange
                Init();
                

                var controller = new UsersController(UserServices);

                //Act

                var response = await controller.GetAll();
                var expected = (ObjectResult)response.Result;

                //Assert
                Xunit.Assert.Equal(200, expected.StatusCode);
                Xunit.Assert.IsType<List<UserDTO>>(expected.Value);
        }

        [TestMethod]
        public async Task Delete_ShouldReturn_MemberOk()
        {
            using (_context)
            {
                Init();
                //Arrange
                var loginDTO = new LoginDTO
                {
                    Email = "Test@gmail.com",
                    Password = "123456"
                };
                var loginController = new AuthController(UserServices, jwtHelper);
                var userController = new UsersController(UserServices);
              

                

                //Act
                var login = await loginController.Login(loginDTO);


                var httpContext = new DefaultHttpContext();
                var r = login as OkObjectResult;
                string token = r.Value.ToString();

                httpContext.Request.Headers.Add("Authorization", "Bearer " + token);

                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                userController.ControllerContext = controllerContext;

                var response = await userController.Delete();
                var expected = (OkObjectResult)response.Result;
                //Assert
                //Xunit.Assert.NotNull(expected);
                Xunit.Assert.Equal(200, expected.StatusCode);
            }
               }
        [TestMethod]
        public async Task Update_ShouldReturnOk_WhenUpdate()
        {
            //Arrange
            Init();
            var loginDTO = new LoginDTO
            {
                Email = "Test@gmail.com",
                Password = "123456"
            };
            var loginController = new AuthController(UserServices, jwtHelper);
            var userController = new UsersController(UserServices);

            //Act
            var login = await loginController.Login(loginDTO);


            var httpContext = new DefaultHttpContext();
            var r = login as OkObjectResult;
            string token = r.Value.ToString();

            httpContext.Request.Headers.Add("Authorization", "Bearer " + token);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            userController.ControllerContext = controllerContext;

            var newUpdaUser = new UserUpdateDTO()
            {
                FirstName = "TestPrueba1",
                LastName = "Test1",
                Email = "Test1@gmail.com",
                Password = Encrypt.GetSHA256("123456"),
                Photo = null
            };

            //Act
            var response = await userController.Update(newUpdaUser);
            var expected = (ObjectResult)response.Result;

            //Assert
            Xunit.Assert.Equal(200, expected.StatusCode);

        }

        private void SeedMember(ApplicationDbContext context)
        {
         
                var role = new Role
                {
                    Name = "Administrator",
                    Description = "Usuario Administrador"
                };

                var user = new User
                { 
                    FirstName = "TestPrueba",
                    LastName = "Test",
                    Email = "Test@gmail.com",
                    Password = Encrypt.GetSHA256("123456"),
                    Photo = null,
                    RoleId = 1
                };
                var organization = new Organizations
                {
                    Name = "Organization ",
                    Image = "ImageOrganizations.jpg",
                    Address = "Address for Organization",
                    Phone = 381,
                    Email = "Email for Organization ",
                    WelcomeText = "WelcomeText for Organization ",
                    AboutUsText = "AboutUsText for Organization " ,
                    CreatedAt = DateTime.Now,
                    FacebookUrl = "FacebookUrl" ,
                    InstagramUrl = "InstagramUrl",
                    LinkedinUrl = "InstragramUrl"
                };
            
                context.Add(role);

                context.Add(organization); 
                context.Add(user);
                
            
            context.SaveChanges();

        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
