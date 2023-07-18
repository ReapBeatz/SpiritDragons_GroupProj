using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class wpnPickUp : MonoBehaviour
{
    [SerializeField] wpnStats wpn;

    // Start is called before the first frame update
    void Start()
    {
        wpn.ammoCur = wpn.ammoMax;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.wpnPickUp(wpn);
            Destroy(gameObject);
        }
    }
}
