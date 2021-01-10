using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] pipes;
    public Transform firstSpawnPoint;
    public GameObject finish;
    public GameObject sink;


    private int Lpercentage = 70;
    private int Ipercentage = 30;

    private GameObject[,] map;

    public int rows;
    public int columns; // 2D Array with capacity mapLength x mapHeight
    public float finishOffsetY = -2f;
    public float sinkOffsetY = 2f;

    private float tileLengthX;
    private float tileLengthY;
    private int possiblePipeStateCount;
    

    void Start()
    {
        tileLengthX = pipes[0].GetComponent<BoxCollider>().size.x;
        tileLengthY = pipes[0].GetComponent<BoxCollider>().size.y;
        possiblePipeStateCount = pipes[0].GetComponent<Tile>().totalStates;
        map = new GameObject[rows,columns];
        GenerateMap();
        
    }

    /*
    public void GenerateMap()
    {

        float sinkSpawnX = firstSpawnPoint.position.x;
        float sinkSpawnY = firstSpawnPoint.position.y + sinkOffsetY;
        Vector3 sinkSpawnPoint = new Vector3(sinkSpawnX, sinkSpawnY, 0f);
        sink = Instantiate(sink, sinkSpawnPoint, Quaternion.identity);

        ProblemSolvingGameManager.instance.sink = sink;
        ProblemSolvingGameManager.instance.waterManagerScript.water = sink.GetComponent<LineRenderer>();


        float nextSpawnX;
        float nextSpawnY;

        for (int i =0; i< rows; i++)
        {
            nextSpawnY = firstSpawnPoint.position.y - (i * tileLengthY);
            for (int j = 0; j< columns; j++)
            {                
                nextSpawnX = firstSpawnPoint.position.x + (j * tileLengthX);                           
                if (!map[i, j])
                {
                    Vector3 nextSpawn = new Vector3(nextSpawnX, nextSpawnY, 0f);
                    int randomPipeType = (int)Random.Range(0, pipes.Length);
                    int randomState = (int)Random.Range(0, possiblePipeStateCount);
                    map[i, j] = Instantiate(pipes[randomPipeType], nextSpawn, Quaternion.identity);
                    map[i, j].GetComponent<Tile>().currentState = randomState;
                }
            }
        }

        float finishSpawnX = firstSpawnPoint.position.x + (columns/2f * tileLengthX);
        float finishSpawnY = firstSpawnPoint.position.y - (rows  * tileLengthY ) + finishOffsetY;
        Vector3 finishSpawnPoint = new Vector3(finishSpawnX,finishSpawnY, 0f);
        finish = Instantiate(finish, finishSpawnPoint, Quaternion.identity);
        
    }
    */




    public void GenerateMap()
    {

        float sinkSpawnX = firstSpawnPoint.position.x;
        float sinkSpawnY = firstSpawnPoint.position.y + sinkOffsetY;
        Vector3 sinkSpawnPoint = new Vector3(sinkSpawnX, sinkSpawnY, 0f);
        sink = Instantiate(sink, sinkSpawnPoint, Quaternion.identity);

        ProblemSolvingGameManager.instance.sink = sink;
        ProblemSolvingGameManager.instance.waterManagerScript.water = sink.GetComponent<LineRenderer>();


        float nextSpawnX;
        float nextSpawnY;

        for (int i = 0; i < rows; i++)
        {
            nextSpawnY = firstSpawnPoint.position.y - (i * tileLengthY);
            for (int j = 0; j < columns; j++)
            {
                nextSpawnX = firstSpawnPoint.position.x + (j * tileLengthX);
                if (!map[i, j])
                {
                    Vector3 nextSpawn = new Vector3(nextSpawnX, nextSpawnY, 0f);
                    int randomPipeType = (int)Random.Range(0, 100);
                    if(randomPipeType <= Lpercentage)
                    {
                        randomPipeType = 0;
                    }
                    else
                    {
                        randomPipeType = 1;
                    }

                    int randomState = (int)Random.Range(0, possiblePipeStateCount);
                    map[i, j] = Instantiate(pipes[randomPipeType], nextSpawn, Quaternion.identity);
                    map[i, j].GetComponent<Tile>().currentState = randomState;
                }
            }
        }

        float finishSpawnX = firstSpawnPoint.position.x + (columns / 2f * tileLengthX);
        float finishSpawnY = firstSpawnPoint.position.y - (rows * tileLengthY) + finishOffsetY;
        Vector3 finishSpawnPoint = new Vector3(finishSpawnX, finishSpawnY, 0f);
        finish = Instantiate(finish, finishSpawnPoint, Quaternion.identity);

    }

}
