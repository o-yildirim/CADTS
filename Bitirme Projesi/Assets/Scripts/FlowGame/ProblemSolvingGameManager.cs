using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemSolvingGameManager : MonoBehaviour
{
    public bool fullyLinked = false;
    public bool inputUnavailable = false;

    public GameObject sink;
    public GameObject finish;

    public FlowingWaterManager waterManagerScript;
    public MapGenerator mapGenerator;
    public List<Vector3> pipesPassedTrough;



    public static ProblemSolvingGameManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
       
    }
    private void Start()
    {
        pipesPassedTrough = new List<Vector3>();
        waterManagerScript = GetComponent<FlowingWaterManager>();
        //mapGenerator = GetComponent<MapGenerator>();
        //mapGenerator.GenerateMap();
    }

    public void startCheckingSequence()
    {
        inputUnavailable = true;
        pipesPassedTrough.Add(sink.transform.position);

        RaycastHit hitTile;
        if(Physics.Raycast(sink.transform.position,-sink.transform.up,out hitTile))
        {
            Tile hitTileScript = hitTile.transform.GetComponent<Tile>();
            if(hitTileScript != null)
            {
                hitTileScript.checkTransmission(0);
            }
        }

        if (fullyLinked)
        {
            Debug.Log("GAME FINISHED");
            //waterManagerScript.drawWater(pipesPassedTrough);
            StartCoroutine(waterManagerScript.drawWaterSlow(pipesPassedTrough));
        }
        else
        {
            Debug.Log("WRONG!");
            pipesPassedTrough.Clear();
            inputUnavailable = false;
            //pipesPassedTrough.Add(sink.transform.position);
        }

    }
}
