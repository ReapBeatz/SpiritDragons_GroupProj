using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

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
        //get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        //xRotation is the camera's x axis. It takes the useres Y mouse position as that translate to up and down. This will make the user be able to look up and down
        if(invertY)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }

        //clamp the camera rotation on the X-axis
        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        //rotate camera on the X-axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the player on the Y-axis
        // no need to do Time.deltaTime again as it has been done already
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
