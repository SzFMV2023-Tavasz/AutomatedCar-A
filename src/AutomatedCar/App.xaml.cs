namespace AutomatedCar
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using AutomatedCar.Models;
    using AutomatedCar.ViewModels;
    using AutomatedCar.Views;
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using Newtonsoft.Json.Linq;

    public class App : Application
    {
        delegate void LoadSelectedWorldMethod(World world);

        private string TEST_WORLD_KEYWORD = "Test_World";
        private string OVAL_WORLD_KEYWORD = "Oval";

        private Dictionary<string, LoadSelectedWorldMethod> worldKeyWorldToActionMap = new Dictionary<string, LoadSelectedWorldMethod>();

        public App()
        {
            this.worldKeyWorldToActionMap.Add(TEST_WORLD_KEYWORD, LoadTestWorldAssets);
            this.worldKeyWorldToActionMap.Add(OVAL_WORLD_KEYWORD, LoadOvalWorldAssets);
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var vm = new WorldSelectionViewModel();
                var selectionWindow = new WorldSelectionWindow() { DataContext = vm };
                selectionWindow.Show();

                vm.WorldSelectedEvent += (sender, args) =>
                {

                    var selectedWorld = vm.SelectedWorld;
                    if (selectedWorld == null)
                    {
                        return;
                    }

                    var world = this.CreateWorld(selectedWorld);
                    
                    var mainWindow = new MainWindow { DataContext = new MainWindowViewModel(world) };

                    mainWindow.Show();
                    desktop.MainWindow = mainWindow;
                    selectionWindow.Close();
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public World CreateWorld(string selectedTrack)
        {
            var world = World.Instance;

            this.worldKeyWorldToActionMap[selectedTrack].Invoke(world);

            return world;
        }

        void LoadTestWorldAssets(World world)
        {
            this.AddDummyCircleTo(world);

            world.PopulateFromJSON($"AutomatedCar.Assets.test_world.json");

            this.AddControlledCarsToTest(world);

            this.AddNPCsToTest(world);

            // add further assets to the TEST world HERE
        }

        void LoadOvalWorldAssets(World world)
        {

            world.PopulateFromJSON($"AutomatedCar.Assets.oval.json");

            this.AddControlledCarsToOval(world);

            this.AddNPCsToOval(world);

            // add further assets to the OVAL world HERE
        }

        private PolylineGeometry GetControlledCarBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            foreach (var i in stuff["objects"][0]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
        }

        private void AddDummyCircleTo(World world)
        {
            var circle = new Circle(200, 200, "circle.png", 20);
            
            circle.Width = 40;
            circle.Height = 40;
            circle.ZIndex = 20;
            circle.Rotation = 45;

            world.AddObject(circle);
        }

        private AutomatedCar CreateControlledCar(int x, int y, int rotation, string filename)
        {
            var controlledCar = new Models.AutomatedCar(x, y, filename);
            
            controlledCar.Geometry = this.GetControlledCarBoundaryBox();
            controlledCar.RawGeometries.Add(controlledCar.Geometry);
            controlledCar.Geometries.Add(controlledCar.Geometry);
            controlledCar.RotationPoint = new System.Drawing.Point(54, 120);
            controlledCar.Rotation = rotation;

            controlledCar.Start();

            return controlledCar;
        }

        private void AddControlledCarsToTest(World world)
        {
            var controlledCar = this.CreateControlledCar(480, 1425, 0, "car_1_white.png");
            var controlledCar2 = this.CreateControlledCar(4250, 1420, -90, "car_1_red.png");

            world.AddControlledCar(controlledCar);
            world.AddControlledCar(controlledCar2);
        }
        private void AddControlledCarsToOval(World world)
        {
            var controlledCar = this.CreateControlledCar(550, 5465, 0, "car_1_white.png");

            world.AddControlledCar(controlledCar);

        }

        private void AddNPCsToOval(World world)
        {
            // create 1 NPC Pedestrian and 1 NPC car here that can be added to the OVAL track
        }

        private void AddNPCsToTest(World world)
        {
            // create 1 NPC Pedestrian and 1 NPC car here that can be added to the TEST track
        }
    }
}