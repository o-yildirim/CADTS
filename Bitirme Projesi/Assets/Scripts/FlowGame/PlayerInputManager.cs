using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    
    void Update()
    {
        if (ProblemSolvingGameManager.instance.inputUnavailable)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            { 
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Tile"))
                {
                    Tile hitTile = hit.transform.GetComponent<Tile>();
                    if (hitTile != null)
                    {
                        hitTile.RotationCall();
                        FlowStatisticManager.instance.IncrementRotation(hitTile);
                    }
                }
                else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Sink"))
                {
                    ProblemSolvingGameManager.instance.startCheckingSequence();
                }
            }
        }
        
    }
}
