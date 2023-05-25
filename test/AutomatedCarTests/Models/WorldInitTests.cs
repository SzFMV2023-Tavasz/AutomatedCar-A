namespace Tests.Models
{
    //using AutomatedCar;
    //using AutomatedCar.Models;
    //using System.Collections.Generic;
    //using System.Linq;
    //using Microsoft.VisualStudio.TestTools.UnitTesting;
    //using static AutomatedCar.Models.World;

    //[TestClass]
    //public class WorldInitTests
    //{

    //    private World CreateWorldAccordingToKeyWorld(string worldName, bool loadOnlyStaticAssets)
    //    {
    //        // Arrange
    //        World.Instance.WorldObjects.Clear();

    //        var app = new App();
            

    //        // Act
    //        var world = app.CreateWorld(worldName, loadOnlyStaticAssets);
    //        return world;
    //    }


    //    [TestMethod]
    //    [DataRow("Oval",true, WorldType.Oval)]
    //    [DataRow("Test_World",true, WorldType.Test)]
    //    public void CreateOvalWorldTest(string worldName,bool loadOnlyStaticAssets, WorldType worldType)
    //    {
    //        var world = CreateWorldAccordingToKeyWorld(worldName, loadOnlyStaticAssets);

    //        // Assert
    //        Assert.AreEqual(worldType, world.SelectedWorld);
    //        Assert.IsTrue(world.WorldObjects.Count() > 0);

    //    }

    //    [TestMethod]
    //    [DataRow("nonexistentKeyworld", true)]
    //    public void CreateWorldGetExceptionTest(string worldName, bool loadOnlyStaticAssets)
    //    {
    //        // Assert
    //        Assert.ThrowsException<KeyNotFoundException>(() => CreateWorldAccordingToKeyWorld(worldName, loadOnlyStaticAssets));
    //    }

    //}
}
