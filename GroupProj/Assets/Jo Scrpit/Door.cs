using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool IsOpen = false;

    [SerializedField] private bool rotateDoor = true;
    [SerializedField] private float speed = 1f;

    [Header("-----Rotation Trigs-----")]
    [SerializedField] private float rotationAmount = 90f;
    [SerializedField] private float forwardDirection = 0;

    private Vector3 startRotation;
    private Vector3 forward;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
        forward = transform.right;

    }

    public void Open(Vector3 UserPosition)
    {
        if(IsOpen)
        {
            if(AnimationCoroutine != null) 
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (rotateDoor)
            {
                float dot = Vector3.Dor(forward, (UserPosition - transform.position).normalized);
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

   private IEnumerator DoRotationOpen(float forwardAmount)
   {
        Quarternion startRotation = transform.rotation;
        Quarternion endRotation;

        if(forwardAmount >= forwardDirection)
        {
            endRotation = Quaterion.Euler(new Vector3(0, startRotation.y - rotationAmount, 0));
        }
        else
        {
            endRotation = Quaterion.Euler(new Vector3(0, startRotation.y + rotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yeild return null;
            time += time.deltaTime * speed;

        }

   }

    public void Close()
    {
        if(IsOpen)
        {
            if(AnimationCoroutine != null)
            {
                StopCorountine (AnimationCoroutine);
            }

            if (rotateDoor)
            {
                AnimationCoroutine = StartCorountine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaterion startRotation = transform.rotation;
        Quaterion enRotation = Quaternion.fuler(startRotation);

        IsOpen = false;

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += time.deltaTime * speed;

        }

    }
}
