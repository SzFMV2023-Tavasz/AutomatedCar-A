namespace AutomatedCarTests.Models
{
    using AutomatedCar;
    using AutomatedCar.Models;
    using Avalonia;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Avalonia.Controls.Platform;

    public class WorldInitTests
    {

        private World CreateWorldAccordingToKeyWorld(string worldName) 
        {
            // Arrange
            World.Instance.WorldObjects.Clear();

            var app = new App();
            var threadingInterface = new InternalPlatformThreadingInterface();

            AvaloniaLocator.CurrentMutable
                .Bind<Avalonia.Platform.IPlatformThreadingInterface>()
                .ToConstant(threadingInterface);


            // Act
            var world = app.CreateWorld(worldName);
            return world;
        }

        [Theory]
        [InlineData("Oval", 203)]
        public void CreateOvalWorldTest(string worldName, int expectedObjCnt)
        {
            var world = CreateWorldAccordingToKeyWorld(worldName);

            // Assert
            Assert.Equal(expectedObjCnt, world.WorldObjects.Where(obj => obj.GetType() == typeof(WorldObject)).ToList().Count());

        }


        [Theory]
        [InlineData("nonexistentKeyworld")]
        public void CreateWorldGetExceptionTest(string worldName)
        {
            // Assert
            Assert.Throws<KeyNotFoundException>(() => CreateWorldAccordingToKeyWorld(worldName));
        }

    }
}
