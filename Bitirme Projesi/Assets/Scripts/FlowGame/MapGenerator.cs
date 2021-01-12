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

    public int debugCounter = 1;
    

    void Start()
    {
        tileLengthX = pipes[0].GetComponent<BoxCollider>().size.x;
        tileLengthY = pipes[0].GetComponent<BoxCollider>().size.y;
        possiblePipeStateCount = pipes[0].GetComponent<Tile>().totalStates;
        map = new GameObject[rows,columns];

        maxI = (rows * columns) / 6;
        maxL = (rows * columns) - maxI;


        PositionSink();
        PositionFinish();
        CreateCorners();
        GenerateSolution();
        MixSolution();
        GenerateMap();
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
                }
            }
        }


    }

    private void PositionFinish()
    {
        float finishSpawnXoffset = 0;
        if (columns % 2 == 0)
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
    }

    private void PositionSink()
    {
        float sinkSpawnX = firstSpawnPoint.position.x;
        float sinkSpawnY = firstSpawnPoint.position.y + sinkOffsetY;
        Vector3 sinkSpawnPoint = new Vector3(sinkSpawnX, sinkSpawnY, 0f);
        ProblemSolvingGameManager.instance.sink.transform.position = sinkSpawnPoint;
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

    
    public void GenerateSolution()
    {
        float currentTileX = firstSpawnPoint.position.x + (columns / 2f * tileLengthX);
        float currentTileY = firstSpawnPoint.position.y - (rows * tileLengthY) + tileLengthY; //burasi + n * tileLengthY falan olabilir

        int currentRowIndex = rows-1;        //finish oncesinden basliyorum;
        int currentColumnIndex = columns/2;
        //Debug.Log(debugCounter.ToString() + ".tile, Row :" + currentRowIndex + " Column: " + currentColumnIndex);

        Vector3 nextTileSpawnPoint = new Vector3(currentTileX, currentTileY, 0f);

        int pipeType = (int) Random.Range(0, pipes.Length);
        //int pipeType = 0;
        //int startingState = (int)Random.Range(0, possiblePipeStateCount);
        int startingState = 0;
        map[currentRowIndex,currentColumnIndex] = Instantiate(pipes[pipeType], nextTileSpawnPoint, Quaternion.identity);
        Tile createdTile = map[currentRowIndex, currentColumnIndex].GetComponent<Tile>();
        createdTile.transform.name = debugCounter.ToString();
        debugCounter++;    
        createdTile.currentState = startingState;
        createdTile.init();

        int input = 2;
        int roll = (int)Random.Range(0, 2);
        int desiredOutput = 0;
        if (roll == 0)
        {
            desiredOutput = 0; //SOL
        }
        else
        {
            desiredOutput = 3; //YUKARI
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
            createdTile.transform.name = debugCounter.ToString();
            debugCounter++;
            
        }



        Debug.Log("Input: " + input + " Output: " + desiredOutput + " for " + createdTile.transform.name);
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
            Debug.Log(tile.transform.name + " Row: " + currentRowIndex + " Column: " + currentColumnIndex);
            return;
        }

        int pipeType = 0;
        bool pipeTypeDetermined = false;

        int desiredOutput = 0;
        bool isOutputDetermined = false;

       
        

        if (output == 0) //ACIL MUDAHALE GEREKIR
        {
            if (currentRowIndex > 0) //YUKARI YARATMAYA CALISIYOR
            {
                currentRowIndex--;
                if (!isOutputDetermined)
                { 
                    int roll = (int)Random.Range(0, 2);
                    if (roll == 0)
                    {
                        desiredOutput = 0; //SOL
                    }
                    else
                    {
                        desiredOutput = 3; //YUKARI
                    }
                }
            }
          
        }
 
        else if (output == 1)
        {
            if (currentColumnIndex < columns-1 ) //SAĞA YARATMAYA CALISIYOR
            {
                currentColumnIndex++;
                if (!isOutputDetermined) desiredOutput = 0;

            }
            
        }
        else if (output == 2)
        {
            if (currentRowIndex < rows-1 )  //ASAGI YARATMAYA CALISIYOR
            {
                
                currentRowIndex++;
                if(!isOutputDetermined) desiredOutput = 3;


            }
            
        }      
        else if (output == 3)
        {
            if (currentColumnIndex > 0)   //SOLA YARATMAYA CALISIYOR
            {         
                currentColumnIndex--;
                if (!isOutputDetermined)
                {
                    int roll = (int)Random.Range(0, 2);
                    if (roll == 0)
                    {
                        desiredOutput = 0; //SOL
                    }
                    else
                    {
                        desiredOutput = 3; //YUKARI
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
            pipeType = 1;
            pipeTypeDetermined = true;
            isOutputDetermined = true;
        }
        else if (currentColumnIndex == columns - 1 || currentColumnIndex == 1)
        {
            pipeType = 0;
            pipeTypeDetermined = true;
        }
        /*else if(currentColumnIndex == columns-1 || currentColumnIndex == 1)
        {
            pipeType = 0;
            pipeTypeDetermined = true;
        }*/







        int input = (output + 2) % possiblePipeStateCount;

        if (!pipeTypeDetermined)
        {
            pipeType = (int)Random.Range(0, pipes.Length);
        }
        int startingState = 0;
        //int pipeType = 0;
        //int startingState = (int)Random.Range(0, possiblePipeStateCount);

        float currentTileX = firstSpawnPoint.transform.position.x + (currentColumnIndex * tileLengthX);
        float currentTileY = firstSpawnPoint.transform.position.y - (currentRowIndex * tileLengthY);
        Vector3 nextTileSpawnPoint = new Vector3(currentTileX, currentTileY, 0f);
        if (!map[currentRowIndex, currentColumnIndex])
        {           
            map[currentRowIndex, currentColumnIndex] = Instantiate(pipes[pipeType], nextTileSpawnPoint, Quaternion.identity);
        }
        
        Tile createdTile = map[currentRowIndex, currentColumnIndex].GetComponent<Tile>();
        createdTile.currentState = startingState;
        createdTile.init();
        createdTile.transform.name = debugCounter.ToString();
        //Debug.Log(debugCounter.ToString() + ".tile, Row :" + currentRowIndex+ " Column: "+ currentColumnIndex);
        debugCounter++;

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




        Debug.Log("Input: "+input + " Output: " + desiredOutput +  " for " + createdTile.transform.name);

        CreateAdjacentTile(createdTile, desiredOutput, currentRowIndex, currentColumnIndex);

    }

    public void MixSolution() //BURASI KESİNLİKLE OPTİMİZE EDİLMELİ
    {
        for(int i = 0; i< rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                if (map[i,j] != null)
                {
                    int randomState = Random.Range(0, possiblePipeStateCount);
                    Tile tile = map[i, j].GetComponent<Tile>();
                    if (tile != null)
                    {
                        for (int x = 0; x < randomState; x++)
                        {
                            tile.manageEdges();
                            tile.currentState = (tile.currentState+1) % possiblePipeStateCount;
                           
                        }
                    }
                }
            }
        }
    }


    /*public void GenerateSolution()
    {
        float currentTileX = firstSpawnPoint.position.x + (columns / 2f * tileLengthX);
        float currentTileY = firstSpawnPoint.position.y - (rows * tileLengthY) + tileLengthY; //burasi + n * tileLengthY falan olabilir

        int currentRowIndex = rows-1;        //finish oncesinden basliyorum;
        int currentColumnIndex = columns/2;
        //Debug.Log(debugCounter.ToString() + ".tile, Row :" + currentRowIndex + " Column: " + currentColumnIndex);

        Vector3 nextTileSpawnPoint = new Vector3(currentTileX, currentTileY, 0f);

        int pipeType = (int) Random.Range(0, pipes.Length);
        //int pipeType = 0;
        int startingState = (int)Random.Range(0, possiblePipeStateCount);
        map[currentRowIndex,currentColumnIndex] = Instantiate(pipes[pipeType], nextTileSpawnPoint, Quaternion.identity);
        Tile createdTile = map[currentRowIndex, currentColumnIndex].GetComponent<Tile>();
        createdTile.transform.name = debugCounter.ToString();
        debugCounter++;    
        createdTile.currentState = startingState;
        createdTile.init();
        while (!createdTile.edges[2])
        {
            createdTile.manageEdges();
            createdTile.currentState = (createdTile.currentState+1) % possiblePipeStateCount;
            
        }

        int output = 0;
        for(int i = 0; i < possiblePipeStateCount; i++)
        {
            if(createdTile.edges[i] && i != 2)
            {
                output = i;
                break;
            }

        }

      
        CreateAdjacentTile(createdTile, output, currentRowIndex, currentColumnIndex);

    }





    public void CreateAdjacentTile(Tile tile, int output, int currentRowIndex, int currentColumnIndex)
    {

        if (tile.gameObject.transform.position == firstSpawnPoint.position && output == 0)
        {
            return;
        }

        if (debugCounter == 5)
        {
            return;
        }
        if ((currentColumnIndex == columns - 1 && output == 1) || (currentColumnIndex == 0 && output == 3))
        {
            return;
        }





        if (output == 0)
        {
            if (currentRowIndex > 0) //YUKARI YARATMAYA CALISIYOR
            {
                currentRowIndex--;
            }

        }

        else if (output == 1)
        {
            if (currentColumnIndex < columns - 1) //SAĞA YARATMAYA CALISIYOR
            {
                currentColumnIndex++;
            }

        }
        else if (output == 2)
        {
            if (currentRowIndex < rows - 1)  //ASAGI YARATMAYA CALISIYOR
            {

                currentRowIndex++;

            }

        }
        else if (output == 3)
        {
            if (currentColumnIndex > 0)   //SOLA YARATMAYA CALISIYOR
            {
                currentColumnIndex--;
            }

        }




        int pipeType = (int)Random.Range(0, pipes.Length);
        //int pipeType = 0;
        int startingState = (int)Random.Range(0, possiblePipeStateCount);

        float currentTileX = firstSpawnPoint.transform.position.x + (currentColumnIndex * tileLengthX);
        float currentTileY = firstSpawnPoint.transform.position.y - (currentRowIndex * tileLengthY);
        Vector3 nextTileSpawnPoint = new Vector3(currentTileX, currentTileY, 0f);
        if (!map[currentRowIndex, currentColumnIndex])
        {
            map[currentRowIndex, currentColumnIndex] = Instantiate(pipes[pipeType], nextTileSpawnPoint, Quaternion.identity);
        }

        Tile createdTile = map[currentRowIndex, currentColumnIndex].GetComponent<Tile>();
        createdTile.transform.name = debugCounter.ToString();
        Debug.Log(debugCounter.ToString() + ".tile, Row :" + currentRowIndex + " Column: " + currentColumnIndex);
        debugCounter++;

        int input = (output + 2) % createdTile.edges.Length;


        if (createdTile != null)
        {
            if (currentRowIndex == 0 || currentRowIndex == rows - 1 || currentColumnIndex == columns - 1 || currentColumnIndex == 0)
            {
                if (currentColumnIndex == 0)
                {
                    while (true)
                    {
                        if (createdTile.edges[input] && !createdTile.edges[3])
                        {
                            break;
                        }
                        createdTile.manageEdges();
                        createdTile.currentState = (createdTile.currentState + 1) % possiblePipeStateCount;

                    }
                }
                else if (currentColumnIndex == columns - 1)
                {
                    while (true)
                    {
                        if (createdTile.edges[input] && !createdTile.edges[1])
                        {
                            break;
                        }
                        createdTile.manageEdges();
                        createdTile.currentState = (createdTile.currentState + 1) % possiblePipeStateCount;

                    }
                }

                if (currentRowIndex == rows - 1)
                {
                    while (true)
                    {
                        if (createdTile.edges[input] && !createdTile.edges[2])
                        {
                            break;
                        }

                        createdTile.manageEdges();
                        createdTile.currentState = (createdTile.currentState + 1) % possiblePipeStateCount;

                    }
                }
                else if (currentRowIndex == 0)
                {
                    while (true)
                    {
                        if (createdTile.edges[input] && !createdTile.edges[0])
                        {
                            break;
                        }
                        createdTile.manageEdges();
                        createdTile.currentState = (createdTile.currentState + 1) % possiblePipeStateCount;
                    }
                }
            }
            else
            {
                bool isDone = false;
                while (!isDone)
                {
                    for (int i = 0; i < possiblePipeStateCount; i++)
                    {
                        if (createdTile.edges[input])
                        {
                            isDone = true;
                            break;
                        }
                        createdTile.manageEdges();
                        createdTile.currentState = (createdTile.currentState + 1) % possiblePipeStateCount;

                    }
                    //pipeType = (pipeType + 1) % pipes.Length;
                    //Destroy(map[currentRowIndex, currentColumnIndex]);
                    //map[currentRowIndex, currentColumnIndex] = null;
                    //map[currentRowIndex, currentColumnIndex] = Instantiate(pipes[pipeType], nextTileSpawnPoint, Quaternion.identity);
                }

            }

        }

        int outputForCreatedTile = 0;

        for (int i = 0; i < possiblePipeStateCount; i++)
        {
            if (createdTile.edges[i] && i != input && i != 2)
            {
                outputForCreatedTile = i;
                break;
            }

        }

        Debug.Log("Output for " + debugCounter.ToString() + " : " + outputForCreatedTile);

        CreateAdjacentTile(createdTile, outputForCreatedTile, currentRowIndex, currentColumnIndex);

    }
    */

}
