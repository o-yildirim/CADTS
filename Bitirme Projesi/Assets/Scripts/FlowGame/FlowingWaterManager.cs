using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingWaterManager : MonoBehaviour
{
    public LineRenderer water;
    public ParticleSystem waterEffect;
    public float offset = 0.1f;
    public float flowSpeed = 3f;
    public GameObject finalPointIfFails;
    public AudioSource waterSoundSource;

    public GameObject cross;
    public float crossDuration = 0.5f;

    private void Start()
    {

        waterSoundSource = ProblemSolvingGameManager.instance.sink.GetComponent<AudioSource>();
    }

    public IEnumerator DrawWaterSlow(List<GameObject> positionList,Coroutine valveRotating)
    {
        yield return valveRotating;
        waterSoundSource.enabled = true;     
        waterSoundSource.Play();

        if (!ProblemSolvingGameManager.instance.fullyLinked)
        {
            if (positionList.Count > 1)
            {
                int input;
                int output=0;

                Tile lastPipe = positionList[positionList.Count - 1].GetComponent<Tile>();
                Vector3 pipeBeforeTheLastPosition = positionList[positionList.Count - 2].transform.position;

                float xDifference = lastPipe.transform.position.x - pipeBeforeTheLastPosition.x;
              
                if (xDifference == 0f)
                {
                   float yDifference = lastPipe.transform.position.y - pipeBeforeTheLastPosition.y;
                   if(yDifference > 0)
                   {
                        input = 2;
                   }
                   else
                   {
                        input = 0;
                   }
                }
                else if (xDifference > 0f)
                {
                    input = 3;   
                }
                else
                {
                    input = 1;    
                }
                    
                for(int i = 0; i < lastPipe.edges.Length; i++)
                {
                    if(lastPipe.edges[i] && i != input)
                    {
                        output = i;
                        break;
                    }
                }

               // Debug.Log("Input: " + input + "  Output: " + output);

                float xOffset = 0f;
                float yOffset = 0f;
                float xLength = lastPipe.GetComponent<BoxCollider>().size.x;
                float yLength = lastPipe.GetComponent<BoxCollider>().size.y;

              //  Debug.Log("xLength: " + xLength + "  yLength: " + yLength);
       

                if (output == 0)
                {
                    yOffset += yLength;
                }
                else if(output == 1)
                {
                    xOffset += xLength;
                }
                else if(output == 2)
                {
                    yOffset -= yLength;
                }
                else // 3
                {
                    xOffset -= xLength;
                }

                Vector3 transformForEmpty = positionList[positionList.Count-1].transform.position + new Vector3(xOffset/2f, yOffset/2f, 0f);
                finalPointIfFails.transform.position = transformForEmpty;
                positionList.Add(finalPointIfFails);
            }
        }


        waterEffect.Play();


        int passedPipesLength = positionList.Count;

        if (passedPipesLength > 2)
        {
            water.positionCount = 2;
            water.SetPosition(0, positionList[0].transform.position);

            while (water.positionCount < passedPipesLength + 1)
            {
                water.SetPosition(water.positionCount - 1, water.GetPosition(water.positionCount - 2));

                while (Vector3.Distance(water.GetPosition(water.positionCount - 1), positionList[water.positionCount - 1].transform.position) >= offset)
                {
                    Vector3 nextTransform = Vector3.MoveTowards(water.GetPosition(water.positionCount - 1), positionList[water.positionCount - 1].transform.position, flowSpeed * Time.deltaTime);
                    nextTransform.z = 0f;
                    water.SetPosition(water.positionCount - 1, nextTransform);
                    yield return null;
                }

                if (water.positionCount < passedPipesLength)
                {
                    water.positionCount++;
                }
                else
                {
                    break;
                }

            }
        }
        else
        {
            water.positionCount = 0;
        }
        
       
        if (!ProblemSolvingGameManager.instance.fullyLinked)
        {
          
            if (positionList.Count > 1)
            {
                cross.transform.position = finalPointIfFails.transform.position;
            
            }
            else
            {
                cross.transform.position = ProblemSolvingGameManager.instance.mapGenerator.firstSpawnPoint.position;
                
            }
            cross.SetActive(true);
            yield return new WaitForSeconds(crossDuration);
            cross.SetActive(false);
        }



    }

    public void ResetElements()
    {
        water.positionCount = 0;
        waterEffect.Stop();
    }

    /*public void drawWater(List<Vector3> positionList)
    {
        int passedPipesLength = positionList.Count;

        water.positionCount = passedPipesLength;
        for (int i = 0; i < passedPipesLength; i++)
        {
            water.SetPosition(i, positionList[i]);
        }
    }*/

}
