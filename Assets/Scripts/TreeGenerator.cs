using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public static TreeGenerator instance = null;

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

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GenerateTree()
    {
        GameObject tree = new GameObject();
        tree.AddComponent<Tree>();
        tree.tag = "Tree";
        tree.AddComponent<CapsuleCollider>();
        CapsuleCollider collider = tree.GetComponent<CapsuleCollider>();
        var root = Instantiate(Resources.Load("TreeGen/Meadow/Trunk"), Vector3.zero, new Quaternion(0,0,0,0)) as GameObject;
        float xzScale = Random.Range(3.0f,6f);
        float yScale = Random.Range(3.0f, 6f);
        root.transform.localScale = new Vector3(xzScale, yScale, xzScale);
        root.transform.parent = tree.transform;

        int branches = Random.Range(2, 4);
        root.gameObject.name = "Root";
        for (int i = 0; i < branches; i++)
        {
            float yRotation = Random.Range(0, (360 / branches)) + (i * (360/branches));
            float angleRange = 90.0f; 
            var branch = Instantiate(Resources.Load("TreeGen/Meadow/Trunk"), root.transform.Find("Socket").transform.position, Quaternion.Euler(Random.Range(angleRange/3, angleRange*2/3), yRotation, 0)) as GameObject;
            branch.transform.Translate(new Vector3(0.0f, -0.25f, 0.0f));
            xzScale = Random.Range(5.0f, 6.0f);
            yScale = Random.Range(4.0f, 6.0f);
            branch.transform.parent = tree.transform;
            branch.transform.localScale = new Vector3(xzScale, yScale, xzScale);
        }

        string objPath = "TreeGen/Meadow/Leafs/Leafs_" + Random.Range(0, 4).ToString();
        string matPath = "TreeGen/Meadow/Materials/Leafs_" + Random.Range(0, 4).ToString();
        var leafs = Instantiate(Resources.Load(objPath), root.transform.Find("Socket").transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
        leafs.GetComponent<MeshRenderer>().material = Resources.Load(matPath) as Material;

        leafs.transform.parent = tree.transform;
        float leafScale = Random.Range(2.0f, 3.0f);
        leafs.transform.localScale = new Vector3(leafScale, leafScale, leafScale);

        foreach (Transform branch in tree.transform)
        {
            if(branch.name == "Socket")
            {
                continue;
            }
            objPath = "TreeGen/Meadow/Leafs/Leafs_" + Random.Range(0, 4).ToString();
            matPath = "TreeGen/Meadow/Materials/Leafs_" + Random.Range(0, 4).ToString();
            try
            {
                leafs = Instantiate(Resources.Load(objPath), branch.transform.Find("Socket").transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
                leafs.GetComponent<MeshRenderer>().material = Resources.Load(matPath) as Material;
                leafScale = Random.Range(2.0f, 4.0f);
                leafs.transform.localScale = new Vector3(leafScale, leafScale, leafScale);
                leafs.transform.parent = tree.transform;
            }
            catch(System.Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
        }

        collider.radius = 0.33f;
        collider.height = 4.0f;
        return tree;
    }
}
