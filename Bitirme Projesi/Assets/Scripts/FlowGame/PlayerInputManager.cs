using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{

    public LayerMask clickAvaliableLayer;
  
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, Mathf.Infinity,clickAvaliableLayer))
            { 
                Tile hitTile = hit.transform.GetComponent<Tile>();
                if(hitTile != null)
                {             
                    hitTile.RotationCall();               
                }
            }
        }
        
    }
}
