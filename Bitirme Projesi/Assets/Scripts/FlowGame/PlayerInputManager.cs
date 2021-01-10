using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{

    public LayerMask clickAvaliableLayer;
    
    // Update is called once per frame
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
