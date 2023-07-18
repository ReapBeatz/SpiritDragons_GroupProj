using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool IsOpen = false;

    [SerializeField] private bool rotateDoor = true;
    [SerializeField] private float speed = 1f;

    [Header("-----Rotation Trigs-----")]
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float forwardDirection = 0;

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
                float dot = Vector3.Dot(forward, (UserPosition - transform.position).normalized);
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

   private IEnumerator DoRotationOpen(float forwardAmount)
   {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if(forwardAmount >= forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y - rotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y + rotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;

        }

   }

    public void Close()
    {
        if(IsOpen)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (rotateDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(90, 0, 0);

        IsOpen = false;

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;

        }

    }
}
