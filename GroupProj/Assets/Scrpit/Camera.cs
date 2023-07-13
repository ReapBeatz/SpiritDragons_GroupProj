using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField] int sensitivity;

    [SerializeField] int LockVerMax;
    [SerializeField] int LockVerMin;

    [SerializeField] bool invertY;


    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        // get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
            xRotation += mouseY;
        else
            xRotation -= mouseY;


        // clamp camera rotataion on Xaxis

        xRotation = Mathf.Clamp(xRotation, LockVerMin, LockVerMax);

        //rotate the cam on the Xaxis 

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the player on Yaxis transform.parent affects the parent aka the player

        transform.parent.Rotate(Vector3.up * mouseX);


    }
}
