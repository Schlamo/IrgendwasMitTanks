using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private List<Transform> players = new List<Transform>();

    public int playerAmount;

    public int mapSize;
    public Transform tree;
    public Transform tree2;
    public Transform tree3;
    public Transform tree4;
    public Transform crate;
    public Transform nitro;

    public Transform tankCamera;

    public Transform halfScreen;

    public Transform tempest;
    public Transform viking;
    public Transform reaper;
    public Transform prometheus;

    private float supplyTimer = 0.0f;

    public float startingAreaRadius;

    private List<Transform> trees = new List<Transform>();
    private List<Transform> crates = new List<Transform>();
    private List<Transform> nitros = new List<Transform>();
    private List<Transform> startingPoints = new List<Transform>();

    public int treeAmount;
    public int nitroAmount;
    public int crateAmount;

    public static GameManager instance = null;
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
    }

    void Start () {
        Vector3 pos;
        /*** Tank Types:
         * 1: M-Q6 Tempest
         * 2: SP28 Viking
         * 3: T-K3 Reaper
         * 4: RY-7 Prometheus
         */

        /*** Tank Types:
         * 1: Red
         * 2: Blue
         * 3: Green
         * 4: Yellow
         */


        int children = GameObject.Find("StartingPoints").transform.childCount;
        for(int i = 0; i < children; i++)
        {
            startingPoints.Add(GameObject.Find("StartingPoints").transform.GetChild(i));
        }

        GameObject.Find("MapCam").GetComponent<Camera>().enabled = false;

        int[] tankTypes = new int[] {0, 0};
        //int[] tankColors = new int[] {0, 1};

        List<int> usedPoints = new List<int>();

        for(int i = 0; i < playerAmount; i++)
        {
            Transform tank;
            Transform cam;
            Transform canvas;
            Tank t;
            SmoothCamera c;

            int position;
            do {
                position = Random.Range(0, startingPoints.Count);
            } while (usedPoints.Contains(position));

            usedPoints.Add(position);

            Transform t_pos = startingPoints[position];
            Vector3 v_pos = new Vector3(t_pos.position.x, t_pos.eulerAngles.y, t_pos.position.z);

            switch (tankTypes[i])
            {
                case 0:
                    tank = Instantiate(tempest, Vector3.zero, Quaternion.Euler(0, 0, 0));
                    cam = Instantiate(tankCamera, Vector3.zero, Quaternion.Euler(0, 0, 0));

                    canvas = Instantiate(halfScreen, Vector3.zero, Quaternion.Euler(0, 0, 0));
                    canvas.GetComponent<Canvas>().worldCamera = cam.Find("Camera").GetComponent<Camera>();

                    canvas.parent = tank;
                    canvas.name = "Canvas";

                    t = tank.GetComponent<Tank>();
                    c = cam.GetComponent<SmoothCamera>();
                    t.cam = cam;
                    c.tank = tank.gameObject;

                    t.padNumber = i+1;
                    Camera camera = cam.Find("Camera").GetComponent<Camera>();
                    setCameraViewport(camera, i+1);
                    t.Spawn(v_pos);
                    players.Add(tank);

                    break;
                case 1:
                    tank = Instantiate(viking, Vector3.zero, Quaternion.Euler(0, 0, 0));
                    cam = Instantiate(tankCamera, Vector3.zero, Quaternion.Euler(0, 0, 0));

                    t = tank.GetComponent<Tank>();
                    c = cam.GetComponent<SmoothCamera>();
                    t.cam = cam;
                    c.tank = tank.gameObject;

                    t.padNumber = i + 1;
                    Camera camera1 = cam.Find("Camera").GetComponent<Camera>();
                    setCameraViewport(camera1, i + 1);
                    t.Spawn(v_pos);
                    players.Add(tank);

                    break;
                case 3:
                    tank = Instantiate(prometheus, Vector3.zero, Quaternion.Euler(0, 0, 0));
                    cam = Instantiate(tankCamera, Vector3.zero, Quaternion.Euler(0, 0, 0));

                    t = tank.GetComponent<Tank>();
                    c = cam.GetComponent<SmoothCamera>();
                    t.cam = cam;
                    c.tank = tank.gameObject;

                    t.padNumber = i + 1;
                    Camera camera2 = cam.Find("Camera").GetComponent<Camera>();
                    setCameraViewport(camera2, i + 1);
                    t.Spawn(v_pos);
                    players.Add(tank);

                    break;
            }
        }

		for(int i = 0; i < treeAmount + crateAmount + nitroAmount; i++)
        {
            do
            {
                pos = GeneratePosition();
            }
            while (!IsPositionValid(pos));

            if(i >= (treeAmount + crateAmount))
            {
                SpawnNitroAt(pos);
            }
            else
            {
                if (i >= treeAmount)
                {
                    SpawnCrateAt(pos);
                }
                else
                {
                    int sort = Random.Range(2,5);

                    switch (sort)
                    {
                        case 1:
                            SpawnTreeAt(tree, pos);
                            break;
                        case 2:
                            SpawnTreeAt(tree2, pos);
                            break;
                        case 3:
                            SpawnTreeAt(tree3, pos);
                            break;
                        case 4:
                            SpawnTreeAt(tree4, pos);
                            break;
                        default:
                            SpawnTreeAt(tree, pos);
                            break;
                    }
                }
            }
        }
	}

    //Returns x and z contains the randomized XZ-position, y the randomized Y-rotation
    private Vector3 GeneratePosition()
    {
        int offset = 2;
        float x = Random.Range((-mapSize / 2.0f) + offset, mapSize / 2.0f - offset);
        float z = Random.Range((-mapSize / 2.0f) + offset, mapSize / 2.0f - offset);
        float r = Random.Range(0.0f, 360.0f);

        return new Vector3(x, r, z);
    }

    public Vector3 GetSpawnPoint()
    {
        int spawn = Random.Range(0, 8);
        return startingPoints[spawn].position;
    }

    public void GiveKillToPlayer(int owner)
    {
        var tank = players[owner-1].GetComponent<Tempest>();
        tank.Kills++;
    }

    private bool IsPositionValid(Vector3 position)
    {
        Vector3 fixedPosition = position;
        fixedPosition.y = 0;
        //Debug.Log(startingPoints.Count);

        foreach(Transform t in startingPoints)
        {
            if(Vector3.Distance(fixedPosition, t.position) < startingAreaRadius)
            {
                return false;
            }
        }

        foreach (Transform c in crates)
        {
            if (Vector3.Distance(fixedPosition, c.position) < 3.0f)
            {
                return false;
            }
        }

        foreach (Transform t in trees)
        {
            if (Vector3.Distance(fixedPosition, t.position) < 3.0f)
            {
                return false;
            }
        }

        return true;
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Cancel"))
        {
            //RemoveNullCrates();
            //SupplyDrop();
            //Debug.Log(crates.Count);
        }
        supplyTimer += Time.deltaTime;
        if(supplyTimer > 15.0f)
        {
            SupplyDrop();
            supplyTimer -= 15.0f;
        }

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

    private void SupplyDrop()
    {
        int crateDifference = crateAmount - crates.Count;
        int nitroDifference = nitroAmount - nitros.Count;

        for (int i = 0; i < crateDifference/2; i++)
        {
            Vector3 pos;
            do
            {
                pos = GeneratePosition();
            } while (!IsPositionValid(pos));
            SpawnCrateAt(pos);
        }

        for (int i = 0; i < nitroDifference/2; i++)
        {
            Vector3 pos;
            do
            {
                pos = GeneratePosition();
            } while (!IsPositionValid(pos));
            SpawnNitroAt(pos);
        }

    }

    private void SpawnCrateAt(Vector3 pos)
    {
        var t_crate = Instantiate(crate, new Vector3(pos.x, 1, pos.z), Quaternion.Euler(0, pos.y, 0));
        crates.Add(t_crate);
    }

    private void SpawnNitroAt(Vector3 pos)
    {
        var t_nitro = Instantiate(nitro, new Vector3(pos.x, 1, pos.z), Quaternion.Euler(0, pos.y, 0));
        nitros.Add(t_nitro);
    }

    

    public void SpawnTreeAt(Transform type, Vector3 pos)
    {
        Transform t_tree = Instantiate(type, new Vector3(pos.x, 0, pos.z), Quaternion.Euler(0, pos.y, 0));
        trees.Add(t_tree);
    }

    public void RemoveNullCrates()
    {
        Debug.Log("BEFORE: " + crates.Count);
        for(int i = 0; i < crates.Count; i++)
        {
            if(crates[i] == null)
            {
                crates.RemoveAt(i);
            }
        }
        Debug.Log("AFTER: " + crates.Count);
    }

    public static void ShowEndScreen()
    {
        Debug.Log("EndScreen");
        SceneManager.LoadScene("EndScreen");
    }
}
