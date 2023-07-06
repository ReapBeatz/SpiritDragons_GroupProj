using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
   


    [Header("-----Stats-----")]
    [Range(1, 10)][SerializeField] int hp;
    [Range(10, 360)][SerializeField] int viewAngle;
    [Range(1, 8)][SerializeField] int playerFaceSpeed;
    [SerializeField] int roamTimer;
    [SerializeField] int roamDist;

    [Header("-----Attack Stats-----")]
    [SerializeField] float attackRate;
    [SerializeField] int damage;
    float lastAttackTime = 0;
    //[SerializeField] Transform shootPos;

    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    bool isAttacking;
    float stoppingDistanceOrig;
    Vector3 startingPos;
    bool destinationChosen;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stoppingDistanceOrig = agent.stoppingDistance;
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(agent.remainingDistance);
        // Very basic enemy movement towards player that doesn't take walls and obstacles into account. Not using NavMesh Agent
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime);
        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (agent.destination != gameManager.instance.player.transform.position)
        {
            StartCoroutine(roam());
        }
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);
            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistanceOrig;
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistanceOrig;
                if (agent.remainingDistance >= agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (!isAttacking && agent.remainingDistance <= agent.stoppingDistance)
                {
                    StartCoroutine(attack());
                }

                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    IEnumerator attack()
    {
        if (Time.time - lastAttackTime >= attackRate)
        {
            IDamage damageable = player.GetComponent<IDamage>();
            if (damageable != null)
            {
                lastAttackTime = Time.time;
                damageable.takeDamage(damage);
            }
        }
        yield return new WaitForSeconds(attackRate/2);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void takeDamage(int amount)
    {
        hp -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);
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
