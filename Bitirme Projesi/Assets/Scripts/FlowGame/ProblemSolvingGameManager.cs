using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemSolvingGameManager : MonoBehaviour
{
    public bool fullyLinked = false;
    public bool inputUnavailable = false;

    public GameObject sink;
    public GameObject finish;
    public GameObject valve;

    public FlowingWaterManager waterManagerScript;
    public MapGenerator mapGenerator;
    public Valve valveScript;

    public List<GameObject> pipesPassedTrough;



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
        pipesPassedTrough = new List<GameObject>();
        waterManagerScript = GetComponent<FlowingWaterManager>();
        valveScript = valve.GetComponentInChildren<Valve>();
        mapGenerator = GetComponent<MapGenerator>();
    }

    public void startCheckingSequence()
    {
        inputUnavailable = true;
        pipesPassedTrough.Add(sink);

        RaycastHit hitTile;
        if(Physics.Raycast(sink.transform.position,-sink.transform.up,out hitTile))
        {
            Tile hitTileScript = hitTile.transform.GetComponent<Tile>();
            if(hitTileScript != null)
            {
                hitTileScript.checkTransmission(0);
            }
        }

        /* if (fullyLinked)
         {
             Debug.Log("GAME FINISHED");

             Coroutine valveRotating = StartCoroutine(valveScript.Rotate(2f));        
             StartCoroutine(waterManagerScript.drawWaterSlow(pipesPassedTrough,valveRotating));
         }
         else
         {
             Debug.Log("WRONG!");
             pipesPassedTrough.Clear();
             inputUnavailable = false;
             //pipesPassedTrough.Add(sink.transform.position);
         }*/

        //Coroutine valveRotating = StartCoroutine(valveScript.Rotate(2f));
        //StartCoroutine(waterManagerScript.drawWaterSlow(pipesPassedTrough, valveRotating));
        StartCoroutine(initiateWater());
        //pipesPassedTrough.Clear();
        //pipesPassedTrough.Add(sink.transform.position);
    }
    public IEnumerator initiateWater()
    {
        Coroutine valveRotating = StartCoroutine(valveScript.Rotate(2f));
        Coroutine waterFlowing =StartCoroutine(waterManagerScript.DrawWaterSlow(pipesPassedTrough, valveRotating));
        yield return waterFlowing;
        if (!fullyLinked)
        {
            pipesPassedTrough.Clear();
            waterManagerScript.ResetElements();
            inputUnavailable = false;
        }
        else
        {
            //ISTATISTIK GOSTER
        }
    }
}
