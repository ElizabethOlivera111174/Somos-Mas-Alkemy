using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Controllers;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Services;
using OngProject.Core.Services.AWS;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System.IO;
using Test.Helper;
using System.Linq;
using System.Threading.Tasks;
using System;
using OngProject.Core.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using OngProject.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.Mapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Test.UnitTest
{
    [TestClass]
    public class AuthTest : BaseTest
    {
        private ApplicationDbContext _context;
        private AuthController authController;
        private IConfiguration _configuration;
        private IUserServices _userService;
        private JwtHelper _JwtHelper;

        public object Request { get; private set; }

        [TestInitialize]
        public void MakeArrange()
        {
            var config = new Dictionary<string, string>{
                {"JWT:Secret", "123456789123456789"},
                {"SendGridAPIKey","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"},
                {"VerifiedAPIMail","ongalkemy@gmail.com" },
                {"Welcomesubject","Bienvenido" },
                {"WelcomeMessage","Usted ha sido registrado exitosamente en nuestra plataforma." },
                {"ContactMessage","Gracias por contactarnos" },
                {"ContactBodyMessage","Nuestro equipo se va a comunicar a la brevedad" }

             };
             _configuration = new ConfigurationBuilder()
                 .AddInMemoryCollection(config)
                 .Build();
            _context = MakeContext("TestDb");
            IUnitOfWork unitOfWork = new UnitOfWork(_context);
            IImageService imagenService = new ImageService();
            IOrganizationsServices _organization = new OrganizationsServices(unitOfWork,imagenService);
            IMailService EmailService = new SendGridMailService(_configuration, _organization);
            _userService = new UserServices(unitOfWork, EmailService, _configuration, imagenService);
            _JwtHelper = new JwtHelper(_configuration);
             authController = new AuthController(_userService,_JwtHelper);
            SeedContacts(_context);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        public IFormFile CreateImage()
        {
            var stream = File.OpenRead(@"..\..\..\Image\Captura1.PNG");
            var image = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };
            return image;
        }

        [TestMethod]
        public async Task Register_RegistroExitoso()
        {
            
            //Arrange
            MakeArrange();
            UserInsertDTO newUser = new UserInsertDTO();
            newUser.FirstName = "User";
            newUser.LastName = "Test";
            newUser.Email = "ailenadriana@gmail.com";
            newUser.Password = "12345";
            newUser.Photo = null;

            //Act
            var response = await authController.Register(newUser);
            var castedExpected = response as OkObjectResult;

            //Assert
            Assert.AreEqual(200, castedExpected.StatusCode);
        }
        [TestMethod]
        public async Task Register_RegistroNOExitoso()
        {
            //Arrange
            //MakeArrange();
            UserInsertDTO newUser = new UserInsertDTO();
            newUser.FirstName = "User";
            newUser.LastName = "Test";
            newUser.Email = "user@example.com";
            newUser.Password = "123";
            newUser.Photo = null;
            

            //Act
            var response = await authController.Register(newUser);
            var castedExpected = response as BadRequestObjectResult;

            //Assert
            Assert.AreEqual(400, castedExpected.StatusCode);
        }
        [TestMethod]
        public async Task Login_LoginExitoso()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            LoginDTO userLogin = new LoginDTO();
            userLogin.Email = "ailenadrianagomez@gmail.com";
            userLogin.Password = "12345";

            //Act
            var response = await authController.Login(userLogin);
            var expected = response as OkObjectResult;

            // Assert
            Assert.AreEqual(200, expected.StatusCode);
        }

        [TestMethod]
        public async Task Login_LoginNOExitoso()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            LoginDTO userLogin = new LoginDTO();
            userLogin.Email = "user@gmail.com";
            userLogin.Password = "12345";

            //Act
            var response = await authController.Login(userLogin);
            var expected = response as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(400, expected.StatusCode);
        }
        [TestMethod]
        public async Task Me_MeExitoso()
        {
            Cleanup();
            MakeArrange();
            //Arrange
            LoginDTO userLogin = new LoginDTO();
            userLogin.Email = "user@example.com";
            userLogin.Password = "12345";

            //Act
            var tokenLogin = await authController.Login(userLogin);
            var httpContext = new DefaultHttpContext();
            var r = tokenLogin as OkObjectResult;
            string token = r.Value.ToString();

            httpContext.Request.Headers.Add("Authorization", "Bearer "+token);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            authController.ControllerContext = controllerContext;
            var response = await authController.Get();
            var expected = response as OkObjectResult;

            // Assert
            Assert.AreEqual(200, expected.StatusCode);
        }
        private void SeedContacts(ApplicationDbContext context)
        {
            var role = new Role
            {
                Name = "Administrator",
                Description = "Usuario administrador test"
            };
            var user = new User
            {
                FirstName = "User",
                LastName = "Test" ,
                Email = "user@example.com",
                Password = Encrypt.GetSHA256("12345"),
                Photo = null,
                RoleId = 1
            };
            var userLogin = new User
            {
                FirstName = "UserLogin",
                LastName = "TestLogin",
                Email = "ailenadrianagomez@gmail.com",
                Password = Encrypt.GetSHA256("12345"),
                Photo = null,
                RoleId = 1
            };

            var organization = new Organizations
            {
                Name = "Organization " ,
                Image = "ImageOrganizations.jpg",
                Address = "Address for Organization " ,
                Phone = 381 ,
                Email = "Email for Organization " ,
                WelcomeText = "WelcomeText for Organization " ,
                AboutUsText = "AboutUsText for Organization ",
                CreatedAt = DateTime.Now,
                FacebookUrl = "FacebookUrl",
                InstagramUrl = "InstagramUrl" ,
                LinkedinUrl = "InstragramUrl"
            };
                
            
            context.Add(organization);
            context.Add(user);
            context.Add(userLogin);
            context.Add(role);
            context.SaveChanges();

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
