using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TitanSoft.DataAccess;

namespace TitanSoft.Tests
{
    [TestClass]
    public class OmdbTests : TestBase
    {
        [TestMethod]
        public void Search_Movies_Test()
        {
            var term = "american";
            var api = new OmdbApi(config, logger) as IOmdbApi;
            var results = api.Search(term);
            Assert.IsTrue(results.Search.Count > 0);
        }

        [TestMethod]
        public void Load_Movie_Test()
        {
            var api = new OmdbApi(config, logger) as IOmdbApi;
            var id = "tt3896198";
            var result = api.GetMovie(id);
            Assert.IsTrue(result != null);
        }
    }
}
