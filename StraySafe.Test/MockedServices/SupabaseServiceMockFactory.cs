using Integration.Supabase.Interfaces;
using Integration.Supabase.Models.Auth.Users;
using Microsoft.AspNetCore.Http;
using Moq;

namespace StraySafe.Test.MockedServices;

public class SupabaseServiceMockFactory
{
    public static Mock<ISupabaseService> Mock()
    {
        Mock<ISupabaseUserService> userServiceMock = new(MockBehavior.Strict);
        Mock<ISupabaseAdminService> adminServiceMock = new(MockBehavior.Strict);
        Mock<ISupabaseService> supabaseServiceMock = new(MockBehavior.Strict);

        MockUserService(userServiceMock);
        MockAdminService(adminServiceMock);
        MockSupabaseService(supabaseServiceMock, userServiceMock, adminServiceMock);

        return supabaseServiceMock;
    }

    private static void MockSupabaseService(Mock<ISupabaseService> supabaseServiceMock,
                                            Mock<ISupabaseUserService> userServiceMock,
                                            Mock<ISupabaseAdminService> adminServiceMock)
    {
        supabaseServiceMock.Setup(x => x.User).Returns(userServiceMock.Object);
        supabaseServiceMock.Setup(x => x.Admin).Returns(adminServiceMock.Object);
    }

    private static void MockUserService(Mock<ISupabaseUserService> userServiceMock)
    {
        userServiceMock.Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(new User
            {
                Email = "testing@straysafe.net",
                Id = "123456789"
            });

        userServiceMock.Setup(x => x.UploadImage(It.IsAny<IFormFile>()))
            .ReturnsAsync("https://www.google.com");
    }

    private static void MockAdminService(Mock<ISupabaseAdminService> adminServiceMock)
    {
        adminServiceMock.Setup(x => x.GetAllUsersAsync())
            .ReturnsAsync(
            [
                new()
                {
                    Email = "testing@straysafe.net",
                    Id = "123456789"
                },
                new()
                {
                    Email = "testing2@straysafe.net",
                    Id = "987654321"
                }
            ]);
    }
}
