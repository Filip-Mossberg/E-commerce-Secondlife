using E_commerce.Models.DTO_s.User;
using E_commerce_recycling.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.UnitTesting.UserTesting
{
    public class User : BaseIntegrationTest
    {
        public User(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
        }

        //[Fact]
        //public async Task Register_ShouldAdd_User_To_Database()
        //{
        //    // Arrange
        //    UserRegisterRequest request = new UserRegisterRequest();

        //    // Act
        //    async Task Action() => Sender.Send(request);

        //    // Assert
        //    await Assert.ThrowsAsync<ArgumentException>(Action);

        //}
    }
}
