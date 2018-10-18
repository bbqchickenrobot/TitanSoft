using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TitanSoft.Tests
{
    public abstract class TestBase
    {
        protected IConfiguration config = A.Fake<IConfiguration>();
        protected ILogger logger = A.Fake<ILogger>();

        protected TestBase()
        {

        }
    }
}