using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int totalStates = 4;
    public int currentState = 0;
    public Dictionary<int, float> stateAngleMatch;
    //public Dictionary<int, bool[]> stateEdgeMatch;


    public bool[] edges = { false, false, false, false };
    public float[] stateAngles = { 0f, -90f, -180f, -270f };
    public bool Ltype;
    public bool Itype;

    public float rotationRate = 0.75f;
  
    void Start()
    {             
  
        init();
    }

    public void RotationCall()
    {
        StartCoroutine(Rotate(rotationRate));
    }

    public IEnumerator Rotate(float rotationRate)
    {

        //Debug.Log("Rotate called");
       
        int nextState =  (currentState + 1) % totalStates;
        

        //float currentZ = states[stateCurrent];
        float currentZ = (transform.rotation.eulerAngles.z - 360f ) % 360;
        float targetZ = stateAngleMatch[nextState];
        float t = 0;
     
        //Debug.Log("Current Z:" +currentZ);
        //Debug.Log("Euler z: " + (transform.rotation.eulerAngles.z - 360f) % 360);
        //Debug.Log("Target Z:" + targetZ);

        

        while ((transform.rotation.eulerAngles.z - 360f) % 360 >= targetZ + 0.03f || (transform.rotation.eulerAngles.z - 360f) % 360 <= targetZ - 0.03f)
        {
           
            t += rotationRate * Time.deltaTime;
            float nextZ = Mathf.Lerp(currentZ, targetZ, t);
            transform.rotation = Quaternion.Euler(0f, 0f, nextZ);
            yield return null;
        }

        manageEdges();

        currentState = nextState;

    }

    public void AssignStateDictionary()
    {
        stateAngleMatch = new Dictionary<int, float>();

        for (int i = 0; i< totalStates; i++)
        {
            stateAngleMatch.Add(i, stateAngles[i]);
        }
    }

    public void init()
    {
        AssignStateDictionary();

        if (Ltype)
        {
            edges[currentState] = true;
            edges[(currentState + 1) % totalStates] = true;
        }
        else if (Itype)
        {
            edges[currentState] = true;
            edges[(currentState + 2) % totalStates] = true;
        }


        transform.rotation = Quaternion.Euler(0f, 0f, stateAngleMatch[currentState]);
    }

    public void manageEdges()
    {
        int stateBeingPreparedTo = currentState + 1;

        if (Ltype)
        {
            edges[currentState] = false;

            edges[(stateBeingPreparedTo) % totalStates] = true;
            edges[(stateBeingPreparedTo + 1) % totalStates] = true;
        }
        else if (Itype)
        {
            edges[currentState] = false;
            edges[(currentState+2) % totalStates] = false;

            edges[stateBeingPreparedTo % totalStates] = true;
            edges[(stateBeingPreparedTo + 2) % totalStates] = true;
        }
    }


    public void checkTransmission(int inputEdge)
    {     
        

        if (!edges[(inputEdge)])
        {
            return;
        }
        Debug.Log(transform.name);
        ProblemSolvingGameManager.instance.pipesPassedTrough.Add(transform.position);


        int outputEdge = 0;
        for (int i = 0; i < edges.Length; i++)
        {
            if (edges[i] && i != inputEdge)
            {
                outputEdge = i;
                break;
            }
        }

        //Debug.Log(outputEdge);

        RaycastHit hit = new RaycastHit();
        switch (outputEdge)
        {
            case 0:
                if (!Physics.Raycast(transform.position, Vector2.up, out hit))
                {
                    return;
                }       
                break;
            case 1:
                if (!Physics.Raycast(transform.position, Vector2.right, out hit))
                {
                    return;
                }         
                break;
            case 2:
                if (!Physics.Raycast(transform.position, -Vector2.up, out hit))
                {
                    return;
                }
                break;
            case 3:
                if (!Physics.Raycast(transform.position, -Vector2.right, out hit))
                { 
                    return;
                }
                break;
        }

        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
           Tile hitTile = hit.transform.GetComponent<Tile>();
           if (hitTile != null)
           {
               // Debug.Log("Send from " +outputEdge+". edge.");
               // Debug.Log("To "+(outputEdge + 2) % totalStates + ". edge.");
                hitTile.checkTransmission((outputEdge + 2 ) % totalStates);
            }
         }  
         else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Finish"))
         {
           ProblemSolvingGameManager.instance.pipesPassedTrough.Add(hit.transform.position);
           ProblemSolvingGameManager.instance.fullyLinked = true;
         }
        
     
    }



}
