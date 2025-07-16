using Integration.Supabase.Interfaces;
using Integration.Supabase.Models.Auth.Users;
using Moq;
using StraySafe.Logic.Admin;
using StraySafe.Test.MockedServices;

namespace StraySafe.Test.Admin;

[TestClass]
public class AdminClientTest
{
    private Mock<ISupabaseService>? _supabaseServiceMock;
    private AdminClient? _adminClient;

    [TestInitialize]
    public void SetUp()
    {
        _supabaseServiceMock = SupabaseServiceMockFactory.Mock();
        _adminClient = new AdminClient(_supabaseServiceMock.Object);
    }

    [TestMethod]
    public async Task GetAllUsers_CalledNormally_GetsAllUsers()
    {
        List<User> users = await _adminClient!.GetAllUsers();
        Assert.AreEqual(2, users.Count);
    } 
}
