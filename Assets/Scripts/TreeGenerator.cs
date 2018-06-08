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
        //GenerateTree();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform GenerateTree()
    {
        var root = Instantiate(Resources.Load("TreeGen/Meadow/Trunk"), Vector3.zero, new Quaternion(0,0,0,0)) as GameObject;
        float xzScale = Random.Range(5.0f,7.5f);
        float yScale = Random.Range(2.5f, 7.5f);
        root.transform.localScale = new Vector3(xzScale, yScale, xzScale);

        int branches = Random.Range(2, 4);
        for (int i = 0; i < branches; i++)
        {
            float angleRange = 50.0f;
            var branch = Instantiate(Resources.Load("TreeGen/Meadow/Trunk"), root.transform.Find("Socket").transform.position, Quaternion.Euler(Random.Range(-angleRange, angleRange), 0, Random.Range(-angleRange, angleRange))) as GameObject;
            branch.transform.Translate(new Vector3(0.0f, -0.25f, 0.0f));
            xzScale = Random.Range(0.25f, 0.5f);
            yScale = Random.Range(0.25f, 0.6f);
            branch.transform.parent = root.transform;
            branch.transform.localScale = new Vector3(xzScale, yScale, xzScale);
        }

        string objPath = "TreeGen/Meadow/Leafs/Leafs_" + Random.Range(0, 4).ToString();
        string matPath = "TreeGen/Meadow/Materials/Leafs_" + Random.Range(0, 4).ToString();
        var leafs = Instantiate(Resources.Load(objPath), root.transform.Find("Socket").transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
        leafs.GetComponent<MeshRenderer>().material = Resources.Load(matPath) as Material;

        leafs.transform.parent = root.transform;
        float leafScale = Random.Range(0.5f, 1.0f);

        foreach (Transform branch in root.transform)
        {
            if(branch.name == "Socket")
            {
                continue;
            }
            objPath = "TreeGen/Meadow/Leafs/Leafs_" + Random.Range(0, 4).ToString();
            matPath = "TreeGen/Meadow/Materials/Leafs_" + Random.Range(0, 4).ToString();
            leafs = Instantiate(Resources.Load(objPath), branch.transform.Find("Socket").transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
            leafs.GetComponent<MeshRenderer>().material = Resources.Load(matPath) as Material;
            leafScale = Random.Range(1.0f, leafScale);
            leafs.transform.localScale = new Vector3(leafScale, leafScale, leafScale);
            leafs.transform.parent = root.transform;
        }
        return root.transform;
    }
}
