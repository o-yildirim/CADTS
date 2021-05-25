using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public GameObject rotateEffect;

    public int totalStates = 4;
    public int currentState = 0;
    public Dictionary<int, float> stateAngleMatch;
    



    public bool[] edges = { false, false, false, false };
    public float[] stateAngles = { 0f, -90f, -180f, -270f };
    public bool Ltype;
    public bool Itype;

    public float rotationTime = 1.2f;
    float rotationAngle = -90f;

    bool isRotating = false;

    void Start()
    {             
  
        init();
    }

    public void RotationCall()
    {
        if (isRotating) return;
        StartCoroutine(Rotate(rotationAngle,rotationTime));
    }

    public IEnumerator Rotate(float rotationAngle,float rotationTime)
    {


        isRotating = true;
        int nextState =  (currentState + 1) % totalStates;
     

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + (Vector3.forward * rotationAngle));


        for (var t = 0f; t < 1; t += Time.deltaTime / rotationTime)
        {
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f,0f,stateAngleMatch[nextState]);

        manageEdges();
        currentState = nextState;

        GameObject effect = Instantiate(rotateEffect, transform.position,Quaternion.identity);
        Destroy(effect, 0.15f);
        isRotating = false;

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
      
        ProblemSolvingGameManager.instance.pipesPassedTrough.Add(this.gameObject);


        int outputEdge = 0;
        for (int i = 0; i < edges.Length; i++)
        {
            if (edges[i] && i != inputEdge)
            {
                outputEdge = i;
                break;
            }
        }


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
                hitTile.checkTransmission((outputEdge + 2 ) % totalStates);
            }
         }  
         else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Finish"))
         {
           ProblemSolvingGameManager.instance.pipesPassedTrough.Add(hit.transform.gameObject);
           ProblemSolvingGameManager.instance.fullyLinked = true;
         }
        
     
    }



}
