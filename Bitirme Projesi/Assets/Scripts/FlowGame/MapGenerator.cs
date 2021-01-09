using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] pipes;
    public Transform firstSpawnPoint;

    public GameObject[,] map;

    public int rows;
    public int columns; // 2D Array with capacity mapLength x mapHeight
    


    public float tileLengthX;
    public float tileLengthY;

    public int possiblePipeStateCount;
    

    void Start()
    {
        tileLengthX = pipes[0].GetComponent<BoxCollider>().size.x;
        tileLengthY = pipes[0].GetComponent<BoxCollider>().size.y;
        possiblePipeStateCount = pipes[0].GetComponent<Tile>().totalStates;
        map = new GameObject[rows,columns];
        GenerateMap();
        
    }

    public void GenerateMap()
    {

        float nextSpawnX;
        float nextSpawnY;

        for (int i =0; i< rows; i++)
        {
            nextSpawnY = firstSpawnPoint.position.y - (i * tileLengthY);
            for (int j = 0; j< columns; j++)
            {
                int randomPipeType = (int)Random.Range(0, pipes.Length);
                int randomState = (int) Random.Range(0, possiblePipeStateCount);

                nextSpawnX = firstSpawnPoint.position.x + (j * tileLengthX);

                Vector3 nextSpawn = new Vector3(nextSpawnX, nextSpawnY, 0f);

                Debug.Log("Random pipe type: " + randomPipeType + " Random State: " + randomState);

                if (!map[i, j])
                {
                    map[i, j] = Instantiate(pipes[randomPipeType], nextSpawn, Quaternion.identity);
                    map[i, j].GetComponent<Tile>().currentState = randomState;
                }
            }
        }
        
    }
}
