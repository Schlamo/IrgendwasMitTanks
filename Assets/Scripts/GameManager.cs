using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#pragma warning disable 0618 // variable declared but not used.

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public float startingAreaRadius = 5.0f;
    public float supplyRefill       = 15.0f;

    public int borderSteps          = 72;
    public int nitroAmount          = 15;
    public int crateAmount          = 15;
    public int rocksAmount          = 50;
    public int treeAmount           = 150;
    public int mapSize              = 100;
    public int mapType              = 0;
    public int killsToWin;
    public Transform tankCamera;
    public Transform halfScreen;
    public Transform crate;
    public Transform nitro;

    public Material meadowMat; //Type 0
    public Material desertMat; //Type 1
    public Material snowMat;   //Type 2

    private float supplyTimer = 0.0f;
    private float time = 0.0f;

    private int playerAmount = 0;

    private List<Transform> trees           = new List<Transform>();
    private List<Transform> rocks           = new List<Transform>();
    private List<Transform> crates          = new List<Transform>();
    private List<Transform> nitros          = new List<Transform>();
    private List<Transform> players         = new List<Transform>();
    private List<Transform> startingPoints  = new List<Transform>();

    private bool gameStopped = false;

    private int winningTank = -1;
    private int winningColor = -1;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (XInputDotNetPure.GamePad.GetState(XInputDotNetPure.PlayerIndex.One).IsConnected)
        {
            playerAmount = 1;
            Debug.Log("Player One Connected");
        }

        if (XInputDotNetPure.GamePad.GetState(XInputDotNetPure.PlayerIndex.Two).IsConnected)
        {
            playerAmount = 2;
            Debug.Log("Player Two Connected");
        }

        if (XInputDotNetPure.GamePad.GetState(XInputDotNetPure.PlayerIndex.Three).IsConnected)
        {
            playerAmount = 3;
            Debug.Log("Player Three Connected");
        }

        if (XInputDotNetPure.GamePad.GetState(XInputDotNetPure.PlayerIndex.Four).IsConnected)
        {
            playerAmount = 4;
            Debug.Log("Player Four Connected");
        }

        if (playerAmount > 1)
        {
            StartGame();
        }
        else
        {
            Application.Quit();
        }

    }

    void Update()
    {
        if(!gameStopped)
        {
            UpdateRanking();

            supplyTimer += Time.deltaTime;

            if (supplyTimer > supplyRefill)
            {
                SupplyDrop();
                supplyTimer -= supplyRefill;
            }
            if (mapType == 2)
            {
                GameObject.Find("Snow").GetComponent<ParticleSystem>().Emit(25);
            }

            time += Time.deltaTime;
        }
        else
        {
            GameObject camera;
            camera = GameObject.Find("MapCam");

            if (camera.transform.position.y > -350)
            {
                GameObject.Find("LightWrapper").transform.Rotate(-Time.deltaTime, -Time.deltaTime, 0);
                camera.transform.Translate(0, 0, Time.deltaTime * 80);
                camera.GetComponent<Camera>().fieldOfView += Time.deltaTime * 20;
            }
            //GameObject.Find("Floor1").transform.localScale -= new Vector3(Time.deltaTime*25, Time.deltaTime * 25, Time.deltaTime * 25);
        }
    }

    private void setCameraViewport(Camera cam, int player)
    {
        if(playerAmount == 2)
        {
            if (player == 1)
            {
                cam.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
            }
            else if (player == 2)
            {
                cam.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
            }
        }
        if (playerAmount == 3)
        {
            if (player == 1)
            {
                cam.rect = new Rect(0.25f, 0.5f, 0.5f, 0.5f);
            }
            else if (player == 2)
            {
                cam.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            }
            else if (player == 3)
            {
                cam.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
            }
        }
        if (playerAmount == 4)
        {
            if (player == 1)
            {
                cam.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            }
            else if (player == 2)
            {
                cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else if (player == 3)
            {
                cam.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            }
            else if (player == 4)
            {
                cam.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
            }
        }
    }

    private void StopGame()
    {
        GameObject.Find("MapCam").GetComponent<Camera>().enabled = true;

        gameStopped = true;
        Debug.Log("StopGame");
        Destroy(GameObject.Find("Trees"));
        Destroy(GameObject.Find("Rocks"));
        Destroy(GameObject.Find("Crates"));
        Destroy(GameObject.Find("Grass"));
        Destroy(GameObject.Find("Snow"));
        GameObject.Find("Tanks").SetActive(false);
        GameObject.Find("GameStats").SetActive(false);

        XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, 0.0f, 0.0f);
        XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.Two, 0.0f, 0.0f);
        XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.Three, 0.0f, 0.0f);
        XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.Four, 0.0f, 0.0f);
        string path = "Tanks/";

        switch (winningTank)
        {
            case 0:
                path += "Tempest";
                break;
            case 1:
                path += "Viking";
                break;
            case 2:
                path += "Prometheus";
                break;
            case 3:
                path += "Reaper";
                break;
            case 4:
                path += "Philipp";
                break;
        }
        Debug.Log(winningTank);
        GameObject tank = Instantiate(Resources.Load(path), new Vector3(0.0f,-364.17f, 8.32f), Quaternion.Euler(0, 0, 0)) as GameObject;
        GameObject tankSlot= GameObject.Find("WinningTank");
        tank.transform.parent = tankSlot.transform;
        tank.AddComponent<WinningTank>();
        tank.GetComponent<WinningTank>().Paint(winningColor);
        Destroy(tank.GetComponent<Rigidbody>());
        Destroy(tank.GetComponent<Tank>());
    }

    private void StartGame()
    {
        Vector3 pos;
        List<int> usedPoints = new List<int>();
        int[] tankTypes = new int[playerAmount];
        int[] tankColors = new int[playerAmount];
        string[] tankNames = new string[playerAmount];
        List<int> colors = new List<int>();

        if(playerAmount == 3)
        {
            GameObject stats = GameObject.Find("GameStats");
            foreach(Transform child in stats.transform)
            {
                child.Translate(new Vector3(0, 140, 0));
            }
        }

        Transform treesParent = GameObject.Find("Trees").transform;
        Transform cratesParent = GameObject.Find("Crates").transform;
        GameObject.Find("MapCam").GetComponent<Camera>().enabled = false;

        if(mapType > 2 || mapType < 0)
            mapType = Random.Range(0, 3);

        GenerateBorder(borderSteps);
        
        int grassDensity = 500;

        Transform light = GameObject.Find("LightWrapper").transform;

        switch (mapType)
        {
            case 0:
                grassDensity *= 2;
                light.rotation = Quaternion.Euler(60, 60, 0);
                break;
            case 1:
                light.rotation = Quaternion.Euler(80, 80, 0);
                grassDensity /= 5;
                break;
            case 2:
                light.rotation = Quaternion.Euler(44, 44, 0);
                grassDensity /= 10;
                break;
        }
        for (int i = 0; i < grassDensity; i++)
        {
            Vector3 position;
            do
            {
                position = GeneratePosition();
            }
            while (!IsPositionValid(position));

            GameObject grass = Instantiate(Resources.Load("Grass"), GetFixedPosition(position), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
            grass.transform.localScale = new Vector3(Random.Range(1.0f, 2), Random.Range(1.0f, 2), Random.Range(1.0f, 2));
            string path = "Grass/";
            switch(mapType)
            {
                case 0:
                    path += "Meadow";
                    break;
                case 1:
                    path += "Desert";
                    break;
                case 2:
                    path += "Snow";
                    break;
            }
            grass.transform.GetComponent<MeshRenderer>().material = Resources.Load(path) as Material;
            grass.transform.parent = GameObject.Find("Grass").transform;
        } 

        for (int i = 0; i < 8; i++)
        {
            colors.Add(i);
        }

        for (int i = 0; i < GameObject.Find("StartingPoints").transform.childCount; i++)
        {
            startingPoints.Add(GameObject.Find("StartingPoints").transform.GetChild(i));
        }

        for (int i = 0; i < playerAmount; i++)
        {
            tankTypes[i] = Random.Range(0, 5);
            tankColors[i] = colors[Random.Range(0, colors.Count)];
            colors.Remove(tankColors[i]);

            switch (tankColors[i])
            {
                case 0:
                    tankNames[i] = "Red";
                    break;
                case 1:
                    tankNames[i] = "Blue";
                    break;
                case 2:
                    tankNames[i] = "Green";
                    break;
                case 3:
                    tankNames[i] = "Yellow";
                    break;
                case 4:
                    tankNames[i] = "Pink";
                    break;
                case 5:
                    tankNames[i] = "Orange";
                    break;
                case 6:
                    tankNames[i] = "Black";
                    break;
                case 7:
                    tankNames[i] = "White";
                    break;
            }
        }

        for (int i = 0; i < playerAmount; i++)
        {
            GameObject tank;
            Transform cam;
            Transform canvas;
            Tank t;
            SmoothCamera c;

            int position;

            do
            {
                position = Random.Range(0, startingPoints.Count);
            }
            while (usedPoints.Contains(position));

            usedPoints.Add(position);

            Transform t_pos = startingPoints[position];
            Vector3 v_pos = new Vector3(t_pos.position.x, t_pos.eulerAngles.y, t_pos.position.z);

            string path = "Tanks/";

            switch (tankTypes[i])
            {
                case 0:
                    path += "Tempest";
                    break;
                case 1:
                    path += "Viking";
                    break;
                case 2:
                    path += "Prometheus";
                    break;
                case 3:
                    path += "Reaper";
                    break;
                case 4:
                    path += "Philipp";
                    break;
            }

            tank = Instantiate(Resources.Load(path), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
            cam = Instantiate(tankCamera, Vector3.zero, Quaternion.Euler(0, 0, 0));

            canvas = Instantiate(halfScreen, Vector3.zero, Quaternion.Euler(0, 0, 0));
            canvas.GetComponent<Canvas>().worldCamera = cam.Find("Camera").GetComponent<Camera>();

            canvas.parent = tank.transform;
            canvas.name = "Canvas";
            canvas.GetComponent<Canvas>().planeDistance = 0.5f;

            t = tank.GetComponent<Tank>();
            c = cam.GetComponent<SmoothCamera>();
            t.cam = cam;
            c.tank = tank;

            t.padNumber = i + 1;
            Camera camera = cam.Find("Camera").GetComponent<Camera>();
            setCameraViewport(camera, i + 1);
            t.Spawn(v_pos);
            t.Paint(tankColors[i]);
            t.Type = tankTypes[i];
            players.Add(tank.transform);
            t.Name = tankNames[i];
            tank.transform.parent = GameObject.Find("Tanks").transform;
            cam.transform.parent = GameObject.Find("Tanks").transform;
        }

        for (int i = 0; i < treeAmount; i++)
        {
            do
            {
                pos = GeneratePosition();
            }
            while (!IsPositionValid(pos));

            int type = Random.Range(2, 5);
			if (mapType == 0) {
				int t = Random.Range (0, 5);

				if (t == 0) {
					SpawnGeneratedTree (type, pos, treesParent);
				} else {
					SpawnTreeAt (type, pos, treesParent);
				}
			} else {
				SpawnTreeAt (type, pos, treesParent);
			}
        }

        for (int i = 0; i < nitroAmount; i++)
        {
            do
            {
                pos = GeneratePosition();
            }
            while (!IsPositionValid(pos));

            SpawnNitroAt(pos);
        }

        for (int i = 0; i < crateAmount; i++)
        {
            do
            {
                pos = GeneratePosition();
            }
            while (!IsPositionValid(pos));

            SpawnCrateAt(pos, cratesParent);
        }

        var floor = GameObject.Find("Floor1");
        switch (mapType)
        {
            case 1:
                floor.GetComponent<Renderer>().material = desertMat;
                foreach (Transform tank in players)
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

            case 2:
                floor.GetComponent<Renderer>().material = snowMat;
                foreach (Transform tank in players)
                {
                    var psL = tank.Find("TracksParticlesLeft").GetComponent<ParticleSystem>();
                    var mainL = psL.main;
                    var colL = psL.colorOverLifetime;
                    var psR = tank.Find("TracksParticlesRight").GetComponent<ParticleSystem>();
                    var mainR = psR.main;
                    var colR = psR.colorOverLifetime;

                    Color c = new Color(255, 255, 255);
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

        for (int i = 0; i < rocksAmount; i++)
        {
            do
            {
                pos = GeneratePosition();
            }
            while (!IsPositionValid(pos));

            int type = Random.Range(1, 5);
            int rotation = Random.Range(0, 360);

            string path = "";

            switch (mapType)
            {
                case 1:
                    path = "Rocks/Desert/Large_";
                    break;
                case 2:
                    path = "Rocks/Snow/Large_";
                    break;
                default:
                    path = "Rocks/Meadow/Large_";
                    break;
            }

            path += type;

            GameObject rock = Instantiate(Resources.Load(path), GetFixedPosition(pos, -1.0f), Quaternion.Euler(-90, rotation, 0)) as GameObject;
            rock.transform.parent = GameObject.Find("Rocks").transform;
            rocks.Add(rock.transform);
            rock.transform.localScale = new Vector3(Random.Range(1, 4), Random.Range(1, 4), Random.Range(1, 4));
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        Vector3 fixedPosition = position;
        fixedPosition.y = 0;

        if (Vector3.Distance(Vector3.zero, fixedPosition) > mapSize - 5)
        {
            return false;
        }

        foreach (Transform t in startingPoints)
        {
            if (Vector3.Distance(fixedPosition, new Vector3(t.position.x, 0, t.position.z)) < startingAreaRadius)
            {
                return false;
            }
        }

        foreach (Transform c in crates)
        {
            if (Vector3.Distance(fixedPosition, new Vector3(c.position.x, 0, c.position.z)) < 5.0f)
            {
                return false;
            }
        }

        foreach (Transform t in trees)
        {
            if (Vector3.Distance(fixedPosition, new Vector3(t.position.x, 0, t.position.z)) < 5.0f)
            {
                return false;
            }
        }

        foreach (Transform t in rocks)
        {
            if (Vector3.Distance(fixedPosition, new Vector3(t.position.x, 0, t.position.z)) < 5.0f)
            {
                return false;
            }
        }

        return true;
    }

    private void UpdateRanking()
    {
        int[] playerRanks = new int[playerAmount];
        int[] playerKills = new int[playerAmount];

        for (int i = 0; i < playerAmount; i++)
        {
            playerKills[i] = players[i].GetComponent<Tank>().Kills;
            if(playerKills[i] >= killsToWin)
            {
                winningColor =  players[i].GetComponent<Tank>().Color;
                winningTank = players[i].GetComponent<Tank>().Type;
                StopGame();
            }
        }

        int range;

        range = playerAmount == 2 ? 3 : 4;

        for (int rank = 1; rank < range; rank++)
        {
            int maxKills = 0;
            int idx = -1;

            for (int i = 0; i < playerAmount; i++)
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

        Transform canvas  = transform.Find("GameStats");
        Transform first   = canvas.Find("First");
        Transform second  = canvas.Find("Second");
        Transform third   = canvas.Find("Third");

        canvas.Find("First").GetComponent<Text>().text = "-";
        canvas.Find("Second").GetComponent<Text>().text = "-";
        canvas.Find("Third").GetComponent<Text>().text = "-";

        for (int i = 0; i < playerAmount; i++)
        {
            if (playerRanks[i] == 1)
            {
                first.GetComponent<Text>().text = players[i].GetComponent<Tank>().Name;
            }
            else if (playerRanks[i] == 2)
            {
                second.GetComponent<Text>().text = players[i].GetComponent<Tank>().Name;
            }
            else if (playerRanks[i] == 3)
            {
                third.GetComponent<Text>().text = players[i].GetComponent<Tank>().Name;
            }
        }
    }

    private Vector3 GeneratePosition()
    {
        int offset = 10;
        float x = Random.Range((-mapSize) + offset, mapSize - offset);
        float z = Random.Range((-mapSize) + offset, mapSize - offset);
        float r = Random.Range(0.0f, 360.0f);

        return new Vector3(x, r, z);
    }

    private void SpawnCrateAt(Vector3 pos, Transform parent)
    {
        var t_crate = Instantiate(crate, GetFixedPosition(pos, 1.5f), Quaternion.Euler(0, pos.y, 0));

        t_crate.parent = parent;
        crates.Add(t_crate);
    }

    private void SpawnNitroAt(Vector3 pos)
    {
        var t_nitro = Instantiate(nitro, GetFixedPosition(pos, 1.0f), Quaternion.Euler(0, pos.y, 0));
        t_nitro.parent = GameObject.Find("PowerUps").transform;
        nitros.Add(t_nitro);
    }

    private void SpawnGeneratedTree(int type, Vector3 pos, Transform parent)
    {
        GameObject tree = TreeGenerator.instance.GenerateTree();
        tree.transform.parent = parent;
        tree.transform.position = GetFixedPosition(pos);
        float scale = Random.Range(0.5f, 1.0f);
        tree.transform.localScale = new Vector3(scale, scale, scale);
        tree.transform.rotation = Quaternion.Euler(0, pos.y, 0);
    }

    private void SpawnTreeAt(int type, Vector3 pos, Transform parent)
    {
        string path = "Trees/";
        float size = Random.Range(0.75f, 1.25f);

        switch (mapType)
        {
            case 0:
                path += "Meadow/Tree_" + type;
                break;
            case 1:
                path += "Desert/Tree_" + type;
                break;
            case 2:
                path += "Snow/Tree_" + type;
                break;
        }
        var tree = Instantiate(Resources.Load(path), GetFixedPosition(pos), Quaternion.Euler(0, pos.y, 0)) as GameObject;

        tree.transform.localScale = new Vector3(size, size, size);
        tree.transform.parent = parent;
        trees.Add(tree.transform);
    }

    private void SupplyDrop()
    {
        int crateDifference = crateAmount - crates.Count;
        int nitroDifference = nitroAmount - nitros.Count;

        for (int i = 0; i < crateDifference / 2; i++)
        {
            Vector3 pos;
            do
            {
                pos = GeneratePosition();
            }
            while (!IsPositionValid(pos));
            SpawnCrateAt(pos, GameObject.Find("Crates").transform);
        }

        for (int i = 0; i < nitroDifference / 2; i++)
        {
            Vector3 pos;
            do
            {
                pos = GeneratePosition();
            } while (!IsPositionValid(pos));

            SpawnNitroAt(pos);
        }
    }

    public Vector3 GetSpawnPoint(int padNumber)
    {
        float[] dists = new float[startingPoints.Count];

        for(int i = 0; i < startingPoints.Count; i++)
        {
            float dist = float.MaxValue;

            for (int j = 0; j < players.Count; j++)
            {
                float tmp = Vector3.Distance(players[j].position, startingPoints[i].position);
                dist = tmp < dist ? tmp : dist;
            }
            dists[i] = dist;
        }

        int idx = 0;
        float maxDist = float.MinValue;
        for(int i = 0; i < dists.Length; i++)
        {
            if(dists[i] > maxDist)
            {
                maxDist = dists[i];
                idx = i;
            } 
        }

        return startingPoints[idx].position;
    }

    public void GiveKillToPlayer(int owner)
    {
        if(owner != -1)
        {
            var tank = players[owner-1].GetComponent<Tank>();
            tank.Kills++;
        }
    }

    public Tank GetPlayerByIdx(int idx)
    {
        return players[idx - 1].GetComponent<Tank>();
    }

    public void DeleteCrate(Crate c)
    {
        this.crates.Remove(c.transform);
    }

    public void DeleteTree(Tree t)
    {
        this.trees.Remove(t.transform);
    }

    public void DeleteNitro(PowerUp n)
    {
        this.nitros.Remove(n.transform);
    }

    public Vector3 GetFixedPosition(Vector3 pos, float offset = 0.0f)
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(pos.x, 0.0f, pos.z), Vector3.down);

        if (Physics.Raycast(ray, out hit))
        {
            return new Vector3(hit.point.x, hit.point.y +offset, hit.point.z);
        }
        return pos;
    }

    public static void ShowEndScreen()
    {
        Debug.Log("EndScreen");
        SceneManager.LoadScene("EndScreen");
    }

    public void GenerateBorder(int steps)
    {
        Vector3 pos = new Vector3(mapSize, -5, 0);

        for (int i = 0; i < steps; i++)
        {
            int rotation = Random.Range(0, 360);
            string path = "Rocks/";
            switch(mapType)
            {
                case 0:
                    path += "Meadow/Large_";
                    break;
                case 1:
                    path += "Desert/Large_";
                    break;
                case 2:
                    path += "Snow/Large_";
                    break;
            }
            path += Random.Range(1, 5);

            pos = Quaternion.Euler(0, (float)360 / steps, 0) * pos;
            var rock = Instantiate(Resources.Load(path), pos, Quaternion.Euler(-90, rotation, 0)) as GameObject;
            rock.transform.localScale = new Vector3(Random.Range(5.0f, 7.0f), Random.Range(5.0f, 7.0f), Random.Range(5.0f, 7.0f));
            rock.transform.parent = GameObject.Find("Rocks").transform;
        }
    }

}
