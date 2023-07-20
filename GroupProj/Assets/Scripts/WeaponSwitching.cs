using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    int selectedWeapon = 0;
    [SerializeField] Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        SelectedWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(selectedWeapon.ToString());
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
        }
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    selectedWeapon = 3;
        //}

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectedWeapon();
        }
        switch (selectedWeapon) 
        {
            case 0:
                //Debug.Log("TakingOut Knife");
                playerAnimator.ResetTrigger("useGun");
                playerAnimator.SetBool("isReloading", false);
                playerAnimator.ResetTrigger("useCBar");

                break;
            case 1:
                //Debug.Log("TakingOut Gun");
                playerAnimator.ResetTrigger("takeOutKnife");
                playerAnimator.ResetTrigger("useCBar");
                break;
            case 2:
                //Debug.Log("TakingOut Crowbar");
                playerAnimator.ResetTrigger("takeOutKnife");
                playerAnimator.ResetTrigger("useGun");
                playerAnimator.SetBool("isReloading", false);
                break;
        }       
    }

    void SelectedWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
