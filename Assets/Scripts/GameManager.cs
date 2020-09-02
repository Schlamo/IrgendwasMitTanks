using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Obstacles;
using PowerUps;
using System.IO;
using System;
using Enumerators;
using Controllers;

#pragma warning disable 0618 // variable declared but not used.

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float startingAreaRadius = 5.0f;
    public float supplyRefill       = 15.0f;

    public int amountOfPlayers      = 2;
    public int borderSteps          = 72;
    public int nitroAmount          = 15;
    public int crateAmount          = 15;
    public int rocksAmount          = 10;
    public int treeAmount           = 150;
    public int mapSize              = 100;
    public int snowIntensity        = 25;
    public bool useKeyboard         = false;
    public bool dayNightCycle       = false;

    public Transform halfScreen;
    public Transform nitro;

    public Material meadowMat; //Type 0
    public Material desertMat; //Type 1
    public Material snowMat;   //Type 2

    public MapType mapType = MapType.Meadow;

    private float supplyTimer = 0.0f;
    private float time = 0.0f;

    private Transform Camera { get; set; }
    private Transform Crate { get; set; }

    private List<Transform> Trees { get; set; } = new List<Transform>();
    private List<Transform> Rocks { get; set; } = new List<Transform>();
    private List<Transform> Crates { get; set; } = new List<Transform>();
    private List<Transform> Nitros { get; set; } = new List<Transform>();
    private List<Transform> Players { get; set; } = new List<Transform>();
    private List<Transform> StartingPoints { get; set; } = new List<Transform>();

    private readonly List<string> colors = new List<string> { "Red", "Blue", "Green", "Yellow", "Pinkt", "Orange", "Black", "White" };

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        RandomizeMapType();
        SetupBorder();
        SetupGrass();
        SetupLighting();

        Crate  = (Resources.Load("Obstacles/Crate") as GameObject).transform;
        Camera = (Resources.Load("Utility/Camera") as GameObject).transform;
    }

    void Start() => StartGame();

    void Update()
    {
        UpdateRanking();

        supplyTimer += Time.deltaTime;

        if (supplyTimer > supplyRefill)
        {
            SupplyDrop();
            supplyTimer -= supplyRefill;
        }
        if (mapType == MapType.Snow)
        {
            GameObject.Find("Snow").GetComponent<ParticleSystem>().Emit(snowIntensity);
        }

        time += Time.deltaTime;
    }

    private void SetCameraViewport(Camera camera, int playerId)
    {
        if (amountOfPlayers == 2)
        {
            if (playerId == 1)
                camera.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
            else if (playerId == 2)
                camera.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        }
        if (amountOfPlayers == 3)
        {
            if (playerId == 1)
                camera.rect = new Rect(0.25f, 0.5f, 0.5f, 0.5f);
            else if (playerId == 2)
                camera.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            else if (playerId == 3)
                camera.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
        if (amountOfPlayers == 4)
        {
            if (playerId == 1)
                camera.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            else if (playerId == 2)
                camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            else if (playerId == 3)
                camera.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            else if (playerId == 4)
                camera.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
    }

    private void SetupLighting()
    {
        Transform light = GameObject.Find("LightWrapper").transform;
        switch (mapType)
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
        if (type == 0) mapType = MapType.Meadow;
        if (type == 1) mapType = MapType.Desert;
        if (type == 2) mapType = MapType.Snow;
    }

    private void SetupGrass()
    {
        int density = 50;
        string path = Path.Combine("Grass", MapType.Meadow.ToString());

        switch (mapType)
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

            GameObject grass = Instantiate(Resources.Load("Grass/Grass"), GetFixedPosition(position), Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
            grass.transform.localScale = new Vector3(UnityEngine.Random.Range(1.0f, 2), UnityEngine.Random.Range(1.0f, 2), UnityEngine.Random.Range(1.0f, 2));
            grass.transform.GetComponent<MeshRenderer>().material = Resources.Load(path) as Material;
        }
    }

    private void StartGame()
    {
        var tankTypes = new int[amountOfPlayers];
        var tankNames = new string[amountOfPlayers];
        var tankColors = new int[amountOfPlayers];
        var usedPoints = new List<int>();
        var treesParent = GameObject.Find("Trees").transform;
        var cratesParent = GameObject.Find("Crates").transform;

        GameObject.Find("MapCam").GetComponent<Camera>().enabled = false;
        Vector3 pos;

        GameObject.FindGameObjectsWithTag("StartingPoint")
            .ToList()
            .ForEach(sp => StartingPoints.Add(sp.transform));

        Enumerable.Range(1, amountOfPlayers + 1).ToList().ForEach(playerId =>
        {
            //Initialize Player X
            var startingPoint = GetValidStartingPoint(usedPoints);
            var tankColor     = RandomTankColor();
            var tankObject    = Instantiate(Resources.Load("Tanks/" + RandomTankType().ToString()), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
            var camera        = Instantiate(Camera, Vector3.zero, Quaternion.Euler(0, 0, 0));
            
            var smoothCamera  = camera.GetComponent<SmoothCamera>();
            smoothCamera      = camera.GetComponent<SmoothCamera>();
            smoothCamera.tank = tankObject;

            var camObject     = camera.Find("Camera").GetComponent<Camera>();
            SetCameraViewport(camObject, playerId);

            var canvas                                  = Instantiate(halfScreen, Vector3.zero, Quaternion.Euler(0, 0, 0));
            canvas.GetComponent<Canvas>().planeDistance = 0.5f;
            canvas.GetComponent<Canvas>().worldCamera   = camera.Find("Camera").GetComponent<Camera>();
            canvas.parent                               = tankObject.transform;
            canvas.name                                 = "Canvas";

            var tank      = tankObject.GetComponent<Tank>();
            tank.cam      = camera;
            tank.Name     = tankColor.ToString();
            tank.playerId = playerId;
            tank.Paint(tankColor);
            tank.Controller = new KeyboardController(playerId);

            var position = StartingPoints[startingPoint];
            StartingPoints.Remove(position);
            tank.Spawn(new Vector3(position.position.x, position.eulerAngles.y, position.position.z));
            Players.Add(tank.transform);
        });

        Enumerable.Range(0, treeAmount)
            .ToList()
            .ForEach(_ => SpawnTreeAt(UnityEngine.Random.Range(1, 5), GetValidPosition(), treesParent));

        Enumerable.Range(0, nitroAmount)
            .ToList()
            .ForEach(_ => SpawnNitroAt(GetValidPosition()));

        Enumerable.Range(0, crateAmount)
            .ToList()
            .ForEach(_ => SpawnCrateAt(GetValidPosition(), cratesParent));

        var floor = GameObject.Find("Floor");
        switch (mapType)
        {
            case MapType.Desert:
                floor.GetComponent<Renderer>().material = desertMat;
                foreach (Transform tank in Players)
                {
                    var psL = tank.Find("TracksParticlesLeft").GetComponent<ParticleSystem>();
                    var mainL = psL.main;
                    var colL = psL.colorOverLifetime;
                    var psR = tank.Find("TracksParticlesRight").GetComponent<ParticleSystem>();
                    var mainR = psR.main;
                    var colR = psR.colorOverLifetime;

                    colL.enabled = true;
                    colR.enabled = true;

                    Color c = new Color(244f / 255f, 154f / 255f, 72f / 255f);
                    mainL.startColor = c;
                    mainR.startColor = c;
                    Gradient grad = new Gradient();
                    grad.SetKeys(new GradientColorKey[] { new GradientColorKey(c, 0.0f), new GradientColorKey(new Color(187 / 255, 97 / 255, 15 / 255), 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    colL.color = grad;
                    colR.color = grad;
                }
                break;

            case MapType.Snow:
                floor.GetComponent<Renderer>().material = snowMat;
                foreach (Transform tank in Players)
                {
                    var psL = tank.Find("TracksParticlesLeft").GetComponent<ParticleSystem>();
                    var mainL = psL.main;
                    var colL = psL.colorOverLifetime;
                    var psR = tank.Find("TracksParticlesRight").GetComponent<ParticleSystem>();
                    var mainR = psR.main;
                    var colR = psR.colorOverLifetime;

                    Color c = new Color(233, 233, 233);
                    mainL.startColor = c;
                    mainR.startColor = c;
                    Gradient grad = new Gradient();
                    grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color(200, 200, 200), 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                    colL.color = grad;
                    colR.color = grad;
                }
                break;
            default:
                floor.GetComponent<Renderer>().material = meadowMat;
                break;
        }

        Enumerable.Range(0, rocksAmount).ToList().ForEach(_ => {
            var rock = Instantiate(
                Resources.Load("Rocks/" + mapType.ToString() + "/Large_" + UnityEngine.Random.Range(1, 5)),
                GetFixedPosition(GetValidPosition(), -1.0f),
                Quaternion.Euler(-90,
                UnityEngine.Random.Range(0, 360), 0)) as GameObject;

            rock.transform.parent     = GameObject.Find("Rocks").transform;
            rock.transform.localScale = new Vector3(UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(1, 4));
            Rocks.Add(rock.transform);
        });
    }

    private int GetValidStartingPoint(List<int> usedPoints)
    {
        int position;

        do
            position = UnityEngine.Random.Range(0, StartingPoints.Count);
        while (usedPoints.Contains(position));
        return position;
    }

    private TankColor RandomTankColor() => (TankColor)UnityEngine.Random.Range(0, 8);

    private TankType RandomTankType() => (TankType)UnityEngine.Random.Range(0, 4);


    private void UpdateRanking()
    {
        int[] playerRanks = new int[amountOfPlayers];
        int[] playerKills = new int[amountOfPlayers];

        for (int i = 0; i < amountOfPlayers; i++)
            playerKills[i] = Players[i].GetComponent<Tank>().Kills;

        int range;

        range = amountOfPlayers == 2 ? 3 : 4;

        for (int rank = 1; rank < range; rank++)
        {
            int maxKills = 0;
            int idx = -1;

            for (int i = 0; i < amountOfPlayers; i++)
            {
                if (playerKills[i] > maxKills && playerRanks[i] == 0)
                {
                    maxKills = playerKills[i];
                    idx = i;
                }
            }
            if (idx != -1)
                playerRanks[idx] = rank;
        }

        Transform canvas = transform.Find("GameStats");
        Transform first = canvas.Find("First");
        Transform second = canvas.Find("Second");
        Transform third = canvas.Find("Third");

        canvas.Find("First").GetComponent<Text>().text = "-";
        canvas.Find("Second").GetComponent<Text>().text = "-";
        canvas.Find("Third").GetComponent<Text>().text = "-";

        for (int i = 0; i < amountOfPlayers; i++)
        {
            if (playerRanks[i] == 1)
                first.GetComponent<Text>().text = Players[i].GetComponent<Tank>().Name;
            else if (playerRanks[i] == 2)
                second.GetComponent<Text>().text = Players[i].GetComponent<Tank>().Name;
            else if (playerRanks[i] == 3)
                third.GetComponent<Text>().text = Players[i].GetComponent<Tank>().Name;
        }
    }

    private Vector3 GeneratePosition()
    {
        int offset = 10;
        float x = UnityEngine.Random.Range((-mapSize) + offset, mapSize - offset);
        float z = UnityEngine.Random.Range((-mapSize) + offset, mapSize - offset);
        float r = UnityEngine.Random.Range(0.0f, 360.0f);

        return new Vector3(x, r, z);
    }

    private void SpawnCrateAt(Vector3 pos, Transform parent)
        => Crates.Add(Instantiate(Crate, GetFixedPosition(pos), Quaternion.Euler(0, pos.y, 0), parent));

    private void SpawnNitroAt(Vector3 pos)
        => Nitros.Add(Instantiate(nitro, GetFixedPosition(pos, 1.0f), Quaternion.Euler(0, pos.y, 0)));

    private void SpawnTreeAt(int type, Vector3 pos, Transform parent)
    {
        float size = UnityEngine.Random.Range(0.75f, 1.25f);

        var treeObject = Instantiate(Resources.Load("Trees/" + mapType.ToString() + "/Tree_" + type), GetFixedPosition(pos), Quaternion.Euler(0, pos.y, 0)) as GameObject;
        treeObject.transform.localScale = Vector3.one * size;
        treeObject.transform.parent = parent;

        var tree = treeObject.GetComponent<Obstacles.Tree>();
        tree.initialScaling = size;
        Trees.Add(treeObject.transform);
    }

    private void SupplyDrop()
    {
        int crateDifference = crateAmount - Crates.Count;
        int nitroDifference = nitroAmount - Nitros.Count;

        for (int i = 0; i < crateDifference / 2; i++)
            SpawnCrateAt(GetValidPosition(), GameObject.Find("Crates").transform);

        for (int i = 0; i < nitroDifference / 2; i++)
            SpawnNitroAt(GetValidPosition());
    }

    public void GiveKillToPlayer(int owner)
    {
        if (owner != -1)
            Players[owner - 1].GetComponent<Tank>().GrantKill();
    }

    public Tank GetPlayerByIdx(int idx) 
        => Players[idx - 1].GetComponent<Tank>();

    public void DeleteCrate(Crate c) => Crates.Remove(c.transform);

    public void DeleteTree(Obstacles.Tree t) => Trees.Remove(t.transform);

    public void DeleteNitro(PowerUp n) => Nitros.Remove(n.transform);

    public Vector3 GetFixedPosition(Vector3 pos, float offset = 0.0f) 
        => (Physics.Raycast(new Ray(new Vector3(pos.x, 0.0f, pos.z), Vector3.down), out RaycastHit hit))
            ? new Vector3(hit.point.x, hit.point.y + offset, hit.point.z)
            : pos;

    public static void ShowEndScreen()
    {
        Debug.Log("EndScreen");
        SceneManager.LoadScene("EndScreen");
    }

    public void SetupBorder()
    {
        var parent   = GameObject.Find("Rocks").transform;
        var position = new Vector3(mapSize, -5, 0);

        for (int i = 0; i < this.borderSteps; i++)
        {
            int rotation = UnityEngine.Random.Range(0, 360);
            string objPath = "Rocks/";
            switch (mapType)
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

            position = Quaternion.Euler(0, (float)360 / this.borderSteps, 0) * position;
            var newRock = Instantiate(Resources.Load(objPath), position, Quaternion.Euler(-90, rotation, 0), parent) as GameObject;
            newRock.transform.localScale = new Vector3(UnityEngine.Random.Range(5.0f, 7.0f), UnityEngine.Random.Range(5.0f, 7.0f), UnityEngine.Random.Range(5.0f, 7.0f));
        }
    }

    /// <summary>
    /// todo: Ist komplett broken, bitte fixen
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool IsPositionValid(Vector3 position)
    {
        var fixedPosition = position;
        fixedPosition.y = 0;

        if (Vector3.Distance(Vector3.zero, fixedPosition) > mapSize - 5) return false;

        var blockedPositions = new List<Transform>();

        blockedPositions.AddRange(StartingPoints);

        if (Crates.Count > 0) blockedPositions.AddRange(Crates);
        if (Trees.Count > 0) blockedPositions.AddRange(Trees);
        if (Rocks.Count > 0) blockedPositions.AddRange(Rocks);

        foreach (Transform t in StartingPoints)
            if (Vector3.Distance(fixedPosition, new Vector3(t.position.x, 0, t.position.z)) < startingAreaRadius) return false;

        return true;
    }

    public Vector3 GetSpawnPoint(int padNumber)
    {
        float[] dists = new float[StartingPoints.Count];

        for (int i = 0; i < StartingPoints.Count; i++)
        {
            float dist = float.MaxValue;

            for (int j = 0; j < Players.Count; j++)
            {
                float tmp = Vector3.Distance(Players[j].position, StartingPoints[i].position);
                dist = tmp < dist ? tmp : dist;
            }
            dists[i] = dist;
        }

        int idx = 0;
        float maxDist = float.MinValue;
        for (int i = 0; i < dists.Length; i++)
        {
            if (dists[i] > maxDist)
            {
                maxDist = dists[i];
                idx = i;
            }
        }

        return StartingPoints[idx].position;
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

    //private void SpawnGeneratedTree(int type, Vector3 pos, Transform parent)
    //{
    //    GameObject tree = TreeGenerator.instance.GenerateTree();
    //    tree.transform.parent = parent;
    //    tree.transform.position = GetFixedPosition(pos);
    //    float scale = UnityEngine.Random.Range(0.5f, 1.0f);
    //    tree.transform.localScale = new Vector3(scale, scale, scale);
    //    tree.transform.rotation = Quaternion.Euler(0, pos.y, 0);
    //}
}
