using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemSolvingGameManager : MonoBehaviour
{
    public bool fullyLinked = false;
    public GameObject sink;

    public static ProblemSolvingGameManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
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
        }
        else
        {
            Debug.Log("WRONG!");
        }

    }
}
