using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("---------- Components ----------")]
    [SerializeField] CharacterController controller;

    [Header("---------- PlayerStats ----------")]
    [Range(1, 10)][SerializeField] int Health;
    [Range(1, 10)][SerializeField] float playerSpeed = 2.0f;
    [Range(1, 7)][SerializeField] float jumpHeight = 1.0f;
    [Range(1, 30)][SerializeField] float gravityValue = -9.81f;
    [Range(1, 5)][SerializeField] int jumpmax;

    [Header("---------- GunStats ----------")]
    [SerializeField] float shotrate;
    [SerializeField] int shotdmg;
    [SerializeField] int shotdis;



    Vector3 move;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    int jumpcount;
    bool isshoot;
    int HealthOrig;

    private void Start()
    {
        HealthOrig = Health;
        spawnPlayer();
    }

    void Update()
    {
        if (GameManager.Instance.activeMenu == null)
        {
            Movement();

            if (Input.GetButton("Shoot") && isshoot)
            {
                StartCoroutine(shoot());
            }
        }
    }

    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpcount = 0;
        }


        move = (transform.right * Input.GetAxis("Horizontal")) +
            (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(move * Time.deltaTime * playerSpeed);






        if (Input.GetButtonDown("Jump") && jumpcount < jumpmax)
        {
            playerVelocity.y = jumpHeight;
            jumpcount++;

        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    IEnumerator shoot()
    {
        isshoot = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shotdis))
        {
          
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takedamage(shotdmg);

            }


        }

        yield return new WaitForSeconds(shotrate);
        isshoot = false;

    }

    public void takedamage(int amount)
    {
        Health -= amount;
        StartCoroutine(GameManager.Instance.playerFlashDamage());
        updatePlayer();
        if (Health <= 0)
        {
            GameManager.Instance.youLose();
        }

    }

    public void updatePlayer()
    {
        GameManager.Instance.playerHPBar.fillAmount = (float)Health / HealthOrig;
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = GameManager.Instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        Health = HealthOrig;
        updatePlayer();
    }
}
