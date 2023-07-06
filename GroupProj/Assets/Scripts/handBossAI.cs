using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class handBossAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    //[SerializeField] NavMeshAgent agent;
    [SerializeField] Transform eyePos;


    [Header("-----Stats-----")]
    [Range(1, 10)][SerializeField] int hp;
    [Range(10, 360)][SerializeField] int viewAngle;
    [SerializeField] int playerFaceSpeed;
    //[SerializeField] int roamTimer;
    //[SerializeField] int roamDist;

    [Header("-----Projectile Stats-----")]
    [SerializeField] float fireRate;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform EyeLPos;
    [SerializeField] Transform EyeLShootPos;
    [SerializeField] Transform EyeRPos;
    [SerializeField] Transform EyeRShootPos;
    [SerializeField] GameObject playerLookDir;

    Vector3 playerDir;
    float angleToPlayer;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        playerLookDir = GameObject.FindGameObjectWithTag("Player");
        gameManager.instance.updateGameGoal(1);
        //stoppingDistanceOrig = agent.stoppingDistance;
       // startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        facePlayer();
        if (canSeePlayer()) ;
    }


    void facePlayer()
    {
        //Quaternion rot = Quaternion.LookRotation(new Vector3(playerLookDir.transform.position.x, playerLookDir.transform.position.y, playerLookDir.transform.position.z));
        //EyeLPos.transform.rotation = Quaternion.Lerp(EyeLPos.transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
        //EyeRPos.transform.rotation = Quaternion.Lerp(EyeRPos.transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
        EyeLPos.transform.LookAt(playerLookDir.transform.position);
        EyeRPos.transform.LookAt(playerLookDir.transform.position);
    }


    bool canSeePlayer()
    {
        //agent.stoppingDistance = stoppingDistanceOrig;
        playerDir = gameManager.instance.player.transform.position - eyePos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y, playerDir.z), transform.forward);
        Debug.Log(angleToPlayer);
        Debug.DrawRay(eyePos.position, playerDir);
        RaycastHit hit;
        if (Physics.Raycast(eyePos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle)
            {
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }

                return true;
            }
        }
        //agent.stoppingDistance = 0;
        return false;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(projectile, EyeLShootPos.position, EyeLShootPos.transform.rotation);
        Instantiate(projectile, EyeRShootPos.position, EyeRShootPos.transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
    public void takeDamage(int amount)
    {
        hp -= amount;
        //agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());
        if (hp <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.black;
    }
}
