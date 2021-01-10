using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingWaterManager : MonoBehaviour
{
    public LineRenderer water;
    public float offset = 0.1f;
    public float flowSpeed = 3f;
    
    public IEnumerator drawWaterSlow(List<Vector3> positionList)
    {
      
        int passedPipesLength = ProblemSolvingGameManager.instance.pipesPassedTrough.Count;
        water.positionCount = 2;
        water.SetPosition(0, positionList[0]);

        while (water.positionCount < passedPipesLength + 1)
        {
            water.SetPosition(water.positionCount - 1, water.GetPosition(water.positionCount - 2));

            while (Vector3.Distance(water.GetPosition(water.positionCount - 1), positionList[water.positionCount - 1]) >= offset)
            {
                Vector3 nextTransform = Vector3.MoveTowards(water.GetPosition(water.positionCount - 1), positionList[water.positionCount - 1], flowSpeed * Time.deltaTime);
                //nextTransform.z = 0f;
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
            Debug.Log("Passed pipes length: " + passedPipesLength + " Line size:" + water.positionCount);
        }

    }
    

    public void drawWater(List<Vector3> positionList)
    {
        int passedPipesLength = positionList.Count;

        water.positionCount = passedPipesLength;
        for (int i = 0; i < passedPipesLength; i++)
        {
            water.SetPosition(i, positionList[i]);
        }
    }

}
