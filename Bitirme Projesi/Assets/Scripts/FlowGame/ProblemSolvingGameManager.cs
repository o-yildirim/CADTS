using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemSolvingGameManager : MonoBehaviour
{
    public bool fullyLinked = false;
    public GameObject sink;
    public FlowingWaterManager waterManagerScript;
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
        pipesPassedTrough.Add(sink.transform.position);
        waterManagerScript = GetComponent<FlowingWaterManager>();
    }

    public void startCheckingSequence()
    {
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
            waterManagerScript.drawWater(pipesPassedTrough);
        }
        else
        {
            Debug.Log("WRONG!");
            pipesPassedTrough.Clear();
            pipesPassedTrough.Add(sink.transform.position);
        }

    }
}
