using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Obstacles;
using PowerUps;
using System.IO;
using Enumerators;
using Controllers;
using Menu;
using System;
using Misc;

#pragma warning disable 0618 // variable declared but not used.

public class GameManager : MonoBehaviour
{
    private GameSettings Settings { get; set; }

    public Transform halfScreen;
    public Transform nitro;

    public Material meadowMat; //Type 0
    public Material desertMat; //Type 1
    public Material snowMat;   //Type 2


    private float supplyTimer = 0.0f;
    private float rankingUpdateTimer = 0.0f;
    /// <summary>
    /// In Sekunden
    /// </summary>
    private float rankingUpdatePeriod = 5.0f; 

    private Transform LootParentGameObject  { get; set; }
    private Transform TreeParentGameObject  { get; set; }
    private Transform GrassParentGameObject { get; set; }
    private Transform RocksParentGameObject { get; set; }
    private Transform Camera { get; set; }
    private Transform Crate { get; set; }
    private Transform Canvas { get; set; }
    private static List<Transform> Trees { get; set; } = new List<Transform>();
    private static List<Transform> Rocks { get; set; } = new List<Transform>();
    private static List<Transform> Crates { get; set; } = new List<Transform>();
    private static List<Transform> Nitros { get; set; } = new List<Transform>();
    private static Dictionary<PlayerIndex, Player> Players { get; set; } = new Dictionary<PlayerIndex, Player>();
    private static List<StartingPoint> StartingPoints { get; set; } = new List<StartingPoint>();

    void Awake()
    {
        var menuController = GameObject.Find("MenuController");
        Settings = menuController.GetComponent<MenuController>().GameSettings;
        Destroy(menuController);

        Crate  = (Resources.Load("Obstacles/Crate") as GameObject).transform;
        Camera = (Resources.Load("Utility/Camera") as GameObject).transform;
        Canvas = GameObject.Find("GameStats").transform;

        LootParentGameObject  = GameObject.Find("Crates").transform;
        TreeParentGameObject  = GameObject.Find("Trees").transform;
        GrassParentGameObject = GameObject.Find("Grass").transform;
        RocksParentGameObject = GameObject.Find("Rocks").transform;

        //RandomizeMapType();
        SetupBorder();
        SetupGrass();
        SetupTrees();
        SetupRocks();
        SetupLighting();
    }

    void Update()
    {
        if(Time.time > rankingUpdateTimer)
        {
            rankingUpdateTimer += rankingUpdatePeriod;
            UpdateRanking();
        }

        if (Time.time > supplyTimer)
            supplyTimer += Settings.SupplyRefill;

        if (Settings.MapType == MapType.Snow)
            GameObject.Find("Snow").GetComponent<ParticleSystem>().Emit(Settings.SnowIntensity);
    }

