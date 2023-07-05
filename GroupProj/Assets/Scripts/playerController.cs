using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;

    [Header("-----Player Stats-----")]
    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 7)][SerializeField] float playerSpeed;
    [Range(10, 20)][SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpsMax;

    [Header("-----Gun Stats-----")]
    [SerializeField] float fireRate;
    [SerializeField] int shootDamage;
    [SerializeField] float shootDist;

    Vector3 move;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    int jumpCount;
    bool isShooting;
    int HPOrig;
    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        spawnPlayer();
    }

    void Update()
    {
        if (gameManager.instance.activeMenu == null)
        {
            movement();
            if (Input.GetButton("Shoot") && !isShooting)
            {
                StartCoroutine(shoot());
            }
        }
    }

    void movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpCount = 0;
        }

        move = transform.right * Input.GetAxis("Horizontal") +
               transform.forward * Input.GetAxis("Vertical");
        //when doing movement it's always best to use Time.deltaTime to normalize between varying framerates
        controller.Move(move * Time.deltaTime * playerSpeed);


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpCount < jumpsMax)
        {
            playerVelocity.y += jumpHeight;
            jumpCount++;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(gameManager.instance.playerFlashDamage());
        updatePlayerUI();
        if (HP <= 0)
        {

            gameManager.instance.youLose();
        }
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        HP = HPOrig;
        updatePlayerUI();
    }
}
