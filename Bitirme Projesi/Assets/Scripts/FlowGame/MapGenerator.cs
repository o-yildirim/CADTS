using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] pipes;
    public Transform firstSpawnPoint;

    //private int Lpercentage = 70;
    //private int Ipercentage = 30;
    private int maxI;
    private int maxL;
    private int ICounter = 0;
    private int LCounter = 0;

    public GameObject[,] map;

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

        maxI = (rows * columns) / 4;
        maxL = (rows * columns) - maxI;

        GenerateMap();
    }

    public void GenerateMap()
    {

        float sinkSpawnX = firstSpawnPoint.position.x;
        float sinkSpawnY = firstSpawnPoint.position.y + sinkOffsetY;
        Vector3 sinkSpawnPoint = new Vector3(sinkSpawnX, sinkSpawnY, 0f);
        ProblemSolvingGameManager.instance.sink.transform.position = sinkSpawnPoint;

        float finishSpawnXoffset = 0;
        if(columns % 2 == 0)
        {
            finishSpawnXoffset = 0;
        }
        else
        {
            finishSpawnXoffset = tileLengthX / 2;
        }

        float finishSpawnX = firstSpawnPoint.position.x + (columns / 2f * tileLengthX) - finishSpawnXoffset;
        float finishSpawnY = firstSpawnPoint.position.y - (rows * tileLengthY) + finishOffsetY;
        Vector3 finishSpawnPoint = new Vector3(finishSpawnX, finishSpawnY, 0f);
        ProblemSolvingGameManager.instance.finish.transform.position = finishSpawnPoint;



        //GenerateSolution();

  
        CreateCorners();

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
                    int randomPipeType = DecidePipeType();
                    int randomState = (int)Random.Range(0, possiblePipeStateCount);
                    map[i, j] = Instantiate(pipes[randomPipeType], nextSpawn, Quaternion.identity);
                    map[i, j].GetComponent<Tile>().currentState = randomState;
                }
            }
        }


    }

    private void CreateCorners()
    {
        //SOL ALT KOSE

        int randomState = Random.Range(0, possiblePipeStateCount);
        float leftBottomCornerX = firstSpawnPoint.position.x;
        float leftBottomCornerY = firstSpawnPoint.position.y - (rows * tileLengthY) + tileLengthY;
        Vector3 cornerSpawn = new Vector3(leftBottomCornerX, leftBottomCornerY, 0f);
        map[rows - 1, 0] = Instantiate(pipes[0], cornerSpawn, Quaternion.identity);
        map[rows - 1, 0].GetComponent<Tile>().currentState = randomState;


        //SAG ALT KOSE
        randomState = Random.Range(0, possiblePipeStateCount);
        cornerSpawn.x += (columns * tileLengthX) - tileLengthX;
        map[rows - 1, columns - 1] = Instantiate(pipes[0], cornerSpawn, Quaternion.identity);
        map[rows - 1, columns-1].GetComponent<Tile>().currentState = randomState;

        //SAG UST KOSE
        randomState = Random.Range(0, possiblePipeStateCount);
        cornerSpawn.y += (rows * tileLengthY) - tileLengthY;
        map[0, columns - 1] = Instantiate(pipes[0], cornerSpawn, Quaternion.identity);
        map[0, columns - 1].GetComponent<Tile>().currentState = randomState;
    }

    /*
    public void GenerateSolution()
    {
        float currentTileX = firstSpawnPoint.position.x + (columns / 2f * tileLengthX);
        float currentTileY = firstSpawnPoint.position.y - (rows * tileLengthY) + tileLengthY; //burasi + n * tileLengthY falan olabilir

        int currentRowIndex = rows-1;        //finish oncesinden basliyorum;
        int currentColumnIndex = columns - 1;

        Vector3 nextTileSpawnPoint = new Vector3(currentTileX, currentTileY, 0f);


        map[currentRowIndex,currentColumnIndex] = Instantiate(pipes[0], nextTileSpawnPoint, Quaternion.identity);





    }
    */

    public int DecidePipeType()
    {
        int randomNum = (int)Random.Range(0f, 1.99999f);
        int type = randomNum;

        if(randomNum == 0)
        {
            if(LCounter < maxL)
            {
                LCounter++;
            }
            else
            {
                type = 1;
                ICounter++;
            }
        }
        else
        {
            if(ICounter < maxI)
            {
                ICounter++;
            }
            else
            {
                type = 0;
                LCounter++;
            }
        }

        return type;
    }
}
