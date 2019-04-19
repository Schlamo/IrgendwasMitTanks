using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Enumerators;
using Tanks;
using Obstacles;
using Controllers;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public GameSettings Settings { get; set; }

<<<<<<< HEAD

=======
    public int borderSteps          = 72;
    public int nitroAmount          = 15;
    public int crateAmount          = 15;
    public int rocksAmount          = 50;
    public int treeAmount           = 150;
    public int mapSize              = 100;
    public int mapType              = 0;
    public int killsToWin;
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c
    public Transform tankCamera;
    public Transform halfScreen;

    public Material meadowMat; //Type 0
    public Material desertMat; //Type 1
    public Material snowMat;   //Type 2

<<<<<<< HEAD
    private List<Transform> players         = new List<Transform>();
    private List<Transform> startingPoints  = new List<Transform>();

    #region MonoBehaviour Methods
    void Awake() {
        if (Application.isEditor)
            Application.runInBackground = true;
=======
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

>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

<<<<<<< HEAD
    void Start() {
        //Reads game settings from the settings.json file
        Initialize();

        //Fills the map with obstacles 
        SetupMap();

        //
        StartGame();
    }

    void Update() {
        UpdateRanking();

        Settings.supplyTimer += Time.deltaTime;

        if (Settings.supplyTimer > Settings.supplyRefill) {
            SupplyDrop();
            Settings.supplyTimer -= Settings.supplyRefill;
        }

        if (Settings.biome == MapType.Snow)
            GameObject.Find("Snow").GetComponent<ParticleSystem>().Emit(Settings.snowIntensity);
=======
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
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
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
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c

        //GameObject.Find("LightWrapper").transform.RotateAround(Vector3.zero, Vector3.forward, 20 * Time.deltaTime);
    }
    #endregion

    #region Private Methods
    private void setCameraViewport(Camera camera, int id) {
        if(Settings.playerCount == 2) {
            if (id == 1)
                camera.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
            else if (id == 2)
                camera.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        }
        else if (Settings.playerCount == 3) {
            if (id == 1)
                camera.rect = new Rect(0.25f, 0.5f, 0.5f, 0.5f);
            else if (id == 2)
                camera.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            else if (id == 3)
                camera.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
        else if (Settings.playerCount == 4) {
            if (id == 1)
                camera.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            else if (id == 2)
                camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            else if (id == 3)
                camera.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            else if (id == 4)
                camera.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
    }

<<<<<<< HEAD
    private void Initialize() {
        try {
            string jsonSettings = System.IO.File.ReadAllText(Application.dataPath + "/settings.json");
            Settings = JsonUtility.FromJson<GameSettings>(jsonSettings);
        }
        catch (System.Exception) { }

        if (Settings.biome == MapType.Undefined)
            RandomizeMapType();
    }

    private void RandomizeMapType() {
        Settings.biome = (MapType)Random.Range(0, 3);
    }

    private void StartGame() {
        Vector3   pos;
        string[]  tankNames  = new string[Settings.playerCount];
        int[]     tankColors = new int[Settings.playerCount];
        int[]     tankTypes  = new int[Settings.playerCount];
        List<int> usedPoints = new List<int>();
        List<int> colors     = new List<int>();
        
        GameObject.Find("MapCam").GetComponent<Camera>().enabled = false;

        tankTypes = new int[] { 0, 4 };
=======
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
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c

        for (int i = 0; i < 8; i++)
            colors.Add(i);

<<<<<<< HEAD
        for (int i = 0; i < this.Settings.playerCount; i++) {
            tankTypes[i] = Random.Range(0, 4);
=======
        for (int i = 0; i < playerAmount; i++)
        {
            tankTypes[i] = Random.Range(0, 5);
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c
            tankColors[i] = colors[Random.Range(0, colors.Count)];
            colors.Remove(tankColors[i]);

            switch (tankColors[i]) {
                case 0: tankNames[i] = "Red";    break;
                case 1: tankNames[i] = "Blue";   break;
                case 2: tankNames[i] = "Green";  break;
                case 3: tankNames[i] = "Yellow"; break;
                case 4: tankNames[i] = "Pink";   break;
                case 5: tankNames[i] = "Orange"; break;
                case 6: tankNames[i] = "Black";  break;
                case 7: tankNames[i] = "White";  break;
            }
        }

        for (int i = 0; i < Settings.playerCount; i++) {
            SmoothCamera c;
            GameObject   tank;
            Transform    cam;
            Transform    canvas;
            Tank         t;
            int          position;

            do 
                position = Random.Range(0, startingPoints.Count);
            while (usedPoints.Contains(position));

            usedPoints.Add(position);

            Transform t_pos = startingPoints[position];
            Vector3 v_pos = new Vector3(t_pos.position.x, t_pos.eulerAngles.y, t_pos.position.z);

            string path = "Tanks/";

<<<<<<< HEAD
            switch (tankTypes[i]) {
                case 0: path += "Tempest";    break;
                case 1: path += "Viking";     break;
                case 2: path += "Prometheus"; break;
                case 3: path += "Reaper";     break;
                case 4: path += "Cthulu";     break;
=======
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
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c
            }

            tank = Instantiate(Resources.Load(path), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
            cam = Instantiate(tankCamera, Vector3.zero, Quaternion.Euler(0, 0, 0));

            canvas = Instantiate(halfScreen, Vector3.zero, Quaternion.Euler(0, 0, 0));
            canvas.GetComponent<Canvas>().worldCamera = cam.Find("Camera").GetComponent<Camera>();

            canvas.SetParent(tank.transform);
            canvas.name = "Canvas";
            canvas.GetComponent<Canvas>().planeDistance = 0.5f;

            t = tank.GetComponent<Tank>();
            c = cam.GetComponent<SmoothCamera>();
            t.cam = cam;
            c.tank = tank;

            t.padNumber = i + 1;

            if (Settings.useKeyboard)
                t.controller = new KeyboardController() { Index = t.padNumber };
            else
                t.controller = new KeyboardController() { Index = t.padNumber };

            Camera camera = cam.Find("Camera").GetComponent<Camera>();
            setCameraViewport(camera, i + 1);
            t.Spawn(v_pos);
            t.Paint(tankColors[i]);
            t.Type = tankTypes[i];
            players.Add(tank.transform);
<<<<<<< HEAD
            t.name = tankNames[i];
        }

        switch (Settings.biome) {
            case MapType.Desert:
                foreach (Transform tank in players) {
                    Gradient       grad  = new Gradient();
                    Color          c     = new Color(244f / 255f, 154f / 255f, 72f / 255f);
                    ParticleSystem psL   = tank.Find("TracksParticlesLeft").GetComponent<ParticleSystem>();
                    ParticleSystem psR   = tank.Find("TracksParticlesRight").GetComponent<ParticleSystem>();
                    var            mainL = psL.main;
                    var            mainR = psR.main;
                    var            colL  = psL.colorOverLifetime;
                    var            colR  = psR.colorOverLifetime;

                    grad.SetKeys(
                        new GradientColorKey[] {
                            new GradientColorKey(c, 0.0f),
                            new GradientColorKey(new Color(187 / 255, 97 / 255, 15 / 255), 1.0f)
                        },
                        new GradientAlphaKey[] {
                            new GradientAlphaKey(1.0f, 0.0f),
                            new GradientAlphaKey(0.0f, 1.0f)
                        }
                    );
=======
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
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c

                    colL.enabled = true;
                    mainL.startColor = c;
                    colL.color = grad;

                    colR.enabled = true;
                    mainR.startColor = c;
                    colR.color = grad;
                }
                break;

            case MapType.Snow:
                foreach (Transform tank in players) {
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
        }
    }

    private void SetupGrass() {
        int       density = 100;
        string    path    = "Grass/" + GetMapTypeAsString();
        Transform root    = GameObject.Find("Grass").transform;

        for (int i = 0; i < density; i++) {
            Vector3    position = GenerateValidPosition();
            GameObject grass    = Instantiate(Resources.Load("Grass"), GetFixedPosition(position), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;

            grass.transform.localScale = new Vector3(Random.Range(1.0f, 2), Random.Range(1.0f, 2), Random.Range(1.0f, 2));
            grass.transform.GetComponent<MeshRenderer>().material = Resources.Load(path) as Material;
            grass.transform.parent = root;
        }
    }

    private void SetupLighting() {
        Transform light = GameObject.Find("LightWrapper").transform;

        switch (Settings.biome) {
            case MapType.Meadow: light.rotation = Quaternion.Euler(60, 60, 0); break;
            case MapType.Desert: light.rotation = Quaternion.Euler(75, 75, 0); break;
            case MapType.Snow:
                light.rotation = Quaternion.Euler(45, 45, 0);
                light.GetChild(0).GetComponent<Light>().intensity = 0.5f;
                break;
        }
    }

    private void SetupMap() {
        GameObject root = GameObject.Find("Loot");
        GameObject floor = GameObject.Find("Floor");

        foreach (Transform sp in GameObject.Find("StartingPoints").transform)
            startingPoints.Add(sp);

        SetupBorder();
        SetupGrass();
        SetupTrees();
        SetupRocks();
        SetupLighting();
        
        switch(Settings.biome) {
            case MapType.Meadow: floor.GetComponent<Renderer>().material = meadowMat; break;
            case MapType.Desert: floor.GetComponent<Renderer>().material = desertMat; break;
            case MapType.Snow:   floor.GetComponent<Renderer>().material = snowMat;   break;
        }

        for (int i = 0; i < this.Settings.nitroCount; i++)
            SpawnNitroAt(GenerateValidPosition(), root.transform);

        for (int i = 0; i < this.Settings.crateCount; i++)
            SpawnCrateAt(GenerateValidPosition(), root.transform);
    }

    private void SetupTrees() {
        for (int i = 0; i < this.Settings.trees; i++)
            SpawnTreeAt(GenerateValidPosition(), GameObject.Find("Obstacles").transform);
    }

    private void SetupRocks() {
        for (int i = 0; i < this.Settings.rocks; i++)
            SpawnRockAt(GenerateValidPosition(), GameObject.Find("Obstacles").transform);
    }

    private void SpawnCrateAt(Vector3 pos, Transform parent) {
        (Instantiate(Resources.Load("Obstacles/Crate"), GetFixedPosition(pos, 1.5f), Quaternion.Euler(0, pos.y, 0)) as GameObject).transform.parent = parent;
    }

    private void SpawnNitroAt(Vector3 pos, Transform parent) {
        (Instantiate(Resources.Load("PowerUp/Nitro"), GetFixedPosition(pos, 1.0f), Quaternion.Euler(0, pos.y, 0)) as GameObject).transform.parent = parent;
    }

    private void SpawnTreeAt(Vector3 pos, Transform root) {
        float  size = Random.Range(1.0f, 2.5f);
        var    tree = Instantiate(Resources.Load("Trees/" + GetMapTypeAsString() + "/Tree_" + Random.Range(2, 5)), GetFixedPosition(pos), Quaternion.Euler(0, pos.y, 0)) as GameObject;

        tree.GetComponent<Obstacles.Tree>().initialScaling = size;
        tree.GetComponent<Obstacles.Tree>().initialHP *= size;
        tree.transform.localScale = new Vector3(size, size, size);
        tree.transform.parent = root;
    }

    private void SpawnRockAt(Vector3 pos, Transform root) {
        string     path = "Rocks/" + this.GetMapTypeAsString() + "/";
        float      size = Random.Range(2.5f, 5.0f);
        int        type = Random.Range(1, 5);
        
        switch(Random.Range(1,5)) {
            case 1: path += "Small_"; break;
            case 2: path += "Tall_"; break;
            case 3: path += "Medium_"; break;
            case 4: path += "Medium_Flat_"; break;
            case 5: path += "Large_"; break;
        }
        path += type;

        GameObject rock = Instantiate(Resources.Load(path), GetFixedPosition(pos), Quaternion.Euler(-90, pos.y, 0)) as GameObject;
        
        rock.transform.localScale = new Vector3(size, size, size);
        rock.transform.parent = root;
    }

    /*private void SpawnGeneratedTreeAt(Vector3 pos, Transform parent)
    {
        GameObject tree = TreeGenerator.instance.GenerateTree();
        tree.transform.parent = parent;
        tree.transform.position = GetFixedPosition(pos);
        float scale = Random.Range(0.5f, 1.0f);
        tree.transform.localScale = new Vector3(scale, scale, scale);
        tree.transform.rotation = Quaternion.Euler(0, pos.y, 0);
    }*/

    private Vector3 GeneratePosition() {
        int   offset = 10;
        float x      = Random.Range((-Settings.mapSize) + offset, Settings.mapSize - offset);
        float z      = Random.Range((-Settings.mapSize) + offset, Settings.mapSize - offset);
        float r      = Random.Range(0.0f, 360.0f);

        return new Vector3(x, r, z);
    }

    private Vector3 GetFixedPosition(Vector3 pos, float offset = 0.0f) {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(pos.x, 0.0f, pos.z), Vector3.down);

        if (Physics.Raycast(ray, out hit))
            return new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);

        return pos;
    }

    private void SupplyDrop() {
        int crateDifference = Settings.crateCount;
        int nitroDifference = Settings.nitroCount;
        Transform root      = GameObject.Find("Loot").transform;

        foreach (Transform loot in root)
        {
<<<<<<< HEAD
            if (loot.name.Contains("Crate"))
                crateDifference--;
            else if (loot.name.Contains("Nitro"))
                nitroDifference--;
        }

        for (int i = 0; i < 10 / 2; i++)
            SpawnCrateAt(GenerateValidPosition(), root);

        for (int i = 0; i < 5 / 2; i++)
            SpawnNitroAt(GenerateValidPosition(), root);
    }

    private bool IsPositionValid(Vector3 position) {
        Vector3 fixedPosition = position;
        fixedPosition.y = 0;

        if (Vector3.Distance(Vector3.zero, fixedPosition) > Settings.mapSize - 5)
            return false;

        List<Transform> blockedPositions = new List<Transform>();

        blockedPositions.AddRange(startingPoints);

        if (GameObject.Find("Obstacles").transform.childCount > 0) {
            foreach (Transform obstacle in GameObject.Find("Obstacles").transform)
                if (Vector3.Distance(fixedPosition, new Vector3(obstacle.position.x, 0, obstacle.position.z)) < Settings.startingRadius)
                    return false;
=======
            playerKills[i] = players[i].GetComponent<Tank>().Kills;
            if(playerKills[i] >= killsToWin)
            {
                winningColor =  players[i].GetComponent<Tank>().Color;
                winningTank = players[i].GetComponent<Tank>().Type;
                StopGame();
            }
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c
        }

        foreach (Transform t in startingPoints)
            if (Vector3.Distance(fixedPosition, new Vector3(t.position.x, 0, t.position.z)) < Settings.startingRadius)
                return false;
        
        return true;
    }

    private void UpdateRanking() {
        int[] playerRanks = new int[Settings.playerCount];
        int[] playerKills = new int[Settings.playerCount];
        int range         = Settings.playerCount == 2 ? 3 : 4;

        for (int i = 0; i < this.Settings.playerCount; i++)
            playerKills[i] = players[i].GetComponent<Tank>().kills;

        for (int rank = 1; rank < range; rank++) {
            int maxKills = 0;
            int idx      = -1;

            for (int i = 0; i < this.Settings.playerCount; i++) {
                if (playerKills[i] > maxKills && playerRanks[i] == 0) {
                    maxKills = playerKills[i];
                    idx = i;
                }
            }

            if (idx != -1)
                playerRanks[idx] = rank;
        }

        Transform canvas = transform.Find("GameStats");
        Transform first  = canvas.Find("First");
        Transform second = canvas.Find("Second");
        Transform third  = canvas.Find("Third");

        canvas.Find("First").GetComponent<Text>().text = "-";
        canvas.Find("Second").GetComponent<Text>().text = "-";
        canvas.Find("Third").GetComponent<Text>().text = "-";

        for (int i = 0; i < this.Settings.playerCount; i++) {
            if (playerRanks[i] == 1)
                first.GetComponent<Text>().text = players[i].GetComponent<Tank>().name;
            else if (playerRanks[i] == 2)
                second.GetComponent<Text>().text = players[i].GetComponent<Tank>().name;
            else if (playerRanks[i] == 3)
                third.GetComponent<Text>().text = players[i].GetComponent<Tank>().name;
        }
    }

<<<<<<< HEAD
    private Vector3 GenerateValidPosition() {
        Vector3 pos;

        do
            pos = GeneratePosition();
        while (!IsPositionValid(pos));
=======
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
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c

        return pos;
    }

    private string GetMapTypeAsString() {
        switch(Settings.biome) {
            case MapType.Meadow: return "Meadow";    break;
            case MapType.Desert: return "Desert";    break;
            case MapType.Snow:   return "Snow";      break;
            default:             return "Undefined";
        }
    }

    #endregion

    #region Public Methods
    public Vector3 GetSpawnPoint(int padNumber) {
        float[] dists = new float[startingPoints.Count];
        int idx = 0;
        float maxDist = float.MinValue;

        for (int i = 0; i < startingPoints.Count; i++) {
            float dist = float.MaxValue;

            for (int j = 0; j < players.Count; j++) {
                float tmp = Vector3.Distance(players[j].position, startingPoints[i].position);
                dist = tmp < dist ? tmp : dist;
            }

            dists[i] = dist;
        }

        
        for (int i = 0; i < dists.Length; i++) {
            if (dists[i] > maxDist) {
                maxDist = dists[i];
                idx = i;
            }
        }

        return startingPoints[idx].position;
    }

    public void GiveKillToPlayer(int owner) {
        if(owner != -1)
            players[owner-1].GetComponent<Tank>().kills++;
    }

    public Tank GetPlayerByIdx(int idx) {
        return players[idx - 1].GetComponent<Tank>();
    }

    public void SetupBorder() {
        Vector3 position = new Vector3(Settings.mapSize, -5, 0);
        Transform root = GameObject.Find("Obstacles").transform;

        for (int i = 0; i < Settings.borderSteps; i++) {
            position = Quaternion.Euler(0, (float)360 / Settings.borderSteps, 0) * position;

            var newRock = Instantiate(Resources.Load("Rocks/" + GetMapTypeAsString() + "/Large_" + Random.Range(1, 5)), position, Quaternion.Euler(-90, Random.Range(0, 360), 0)) as GameObject;
            newRock.transform.localScale = new Vector3(Random.Range(5.0f, 7.0f), Random.Range(5.0f, 7.0f), Random.Range(5.0f, 7.0f));
            newRock.transform.parent = root;
        }
    }

    public static void CreateLoot(Transform t) {
        Debug.Log((PowerUpType)Random.Range(0, 4));

<<<<<<< HEAD
        Instantiate(Resources.Load(
            string.Format("PowerUp/{0}", (PowerUpType)Random.Range(0, 4))), 
            t.position, 
            t.rotation
        );
=======
            pos = Quaternion.Euler(0, (float)360 / steps, 0) * pos;
            var rock = Instantiate(Resources.Load(path), pos, Quaternion.Euler(-90, rotation, 0)) as GameObject;
            rock.transform.localScale = new Vector3(Random.Range(5.0f, 7.0f), Random.Range(5.0f, 7.0f), Random.Range(5.0f, 7.0f));
            rock.transform.parent = GameObject.Find("Rocks").transform;
        }
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c
    }
    #endregion
}
