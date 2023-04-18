namespace AutomatedCar
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.NPC;
    using AutomatedCar.ViewModels;
    using AutomatedCar.Views;
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using Newtonsoft.Json.Linq;
    using static AutomatedCar.Models.World;

    public class App : Application
    {
        delegate void LoadSelectedWorldMethod(World world, bool loadOnlyStaticAssets);

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

                    var world = this.CreateWorld(selectedWorld, false);

                    var mainWindow = new MainWindow { DataContext = new MainWindowViewModel(world) };

                    mainWindow.Show();
                    desktop.MainWindow = mainWindow;
                    selectionWindow.Close();
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public World CreateWorld(string selectedTrack, bool loadOnlyStaticAssets)
        {
            var world = World.Instance;

            this.worldKeyWorldToActionMap[selectedTrack].Invoke(world, loadOnlyStaticAssets);

            return world;
        }

        void LoadTestWorldAssets(World world, bool loadOnlyStaticAssets)
        {
            this.AddDummyCircleTo(world);
            world.SetSelectedWorldTo(WorldType.Test);
            world.PopulateFromJSON($"AutomatedCar.Assets.test_world.json");


            if (!loadOnlyStaticAssets)
            {
                this.AddControlledCarsToTest(world);
                this.AddNPCsToTest(world);

                // add further assets to the TEST world HERE
            }
        }

        void LoadOvalWorldAssets(World world, bool loadOnlyStaticAssets)
        {

            world.PopulateFromJSON($"AutomatedCar.Assets.oval.json");
            world.SetSelectedWorldTo(WorldType.Oval);


            if (!loadOnlyStaticAssets)
            {
                this.AddControlledCarsToOval(world);
                this.AddNPCsToOval(world);

                // add further assets to the OVAL world HERE
            }
        }

        private void AddNPCsToOval(World world)
        {
            // create 1 NPC car here that can be added to the OVAL track
            var car1 = this.CreateNPCCar(545, 4860, 0, "car_3_black.png", world, WorldType.Oval, this.GetPathPointsFrom("NPC_oval_world_path.json", "car"));
            world.AddObject(car1);

            world.npcManager.Start();
        }

        private void AddNPCsToTest(World world)
        {
            // create 1 NPC Pedestrian and 1 NPC car here that can be added to the TEST track
            var car1 = this.CreateNPCCar(3620, 1700, 180, "car_3_black.png", world, WorldType.Test, this.GetPathPointsFrom("NPC_test_world_path.json", "car"));
            var pedestrian1 = this.CreateNPCPedestrian(1950, 630, 270, "man.png", world, this.GetPathPointsFrom("NPC_test_world_path.json", "pedestrian"));
            world.AddObject(car1);
            world.AddObject(pedestrian1);

            world.npcManager.Start();
        }
        private PolylineGeometry GetNPCCarBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            foreach (var i in stuff["objects"][6]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);

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
        private NPCCar CreateNPCCar(int x, int y, int rotation, string filename, World world, WorldType type, List<PathPoint> pathPoints)
        {
            var npcCar = new NPCCar(x, y, filename, type, world.npcManager);

            npcCar.Geometry = this.GetNPCCarBoundaryBox();
            npcCar.RawGeometries.Add(npcCar.Geometry);
            npcCar.Geometries.Add(npcCar.Geometry);
            npcCar.RotationPoint = new System.Drawing.Point(54, 120);
            npcCar.Rotation = rotation;

            npcCar.PathPoints = pathPoints;
            return npcCar;
        }

        private Pedestrian CreateNPCPedestrian(int x, int y, int rotation, string filename, World world, List<PathPoint> pathPoints)
        {
            var pedestrian = new Pedestrian(x, y, filename, world.npcManager);

            pedestrian.Geometry = this.GetControlledNPCPedestrianBoundaryBox();
            pedestrian.RawGeometries.Add(pedestrian.Geometry);
            pedestrian.Geometries.Add(pedestrian.Geometry);
            pedestrian.RotationPoint = new System.Drawing.Point(37, 30);
            pedestrian.Rotation = rotation;

            pedestrian.PathPoints = pathPoints;

            return pedestrian;
        }

        private PolylineGeometry GetControlledNPCPedestrianBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            foreach (var i in stuff["objects"][30]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
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
        public List<PathPoint> GetPathPointsFrom(string filePath, string type)
        {
            // Gives back a pathPoint list related to a specific NPC type
            // type: "car" or "pedestrian", anything else throws an exception

            List<PathPoint> pathPoints = new List<PathPoint>();

            string fullPath = $"AutomatedCar.Assets.NPCpaths." + filePath;
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(fullPath));
            string json_text = reader.ReadToEnd();
            dynamic pathPointList = JObject.Parse(json_text);

            foreach (var point in pathPointList[type])
            {
                pathPoints.Add(new PathPoint(
                    point["x"].ToObject<int>(),
                    point["y"].ToObject<int>(),
                    point["rotation"].ToObject<double>(),
                    point["speed"].ToObject<int>()));
            }

            return pathPoints;
        }
    }
}