using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrig : MonoBehaviour
{
    
    [Header("-----Triggers-----")]
    [SerializeField] private TextMeshPro useText;

    [SerializeField] private Transform Camera;

    [SerializeField] private float MaxUseDist = 5f;

    [SerializeField] private LayerMask useLayer;


    public void onUse()
    {
        if(Physics.Raycast(Camera.position, Camera.forward,RaycastHit Hit, MaxUseDist, useLayer))
        {
            if(hit.collider.TryGetComponent<Door>)
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
