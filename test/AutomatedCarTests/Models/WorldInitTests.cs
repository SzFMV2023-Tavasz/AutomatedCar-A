namespace Tests.Models
{
    using AutomatedCar;
    using AutomatedCar.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using static AutomatedCar.Models.World;

    public class WorldInitTests
    {

        private World CreateWorldAccordingToKeyWorld(string worldName, bool loadOnlyStaticAssets)
        {
            // Arrange
            World.Instance.WorldObjects.Clear();

            var app = new App();
            

            // Act
            var world = app.CreateWorld(worldName, loadOnlyStaticAssets);
            return world;
        }

        [Theory]
        [InlineData("Oval",true, WorldType.Oval)]
        [InlineData("Test_World",true, WorldType.Test)]
        public void CreateOvalWorldTest(string worldName,bool loadOnlyStaticAssets, WorldType worldType)
        {
            var world = CreateWorldAccordingToKeyWorld(worldName, loadOnlyStaticAssets);

            // Assert
            Assert.Equal(worldType, world.SelectedWorld);
            Assert.True(world.WorldObjects.Count() > 0);

        }

        [Theory]
        [InlineData("nonexistentKeyworld", true)]
        public void CreateWorldGetExceptionTest(string worldName, bool loadOnlyStaticAssets)
        {
            // Assert
            Assert.Throws<KeyNotFoundException>(() => CreateWorldAccordingToKeyWorld(worldName, loadOnlyStaticAssets));
        }

    }
}
