using System.Collections.Generic;
using Moq;
using NerdDinner.Infrastructure;
using NerdDinner.Tests.NhHelpers;
using NUnit.Framework;
using NerdDinner.Controllers;
using System.Web.Mvc;
using NerdDinner.Models;

namespace NerdDinner.Tests.Controllers {
 
    [TestFixture]
    public class SearchControllerTest : NhInMemoryFixtureBase {

        SearchController CreateSearchController() {
            var repository = new NhDinnerRepository(Session);
            return new SearchController(Session, repository);
        }

        [Test]
        public void SearchByLocationAction_Should_Return_Json()
        {
            // Arrange            
            var controller = new SearchController(Session, new NhDinnerRepository(Session));
            // Act
            var result = controller.SearchByLocation(99, -99);

            // Assert
            Assert.IsInstanceOf(typeof(JsonResult), result);
        }

        [Test]
        public void SearchByLocationAction_Should_Return_JsonDinners()
        {

            // Arrange
            var controller = CreateSearchController();
            // Act
            var result = (JsonResult)controller.SearchByLocation(99, -99);

            // Assert
            Assert.IsInstanceOf(typeof(List<JsonDinner>), result.Data);
            var dinners = (List<JsonDinner>)result.Data;
            Assert.AreEqual(100, dinners.Count);
        }

        [Test]
        public void SearchByLocationAction_Should_Filter_By_Distance()
        {

            // Arrange
            var controller = CreateSearchController();
            // Act
            JsonResult result = controller.SearchByLocation(1, 1);

            // Assert
            Assert.IsInstanceOf(typeof(List<JsonDinner>), result.Data);
            var dinners = (List<JsonDinner>)result.Data;
            Assert.AreEqual(0, dinners.Count);
        }
    }
}
