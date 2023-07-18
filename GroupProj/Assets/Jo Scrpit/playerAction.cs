using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAction : MonoBehaviour
{
    [SerializeField] private TextMeshPro useText;

    [SerializeField] private Transform Camera;

    [SerializeField] private float MaxUseDist = 5f;

    [SerializeField] private LayerMask useLayer;


    public void onUse()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, RaycastHit Hit, MaxUseDist, useLayer))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                if (door.IsOpen)
                {
                    door.close();
                }
                else
                {
                    door.Open(transform.position);
                }

            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, RaycastHit Hit, MaxUseDist, useLayer)
            && hit.collider.TryGetComponent<Door>(out Door door))
        {
            if(door.IsOpen)
            {
                useText.SetText("Close E") ;
            }
            else 
            {
                useText.SetText("Open E") ;
            }

            useText.GameObject.SetActive(true);
            useText.transform.position = hit.point - (hit.point - Camera.position.normalized * 0.01f); 
            useText.transform.rotation = Quaterion.LookRotation((hit.point - Camera.position).normalized);

        }
        else
        {
            useText.GameObject.SetActive(false);
        }
    }
}
