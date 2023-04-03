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

    [Collection("AvaloniaTestFramework")]
    public class WorldInitTests
    {

        // Mock implementation of IPlatformThreadingInterface
        public class MockPlatformThreadingInterface : IPlatformThreadingInterface
        {
            public bool CurrentThreadIsLoopThread => throw new NotImplementedException();

            public event Action<DispatcherPriority?> Signaled;

            public void RunLoop(CancellationToken cancellationToken)
            {
                // Do nothing
            }

            public void Signal()
            {
                // Do nothing
            }

            public void Signal(DispatcherPriority priority)
            {
                throw new NotImplementedException();
            }

            public void StartTimer(TimeSpan interval, Action callback)
            {
                // Do nothing
            }

            public IDisposable StartTimer(DispatcherPriority priority, TimeSpan interval, Action tick)
            {
                throw new NotImplementedException();
            }

            public void StopTimer()
            {
                // Do nothing
            }
        }

        

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
