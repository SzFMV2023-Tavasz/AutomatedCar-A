namespace AutomatedCarTests.Models
{
    using AutomatedCar;
    using AutomatedCar.Models;
    using Avalonia;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Avalonia.Controls.Platform;
    using static AutomatedCar.Models.World;

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
        [InlineData("Oval", WorldType.Oval)]
        [InlineData("Test_World", WorldType.Test)]
        public void CreateOvalWorldTest(string worldName, WorldType worldType)
        {
            var world = CreateWorldAccordingToKeyWorld(worldName);

            // Assert
            Assert.Equal(worldType, world.SelectedWorld);
            Assert.True(world.WorldObjects.Count() > 0);

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
