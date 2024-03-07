using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.IntegrationTests
{
    public class UserTests : BaseIntegrationTest
    {
        public UserTests(IntegrationTestFactory factory) : base(factory)
        {
            
        }

        [Fact]
        public async Task Create_Should_Add_New_User_To_Database()
        {
            // Arrange
            var test = new UserRegisterRequest
            {
                Email = "Test@gmail.com",
                PasswordHash = "6Hda#8Fd&-d2",
                ConfirmPassword = "6Hda#8Fd&-d2"
            };

            // Act
            var response = await Sender.Send(test);

            // Assert
            var user = _context.Users.FirstOrDefault(u => u.Id == response.Result);
            
            Assert.NotNull(user);
        }

        [Fact]
        public async Task Get_ShouldGet_CategoryFromDatabase()
        {
            // Arrange

            // Act
            var category = _context.Category.FirstOrDefault(c => c.Id == 1);

            //Assert
            Assert.NotNull(category);
        }
    }
}

