using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PCGScenePopulation : MonoBehaviour
{

    public GameObject[] spawnObjects;
    public float radius;
    public int quantity;
    public float raycastDistance = 100f;
    public float overlap = 1f;
    public LayerMask populationLayer;
    public LayerMask terrainLayer;
    public bool orientToTerrain;
    public float smallScale=0.75f;
    public float largeScale=1.25f;
    public int seed;
    private int _seedCheck;
    private Vector2 _heightPlacementCheck;
    private int _quantityCheck;
    public Vector2 heightPlacement;


    private void Awake()
    {
        //Random.InitState(seed);
    }


    // Start is called before the first frame update
    void Start()
    {
        Populate();
    }
    private void Update()
    {
        if (_seedCheck != seed|| _heightPlacementCheck != heightPlacement||_quantityCheck!=quantity)
        {
            _seedCheck = seed;
            _heightPlacementCheck = heightPlacement;
            _quantityCheck = quantity;
            Populate();
        }
    }


    void Populate()
    {

        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        int counter = 0;
        Random.InitState(seed);

        for (int i = 0; i < quantity; i++)
        {
            Vector3 randomPos = (Random.insideUnitSphere * radius) + transform.position;
            float scale = Random.Range(smallScale, largeScale);
            Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            GameObject spawnObject = spawnObjects[Random.Range(0, spawnObjects.Length)];


            RaycastHit hit;
            if (Physics.Raycast(randomPos + new Vector3(0.0f, raycastDistance / 2, 0.0f), Vector3.down, out hit, raycastDistance, terrainLayer))
            {
                if (hit.point.y > heightPlacement.x && hit.point.y < heightPlacement.y)
                {

                    Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    Vector3 overlapBox = new Vector3(overlap, overlap, overlap);
                    Collider[] collidersOverlapped = new Collider[1];
                    int collidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapBox, collidersOverlapped, spawnRotation, populationLayer);
                    if (collidersFound == 0)
                    {
                        if (orientToTerrain)
                        {
                            GameObject clone = Instantiate(spawnObject, hit.point, spawnRotation * randomRot, transform);
                            //clone.transform.localScale = new Vector3(scale, scale, scale);
                            counter++;

                        }
                        else
                        {
                            GameObject clone = Instantiate(spawnObject, hit.point, randomRot, transform);
                            counter++;
                        }


                    }
                    //else { i--; }



                }



            }
            else { i--; }



        }
        Debug.Log(counter);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color of the gizmo
        Gizmos.DrawSphere(transform.position, radius);
    }
}
