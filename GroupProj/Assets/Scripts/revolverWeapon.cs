using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class revolverWeapon : MonoBehaviour
{
    //[SerializeField] Impact impactScript;
    [SerializeField] Animator animator;

    [SerializeField] int damage;
    [SerializeField] float range = 100f;
    [SerializeField] float fireRate = 5f;

    //int layerMask = 7;

    [SerializeField] int clipAmmo = 5;
    public int totalAmmo = 40;
    int neededAmmo;
    public int currentAmmo;
    public float reloadTime;
    private bool isReloading = false;
    bool isShooting;
    //private bool overcharged = false;

    [SerializeField] Transform muzzle;
    [SerializeField] Camera fpsCam;
    //[SerializeField] ParticleSystem muzzleFlash;
    //[SerializeField] GameObject impactEffect;
    //[SerializeField] float inaccuracyDistance = 5f;
    //[SerializeField] float fadeDuration = 0.3f;
    //[SerializeField] float nextTimeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //animator.enabled = false;
        animator.SetTrigger("useGun");
        isReloading = false;
        animator.ResetTrigger("Shoot");
        animator.SetBool("isReloading", false);
        //animator.enabled = true;
    }

    private void OnDisable()
    {
        animator.SetTrigger("toIdle");
    }
    // Update is called once per frame
    void Update()
    {
        //if (currentAmmo <= 0)
        //{
        //    StartCoroutine(Reload());
        //    return;
        //}
        if (Input.GetKeyDown(KeyCode.R)||currentAmmo <= 0 && totalAmmo != 0)
        {
            StartCoroutine(Reload());
        }
        if (Input.GetButtonDown("Shoot") && !isShooting && !isReloading)//Time.time >= nextTimeToFire)
        {
            //nextTimeToFire = Time.time + 1f / fireRate;
            StartCoroutine(shoot());
        }
        //if (overcharged)
        //{
        //    damage = overDamage;
        //}
        //else
        //{
        //    damage = defaultDamage;
        //}
        //if (currentAmmo == clipAmmo)
        //{
        //    overcharged = false;
        //}
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("isReloading", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("isReloading", false);
        //yield return new WaitForSeconds(.1f);
        if (totalAmmo >= clipAmmo)
        {
            //overcharged = false;
            neededAmmo = clipAmmo - currentAmmo;
            currentAmmo += neededAmmo;
        }
        if (totalAmmo <= clipAmmo)
        {
            currentAmmo += totalAmmo;
            totalAmmo = 0;
        }
        else
        {
            totalAmmo -= neededAmmo;
        }
        isReloading = false;
        //if (currentAmmo > clipAmmo)
        //{
        //    overcharged = true;
        //}
    }

    IEnumerator shoot()
    {
        animator.SetTrigger("Shoot");
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, range))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(damage);
            }
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
    //void Shoot()
    //{
    //    animator.SetTrigger("Shoot");
    //    muzzleFlash.Play();
    //    currentAmmo--;
    //
    //    RaycastHit hit;
    //    Vector3 shootingDir = GetShootingDirection();
    //    if (Physics.Raycast(fpsCam.transform.position, shootingDir, out hit, range, layerMask))
    //    {
    //        Debug.Log(hit.transform.name);
    //        //CreateLaser(hit.point);
    //        LaserVoid laserVoid = hit.transform.GetComponent<LaserVoid>();
    //        if (laserVoid != null)
    //        {
    //            laserVoid.TakeDamage(damage);
    //        }
    //
    //        BasicVoid basicVoid = hit.transform.GetComponent<BasicVoid>();
    //        if (basicVoid != null)
    //        {
    //            basicVoid.TakeDamage(damage);
    //        }
    //
    //        GroundVoid groundVoid = hit.transform.GetComponent<GroundVoid>();
    //        if (groundVoid != null)
    //        {
    //            groundVoid.TakeDamage(damage);
    //        }
    //
    //        FlyingVoid flyingVoid = hit.transform.GetComponent<FlyingVoid>();
    //        if (flyingVoid != null)
    //        {
    //            flyingVoid.TakeDamage(damage);
    //        }
    //
    //        SpiderVoid spiderVoid= hit.transform.GetComponent<SpiderVoid>();
    //        if (spiderVoid != null)
    //        {
    //            spiderVoid.TakeDamage(damage);
    //        }
    //
    //        GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
    //        Destroy(impactGO, 0.2f);
    //    }
    //}
    //Vector3 GetShootingDirection()
    //{
    //    Vector3 targetPos = fpsCam.transform.position + fpsCam.transform.forward * range;
    //    targetPos = new Vector3(targetPos.x + Random.Range(-inaccuracyDistance, inaccuracyDistance), targetPos.y + Random.Range(-inaccuracyDistance, inaccuracyDistance), targetPos.z + Random.Range(-inaccuracyDistance, inaccuracyDistance));
    //    Vector3 direction = targetPos - fpsCam.transform.position;
    //    return direction.normalized;
    //}
    //void CreateLaser(Vector3 end)
    //{
    //    impactScript.lr.SetPositions(new Vector3[2] { muzzle.position, end });
    //    StartCoroutine(FadeLaser(impactScript.lr));
    //}
    //
    //IEnumerator FadeLaser(LineRenderer lr)
    //{
    //    float alpha = 1;
    //    while (alpha > 0)
    //    {
    //        alpha -= Time.deltaTime / fadeDuration;
    //        lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
    //        lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alpha);
    //        yield return null;
    //    }
    //}
}
