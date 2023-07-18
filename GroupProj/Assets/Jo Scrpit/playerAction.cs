using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class playerAction : MonoBehaviour
{
    [SerializeField] private TextMeshPro useText;

    [SerializeField] private Transform Camera;

    [SerializeField] private float MaxUseDist = 5f;

    [SerializeField] private LayerMask useLayer;

    [SerializeField] Door door;

    public void onUse()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, MaxUseDist, useLayer))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                if (door.IsOpen)
                {
                    door.Close();
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
        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, MaxUseDist, useLayer)
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

            useText.gameObject.SetActive(true);
            useText.transform.position = hit.point - (hit.point - Camera.position.normalized * 0.01f); 
            useText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);

        }
        else
        {
            useText.gameObject.SetActive(false);
        }
    }
}
