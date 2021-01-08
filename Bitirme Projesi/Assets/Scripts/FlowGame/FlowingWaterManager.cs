using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingWaterManager : MonoBehaviour
{
    public LineRenderer water;

    /*
    public IEnumerator drawWater(List<Vector3> positionList)
    {
        int passedPipesLength = ProblemSolvingGameManager.instance.pipesPassedTrough.Count;

        water.positionCount = passedPipesLength;
        for (int i = 0; i < passedPipesLength; i++)
        {
            water.SetPosition(i, ProblemSolvingGameManager.instance.pipesPassedTrough[i]);
        }

        yield return null;
        
    }
    */

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
