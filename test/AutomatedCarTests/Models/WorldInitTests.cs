namespace Tests.Models
{
    using AutomatedCar;
    using AutomatedCar.Models;
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using Avalonia.Threading;
    using Avalonia.Platform;
    using System.Threading;
    using Avalonia.Controls.Platform;

    public class WorldInitTests
    {

        private World CreateWorldAccordingToKeyWorld(string worldName) 
        {
            // Arrange
            var app = new App();
            var threadingInterface = new InternalPlatformThreadingInterface();

            AvaloniaLocator.CurrentMutable
                .Bind<Avalonia.Platform.IPlatformThreadingInterface>()
                .ToConstant(threadingInterface);


            // Act
            new World();    //reset world
            return app.CreateWorld(worldName); ;
        }

        [Theory]
        [InlineData("Oval", 249)]
        [InlineData("Test_World", 46)]
        public void CreateWorldTest(string worldName, int expectedObjCnt)
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
