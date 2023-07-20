using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class wpnStats : ScriptableObject
{
    public int shootDist;
    public float fireRate;
    public int shootDmg;
    public int ammoCur;
    public int ammoMax;

    public GameObject model;

}