    private void SetCameraViewport(Camera camera, PlayerIndex index)
    {
        if (Settings.PlayerCount == 2)
        {
            if (index == PlayerIndex.First)
                camera.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
            else if (index == PlayerIndex.Second)
                camera.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        }
        if (Settings.PlayerCount == 3)
        {
            if (index == PlayerIndex.First)
                camera.rect = new Rect(0.25f, 0.5f, 0.5f, 0.5f);
            else if (index == PlayerIndex.Second)
                camera.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            else if (index == PlayerIndex.Third)
                camera.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
        if (Settings.PlayerCount == 4)
        {
            if (index == PlayerIndex.First)
                camera.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            else if (index == PlayerIndex.Second)
                camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            else if (index == PlayerIndex.Third)
                camera.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            else if (index == PlayerIndex.Fourth)
                camera.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
    }

    private void SetupLighting()
    {
        Transform light = GameObject.Find("LightWrapper").transform;
        switch (Settings.MapType)
        {
            case MapType.Meadow:
                light.rotation = Quaternion.Euler(60, 60, 0);
                break;
            case MapType.Desert:
                light.rotation = Quaternion.Euler(75, 75, 0);
                break;
            case MapType.Snow:
                light.rotation = Quaternion.Euler(45, 45, 0);
                break;
        }
    }

    private void RandomizeMapType()
    {
        int type = UnityEngine.Random.Range(0, 3);
        if (type == 0) Settings.MapType = MapType.Meadow;
        if (type == 1) Settings.MapType = MapType.Desert;
        if (type == 2) Settings.MapType = MapType.Snow;
    }

    private void InitializePlayer(PlayerConfiguration playerConfiguration)
    {
        StartingPoint startingPoint = GetStartingPoint();
        var tankObject = Instantiate(Resources.Load("Tanks/" + playerConfiguration.TankType.ToString()), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
        var camera = Instantiate(Camera, Vector3.zero, Quaternion.Euler(0, 0, 0));

        var smoothCamera = camera.GetComponent<SmoothCamera>();
        smoothCamera = camera.GetComponent<SmoothCamera>();
        smoothCamera.tank = tankObject;

        var camObject = camera.Find("Camera").GetComponent<Camera>();
        SetCameraViewport(camObject, playerConfiguration.PlayerIndex);

        var canvas = Instantiate(halfScreen, Vector3.zero, Quaternion.Euler(0, 0, 0));
        canvas.GetComponent<Canvas>().planeDistance = 0.5f;
        canvas.GetComponent<Canvas>().worldCamera = camera.Find("Camera").GetComponent<Camera>();
        canvas.parent = tankObject.transform;
        canvas.name = "Canvas";

        var tank = tankObject.GetComponent<Tank>();
        tank.cam = camera;
        tank.PlayerIndex = playerConfiguration.PlayerIndex;
        tank.Paint(playerConfiguration.Color);
        tank.Controller = TankControllerFactory.GetTankController(playerConfiguration);

        tank.Initialize(new Vector3(startingPoint.Transform.position.x, startingPoint.Transform.eulerAngles.y, startingPoint.Transform.position.z));
        Players.Add(playerConfiguration.PlayerIndex, new Player { Tank = tank.transform, Configuration = playerConfiguration });
    }

    private static StartingPoint GetStartingPoint()
    {
        var startingPoint = StartingPoints
            .Where(x => !x.Used)
            .OrderBy(x => Guid.NewGuid())
            .First();

        startingPoint.Used = true;
        return startingPoint;
    }

    private void Start()
    {
        GameObject.Find("MapCam").GetComponent<Camera>().enabled = false;

        GameObject.FindGameObjectsWithTag("StartingPoint")
            .ToList()
            .ForEach(sp => StartingPoints.Add( new StartingPoint { Transform = sp.transform, Used = false }));

        Settings.PlayerConfigurations
            .ToList()
            .ForEach(playerConfiguration => InitializePlayer(playerConfiguration));

        Enumerable.Range(0, Settings.NitroCount)
            .ToList()
            .ForEach(_ => SpawnNitroAt(GetValidPosition()));

        Enumerable.Range(0, Settings.CrateCount)
            .ToList()
            .ForEach(_ => SpawnCrateAt(GetValidPosition()));

        Players.Values.ToList().ForEach(player =>
        {
            var psL = player.Tank.Find("TracksParticlesLeft").GetComponent<ParticleSystem>();
            var mainL = psL.main;
            var colL = psL.colorOverLifetime;
            var psR = player.Tank.Find("TracksParticlesRight").GetComponent<ParticleSystem>();
            var mainR = psR.main;
            var colR = psR.colorOverLifetime;

            var floor = GameObject.Find("Floor");
            Color c;
            Gradient g;

            colL.enabled = true;
            colR.enabled = true;

            switch(Settings.MapType)
            {
                case MapType.Desert:
                    floor.GetComponent<Renderer>().material = desertMat;
                    c = new Color(244f / 255f, 154f / 255f, 72f / 255f);
                    mainL.startColor = c;
                    mainR.startColor = c;
                    g = new Gradient();
                    g.SetKeys(new GradientColorKey[] { new GradientColorKey(c, 0.0f), new GradientColorKey(new Color(187 / 255, 97 / 255, 15 / 255), 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    colL.color = g;
                    colR.color = g;
                    break;
                case MapType.Snow:
                    floor.GetComponent<Renderer>().material = snowMat;
                    c = new Color(233, 233, 233);
                    mainL.startColor = c;
                    mainR.startColor = c;
                    g = new Gradient();
                    g.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color(200, 200, 200), 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    colL.color = g;
                    colR.color = g;
                    break;
                default:
                    floor.GetComponent<Renderer>().material = meadowMat;
                    break;
            }
        });
    }

    private void SetupTrees()
        => Enumerable.Range(0, Settings.TreeCount)
        .ToList()
        .ForEach(_ => {
            var tree = Instantiate(
                Resources.Load("Trees/" + Settings.MapType.ToString() + "/Tree_" + UnityEngine.Random.Range(1, 5)),
                GetFixedPosition(GetValidPosition(), -1.0f),
                Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0)) as GameObject;
            tree.transform.parent     = TreeParentGameObject;
            tree.transform.localScale = UnityEngine.Random.Range(0.5f, 1.5f) * Vector3.one;
            tree.transform.GetComponent<Obstacles.Tree>().InitialScaling = tree.transform.localScale.x;
            Trees.Add(tree.transform);
        });

    private void SetupRocks() 
        => Enumerable.Range(0, Settings.RockCount)
        .ToList()
        .ForEach(_ => {
            var rock = Instantiate(
                Resources.Load("Rocks/" + Settings.MapType.ToString() + "/Large_" + UnityEngine.Random.Range(1, 5)),
                GetFixedPosition(GetValidPosition(), -1.0f),
                Quaternion.Euler(-90,UnityEngine.Random.Range(0, 360), 0)) as GameObject;

            rock.transform.parent = RocksParentGameObject;
            rock.transform.localScale = new Vector3(UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(1, 4));
            Rocks.Add(rock.transform);
        });

    private TankColor RandomTankColor() => (TankColor)UnityEngine.Random.Range(0, 8);

    private TankType RandomTankType() => (TankType)UnityEngine.Random.Range(0, 4);

    private void UpdateRanking() 
        => Players
        .Values
            .Where(player => player.Tank.GetComponent<Tank>().Kills > 0)
            .OrderBy(player => player.Tank.GetComponent<Tank>().Kills)
            .Select(player => player.Configuration)
            .ToList()
            .ForEach(player => {
                int counter = 0;
                new List<Text> {
                    Canvas.Find("First").GetComponent<Text>(),
                    Canvas.Find("Second").GetComponent<Text>(),
                    Canvas.Find("Third").GetComponent<Text>()
                }
                    .Skip(counter)
                    .First()
                    .text = player.ToString();

                counter++;
            });

    private Vector3 GeneratePosition()
    {
        int offset = 10;
        float x = UnityEngine.Random.Range((-Settings.MapSize) + offset, Settings.MapSize - offset);
        float z = UnityEngine.Random.Range((-Settings.MapSize) + offset, Settings.MapSize - offset);
        float r = UnityEngine.Random.Range(0.0f, 360.0f);

        return new Vector3(x, r, z);
    }

    private void SpawnCrateAt(Vector3 pos)
        => Crates.Add(Instantiate(Crate, GetFixedPosition(pos), Quaternion.Euler(0, pos.y, 0)));

    private void SpawnNitroAt(Vector3 pos)
        => Nitros.Add(Instantiate(nitro, GetFixedPosition(pos, 1.0f), Quaternion.Euler(0, pos.y, 0)));

    private void SpawnTreeAt(int type, Vector3 pos)
    {
        var size     = UnityEngine.Random.Range(0.75f, 1.25f);
        var position = GetFixedPosition(pos);
        var path = "Trees/" + Settings.MapType.ToString() + "/Tree_" + type;

        var treeObject = Instantiate(Resources.Load(path), position , Quaternion.Euler(0, pos.y, 0), TreeParentGameObject) as GameObject;
        treeObject.transform.localScale = size * Vector3.one;

        var tree = treeObject.GetComponent<Obstacles.Tree>();
        tree.InitialScaling = size;
        Trees.Add(treeObject.transform);
    }

    private void SupplyDrop()
    {
        int crateDifference = Settings.CrateCount - Crates.Count;
        int nitroDifference = Settings.NitroCount - Nitros.Count;

        for (int i = 0; i < crateDifference / 2; i++)
            SpawnCrateAt(GetValidPosition());

        for (int i = 0; i < nitroDifference / 2; i++)
            SpawnNitroAt(GetValidPosition());
    }

    //ToDo fix this mess
    public static void GiveKillToPlayer(PlayerIndex owner)
    {
        if(Players.TryGetValue(owner, out var player))
            player.Tank.GetComponent<Tank>().AddKill();
    }

    public void DeleteCrate(Crate c) 
        => Crates.Remove(c.transform);

    public void DeleteTree(Obstacles.Tree t) 
        => Trees.Remove(t.transform);

    public static void DeleteNitro(PowerUp n) 
        => Nitros.Remove(n.transform);

    public Vector3 GetFixedPosition(Vector3 pos, float offset = 0.0f) 
        => (Physics.Raycast(new Ray(new Vector3(pos.x, 0.0f, pos.z), Vector3.down), out RaycastHit hit))
            ? new Vector3(hit.point.x, hit.point.y + offset, hit.point.z)
            : pos;

    public static void ShowEndScreen() 
        => SceneManager.LoadScene("EndScreen");

    public void SetupBorder()
    {
        var parent   = GameObject.Find("Rocks").transform;
        var position = new Vector3(Settings.MapSize, -5, 0);

        for (int i = 0; i < Settings.BorderSteps; i++)
        {
            int rotation = UnityEngine.Random.Range(0, 360);
            string objPath = "Rocks/";
            switch (Settings.MapType)
            {
                case MapType.Meadow:
                    objPath += "Meadow/Large_";
                    break;
                case MapType.Desert:
                    objPath += "Desert/Large_";
                    break;
                case MapType.Snow:
                    objPath += "Snow/Large_";
                    break;
            }
            objPath += UnityEngine.Random.Range(1, 5);

            position = Quaternion.Euler(0, (float)360 / Settings.BorderSteps, 0) * position;
            var newRock = Instantiate(Resources.Load(objPath), position, Quaternion.Euler(-90, rotation, 0), parent) as GameObject;
            newRock.transform.localScale = new Vector3(UnityEngine.Random.Range(5.0f, 7.0f), UnityEngine.Random.Range(5.0f, 7.0f), UnityEngine.Random.Range(5.0f, 7.0f));
        }
    }

    private void SetupGrass()
    {
        int density = 50;
        string path = Path.Combine("Grass", MapType.Meadow.ToString());

        switch (Settings.MapType)
        {
            case MapType.Desert:
                density /= 2;
                break;
            case MapType.Snow:
                density *= 2;
                break;
        }

        for (int i = 0; i < density; i++)
        {
            Vector3 position;
            do
                position = GeneratePosition();
            while (!IsPositionValid(position));

            GameObject grass = Instantiate(
                Resources.Load("Grass/Grass"),
                GetFixedPosition(position),
                Quaternion.Euler(0.0f,
                UnityEngine.Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;

            grass.transform.parent = GrassParentGameObject;
            grass.transform.localScale = new Vector3(UnityEngine.Random.Range(1.0f, 2), UnityEngine.Random.Range(1.0f, 2), UnityEngine.Random.Range(1.0f, 2));
            grass.transform.GetComponent<MeshRenderer>().material = Resources.Load(path) as Material;
        }
    }

    /// <summary>
    /// todo: Ist komplett broken, bitte fixen
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool IsPositionValid(Vector3 position)
    {
        //var fixedPosition = position;
        //fixedPosition.y = 0;

        //if (Vector3.Distance(Vector3.zero, fixedPosition) > Settings.MapSize - 5) return false;

        //var blockedPositions = new List<Transform>();

        //blockedPositions.AddRange(StartingPoints);

        //if (Crates.Count > 0) blockedPositions.AddRange(Crates);
        //if (Trees.Count > 0) blockedPositions.AddRange(Trees);
        //if (Rocks.Count > 0) blockedPositions.AddRange(Rocks);

        //foreach (Transform t in StartingPoints)
        //    if (Vector3.Distance(fixedPosition, new Vector3(t.position.x, 0, t.position.z)) < Settings.StartingRadius) return false;

        return true;
    }

    public static Vector3 GetStartingPoint(PlayerIndex playerIndex)
    {
        Players.TryGetValue(playerIndex, out var player);
        if (player == null) throw new Exception();//ToDo Exception Handling
        return StartingPoints
            .OrderBy(possibleStartingPoint => Vector3.Distance(player.Tank.position , possibleStartingPoint.Transform.position))
            .First()
            .Transform.position;
    }

    private Vector3 GetValidPosition(Vector3 offset) 
        => GetValidPosition() + offset;

    private Vector3 GetValidPosition()
    {
        Vector3 pos;
        do
            pos = GeneratePosition();
        while (!IsPositionValid(pos));
        return pos;
    }
}
