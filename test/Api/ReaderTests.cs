using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.Xunit;
using Xunit;

namespace StrawberryTesting.Tests.Api
{
    public class ReaderTests : BaseApiTest
    {
        [Fact]
        public async Task GivenAnonymousUser_WhenGetBlogs_ReturnsPosts()
        {
            using var testServer = GivenTestHost();
            var blogClient = testServer.GetBlogClient();

            var operationResult = await blogClient.ReadBlogs.ExecuteAsync();
            
            operationResult.Should().MatchSnapshot();
        }
        
        [Fact]
        public async Task GivenReaderAndi_WhenGetBlogs_ReturnsPosts()
        {
            using var testServer = GivenTestHost();
            var blogClient = testServer.GetBlogClient(TestUsers.ReaderAndi);

            var operationResult = await blogClient.ReadBlogs.ExecuteAsync();
            
            operationResult.Should().MatchSnapshot();
        }
    }
}