﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] pipes;
    public Transform firstSpawnPoint;

    private int maxI;
    private int maxL;
    private int ICounter = 0;
    private int LCounter = 0;

    public GameObject[,] map;
    public GameObject mapGameObject;

    public int rows;
    public int columns; 
    public float finishOffsetY = -2f;
    public float sinkOffsetY = 2f;
    public float valveOffsetX = 2f;
    public float valveOffsetY = 1f;


    private float tileLengthX;
    private float tileLengthY;


    private int possiblePipeStateCount;

    

    void Start()
    {
        tileLengthX = pipes[0].GetComponent<BoxCollider>().size.x;
        tileLengthY = pipes[0].GetComponent<BoxCollider>().size.y;
        possiblePipeStateCount = pipes[0].GetComponent<Tile>().totalStates;
        InitializeMapValues();

    }

    public void InitializeMapValues()
    {
        map = new GameObject[rows, columns];

        maxI = (rows * columns) / 6;
        maxL = (rows * columns) - maxI;
    }

    public void GenerateMap()
    {
      
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
                    map[i, j].transform.parent = mapGameObject.transform;
                }
            }
        }


    }

    public void PositionFinish()
    {
        float finishSpawnXoffset = 0;
        if (columns % 2 != 0)
        {
            finishSpawnXoffset = tileLengthX / 2;
        }
       
        float finishSpawnX = firstSpawnPoint.position.x + (columns / 2f * tileLengthX) - finishSpawnXoffset;
        float finishSpawnY = firstSpawnPoint.position.y - (rows * tileLengthY) + finishOffsetY;
        Vector3 finishSpawnPoint = new Vector3(finishSpawnX, finishSpawnY, 0f);
        ProblemSolvingGameManager.instance.finish.transform.position = finishSpawnPoint;
        ProblemSolvingGameManager.instance.finish.transform.parent = mapGameObject.transform;
    }

    public void PositionSink()
    {
        float sinkSpawnX = firstSpawnPoint.position.x;
        float sinkSpawnY = firstSpawnPoint.position.y + sinkOffsetY;
        Vector3 sinkSpawnPoint = new Vector3(sinkSpawnX, sinkSpawnY, 0f);
        ProblemSolvingGameManager.instance.sink.transform.position = sinkSpawnPoint;
        ProblemSolvingGameManager.instance.sink.transform.parent = mapGameObject.transform;
    }
    public void PositionValve()
    {
        float valveSpawnX = firstSpawnPoint.position.x + valveOffsetX;
        float valveSpawnY = firstSpawnPoint.position.y + valveOffsetY;
        Vector3 valveSpawnPoint = new Vector3(valveSpawnX, valveSpawnY, 0f);
        ProblemSolvingGameManager.instance.valve.transform.position = valveSpawnPoint;
        ProblemSolvingGameManager.instance.valve.transform.parent = mapGameObject.transform;
       

    }


    public void CreateCorners()
    {
    

        int randomState = Random.Range(0, possiblePipeStateCount);
        float leftBottomCornerX = firstSpawnPoint.position.x;
        float leftBottomCornerY = firstSpawnPoint.position.y - (rows * tileLengthY) + tileLengthY;


        Vector3 cornerSpawn = new Vector3(leftBottomCornerX, leftBottomCornerY, 0f);
        map[rows - 1, 0] = Instantiate(pipes[0], cornerSpawn, Quaternion.identity);
        map[rows - 1, 0].GetComponent<Tile>().currentState = randomState;
        map[rows-1,0].transform.parent = mapGameObject.transform;

        randomState = Random.Range(0, possiblePipeStateCount);
        cornerSpawn.x += (columns * tileLengthX) - tileLengthX;
        map[rows - 1, columns - 1] = Instantiate(pipes[0], cornerSpawn, Quaternion.identity);
        map[rows - 1, columns-1].GetComponent<Tile>().currentState = randomState;
        map[rows - 1, columns-1].transform.parent = mapGameObject.transform;

 
        randomState = Random.Range(0, possiblePipeStateCount);
        cornerSpawn.y += (rows * tileLengthY) - tileLengthY;
        map[0, columns - 1] = Instantiate(pipes[0], cornerSpawn, Quaternion.identity);
        map[0, columns - 1].GetComponent<Tile>().currentState = randomState;
        map[0, columns-1].transform.parent = mapGameObject.transform;
    }

    
    public void GenerateSolution()
    {
        float spawnOffsetX = 0;
        if (columns % 2 != 0)
        {
            spawnOffsetX = tileLengthX / 2;
        }

        float currentTileX = firstSpawnPoint.position.x + (columns / 2f * tileLengthX) - spawnOffsetX;
        float currentTileY = firstSpawnPoint.position.y - (rows * tileLengthY) + tileLengthY; 

        int currentRowIndex = rows-1;       
        int currentColumnIndex = columns/2;      

        Vector3 nextTileSpawnPoint = new Vector3(currentTileX, currentTileY, 0f);

        int pipeType = (int) Random.Range(0, pipes.Length);    
        int startingState = (int)Random.Range(0, possiblePipeStateCount);  
        map[currentRowIndex,currentColumnIndex] = Instantiate(pipes[pipeType], nextTileSpawnPoint, Quaternion.identity);
        Tile createdTile = map[currentRowIndex, currentColumnIndex].GetComponent<Tile>();
        createdTile.currentState = startingState;
        createdTile.transform.parent = mapGameObject.transform;
        
        int input = 2;
        int desiredOutput = 0;
        int roll = (int)Random.Range(0, 3);
        if (roll == 0)
        {
            desiredOutput = 0; 
        }
        else if(roll == 1)
        {
            desiredOutput = 1; 
        }
        else if(roll == 2)
        {
            desiredOutput = 3;
        }

        bool isDone = false;
        while(!isDone)
        {
            for (int j = 0; j < possiblePipeStateCount; j++)
            {
                if (createdTile.edges[input] && createdTile.edges[desiredOutput])
                {
                    isDone = true;
                    break;
                }
                createdTile.manageEdges();
                createdTile.currentState = (createdTile.currentState + 1) % possiblePipeStateCount;

            }
            if (isDone)
            {
                break;
            }
            pipeType = (pipeType + 1) % pipes.Length;
            Destroy(map[currentRowIndex, currentColumnIndex]);
            map[currentRowIndex, currentColumnIndex] = Instantiate(pipes[pipeType], nextTileSpawnPoint, Quaternion.identity);
            createdTile = map[currentRowIndex, currentColumnIndex].GetComponent<Tile>();

            createdTile.currentState = startingState;
            createdTile.init();

            createdTile.transform.parent = mapGameObject.transform;
           
        }



        CreateAdjacentTile(createdTile, desiredOutput, currentRowIndex, currentColumnIndex);

    }
    

    public int DecidePipeType()
    {
        int randomNum = (int)Random.Range(0f, pipes.Length);
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

    public void CreateAdjacentTile(Tile tile,int output,int currentRowIndex,int currentColumnIndex)
    {

        if(tile.gameObject.transform.position == firstSpawnPoint.position && output == 0 )
        {            
            return;
        }

    
 
        int desiredOutput = 0;
        bool isOutputDetermined = false;

           

        if (output == 0) 
        {
            if (currentRowIndex > 0) 
            {
                currentRowIndex--;
                if (!isOutputDetermined)
                { 
                    int roll = (int)Random.Range(0, 2);
                    if (roll == 0)
                    {
                        desiredOutput = 0; 
                    }
                    else
                    {
                        desiredOutput = 3; 
                    }
                }
            }
          
        }
 
        else if (output == 1)
        {
            if (currentColumnIndex < columns-1 ) 
            {
                currentColumnIndex++;
                if (!isOutputDetermined) desiredOutput = 0;

            }
            
        }
        else if (output == 2)
        {
            if (currentRowIndex < rows-1 )  
            {
                
                currentRowIndex++;
                if(!isOutputDetermined) desiredOutput = 3;


            }
            
        }      
        else if (output == 3)
        {
            if (currentColumnIndex > 0)   
            {         
                currentColumnIndex--;
                if (!isOutputDetermined)
                {
                    int roll = (int)Random.Range(0, 2);
                    if (roll == 0)
                    {
                        desiredOutput = 0; 
                    }
                    else
                    {
                        desiredOutput = 3; 
                    }
                }
            }               
        }

        if (currentRowIndex == 0 && currentColumnIndex == 0)
        {
            desiredOutput = 0;
            isOutputDetermined = true;
        }
        else if (currentRowIndex == 0)
        {
            desiredOutput = 3;
            isOutputDetermined = true;
        }



        if (currentColumnIndex == 0 && currentRowIndex > 0)
        {
            desiredOutput = 0;
            isOutputDetermined = true;
        }
       



        int input = (output + 2) % possiblePipeStateCount;

     
        int pipeType = (int)Random.Range(0, pipes.Length);
        int startingState = 0;
    

        float currentTileX = firstSpawnPoint.transform.position.x + (currentColumnIndex * tileLengthX);
        float currentTileY = firstSpawnPoint.transform.position.y - (currentRowIndex * tileLengthY);
        Vector3 nextTileSpawnPoint = new Vector3(currentTileX, currentTileY, 0f);
        if (!map[currentRowIndex, currentColumnIndex])
        {           
            map[currentRowIndex, currentColumnIndex] = Instantiate(pipes[pipeType], nextTileSpawnPoint, Quaternion.identity);
        }
        
        Tile createdTile = map[currentRowIndex, currentColumnIndex].GetComponent<Tile>();
        createdTile.currentState = startingState;

        bool isDone = false;
        while (!isDone)
        { 
            for (int j = 0; j < possiblePipeStateCount; j++)
            {
                if (createdTile.edges[input] && createdTile.edges[desiredOutput])
                {
                    isDone = true;
                    break;
                }
                createdTile.manageEdges();
                createdTile.currentState = (createdTile.currentState + 1) % possiblePipeStateCount;

            }
            if (isDone)
            {
                break;
            }
            
            pipeType = (pipeType + 1) % pipes.Length;
            Destroy(map[currentRowIndex, currentColumnIndex]);           
            map[currentRowIndex, currentColumnIndex] = Instantiate(pipes[pipeType], nextTileSpawnPoint, Quaternion.identity);
            createdTile = map[currentRowIndex, currentColumnIndex].GetComponent<Tile>();
            createdTile.currentState = startingState;
            createdTile.init();
           
        }
        createdTile.transform.parent = mapGameObject.transform; 
        CreateAdjacentTile(createdTile, desiredOutput, currentRowIndex, currentColumnIndex);

    }

    public void MixSolution() 
    {
        for(int i = 0; i< rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                if (map[i,j] != null)
                {
                   
                    Tile tile = map[i, j].GetComponent<Tile>();
                    if (tile != null)
                    {
                        int randomState;
                        do
                        {
                          randomState = Random.Range(0, possiblePipeStateCount);
                        } while (randomState != tile.currentState);
                  
                        tile.currentState = randomState;              
                        tile.manageEdges();
                        tile.currentState = (tile.currentState+1)%possiblePipeStateCount;
                    }
                }
            }
        }
    }

    public void RepositionCamera()
    {
        float xPos = (map[0, 0].transform.position.x + map[0, columns - 1].transform.position.x)/2f;
        float yPos = (ProblemSolvingGameManager.instance.sink.transform.position.y + ProblemSolvingGameManager.instance.finish.transform.position.y)/ 2f;

        Camera.main.orthographicSize = 5.15f;
        Vector3 newCamPos = new Vector3(xPos, yPos, 0f);
        newCamPos.z = Camera.main.transform.position.z;
        Camera.main.transform.position = newCamPos;
           
    }
}
