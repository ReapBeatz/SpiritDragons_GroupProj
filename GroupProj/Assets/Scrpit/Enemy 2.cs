using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    [Header("---------- Components ----------")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headpos;

    [Header("---------- Stats ----------")]
    [Range(1, 30)][SerializeField] int HP;
    [SerializeField] Renderer model;
    [Range(10, 360)][SerializeField] int viewangle;
    [SerializeField] int playerfacespeed;
    [SerializeField] int roamTimer;
    [SerializeField] int roamDist;


    [Header("---------- GunStats ----------")]
    [SerializeField] float shotrate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootpos;


    public bool playerinrange;
    Vector3 playerDir;
    float angletoPlayer;
    bool isshooting;
    float stoppingDistianceOrgin;
    Vector3 startingPos;
    bool desitinationChosen;

    void Start()
    {
        GameManager.Instance.updateGameGoal(1);

    }


    void Update()
    {

        if (playerinrange && !Canseeplayer())
        {
            StartCoroutine(roam());
        }
        else if (agent.destination != GameManager.Instance.transform.position)
        {
            StartCoroutine(roam());
        }
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !desitinationChosen)
        {
            desitinationChosen = true;

            yield return new WaitForSeconds(roamTimer);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            desitinationChosen = false;
        }


    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerfacespeed);

    }



    bool Canseeplayer()
    {


        playerDir = GameManager.Instance.player.transform.position - headpos.position;

        angletoPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(headpos.position, playerDir);
        Debug.Log(angletoPlayer);
        playerDir.y = 0;

        RaycastHit hit;
        if (Physics.Raycast(headpos.position, playerDir, out hit))
        {
            if (!hit.collider.CompareTag("Player") && angletoPlayer < viewangle)
            {
                agent.SetDestination(GameManager.Instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (!isshooting)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }

        }
        agent.stoppingDistance = 0;
        return false;

    }

    IEnumerator shoot()
    {
        isshooting = true;
        Instantiate(bullet, shootpos.position, transform.rotation);

        yield return new WaitForSeconds(shotrate);
        isshooting = false;

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinrange = true;

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinrange = false;

        }
    }


    public void takedamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(GameManager.Instance.player.transform.position);
        StartCoroutine(flashdmg());
        if (HP <= 0)
        {
            GameManager.Instance.updateGameGoal(-1);
            Destroy(gameObject);
        }

    }

    IEnumerator flashdmg()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;


    }

}

