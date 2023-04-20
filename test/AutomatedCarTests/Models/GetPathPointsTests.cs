namespace Tests.Models
{
    using System;
    using System.Linq;
    using AutomatedCar;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetPathPointsTests
    {

        //[TestMethod]
        //[DataRow("NPC_test_world_path.json", "car", 37)]
        //[DataRow("NPC_test_world_path.json", "pedestrian", 47)]
        //[DataRow("NPC_oval_world_path.json", "car", 44)]
        //public void GetPathPointsToSelectedWorldTest(string filePath, string type, int expectedLength)
        //{
        //    var app = new App();
        //    var pathPoints = app.GetPathPointsFrom(filePath, type);

        //    // Assert
        //    Assert.AreEqual(expectedLength, pathPoints.Count());
        //    Assert.IsTrue(pathPoints.Count() != 0);

        //}

        [TestMethod]
        [DataRow("NPC_test_world_path.json", "nonexistentkeyworld")]
        [DataRow("NPC_oval_world_path.json", "nonexistentkeyworld")]
        public void GetPathPointsAndGetExceptionTest(string filePath, string type)
        {
            var app = new App();
            Assert.ThrowsException<NullReferenceException>(() => app.GetPathPointsFrom(filePath, type));
        }
    }
}
