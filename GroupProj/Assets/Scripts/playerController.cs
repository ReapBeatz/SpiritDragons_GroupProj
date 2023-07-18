using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
//using System.Net.Sockets;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;

    [Header("-----Player Stats-----")]
    [Range(0, 10)][SerializeField] int HP;
    [SerializeField] float currentPlayerSpeed;
    //[SerializeField] float playerSpeed;
    float playerSpeedOrig;
    [SerializeField] float sprintCharge;
    [SerializeField]float sprintSpeed;
    float sprintChargeOrig;
    [Range(10, 20)][SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpsMax;

    [Header("-----Gun Stats-----")]
    [SerializeField] List<wpnStats> wpnList = new List<wpnStats>();
    [SerializeField] GameObject wpn;
    

    [SerializeField] float fireRate;
    [SerializeField] int shootDamage;
    [SerializeField] float shootDist;

    Vector3 move;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    int jumpCount;
    bool isShooting;
    int HPOrig;
    public int selectedWpn;
    // Start is called before the first frame update
    void Start()
    {
        playerSpeedOrig = currentPlayerSpeed;
        sprintSpeed = currentPlayerSpeed * 2;
        HPOrig = HP;
        sprintChargeOrig = sprintCharge;
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
            if (Input.GetKey(KeyCode.LeftShift) && sprintCharge > 0)
            {
                currentPlayerSpeed = sprintSpeed;
                sprintCharge -= .01f;
                updatePlayerUI();
            }
            else
            {
                if (sprintCharge < sprintChargeOrig)
                {
                    sprintCharge += 1 * Time.deltaTime;
                    updatePlayerUI();
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || sprintCharge == 0)
            {
                currentPlayerSpeed = playerSpeedOrig;
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
        controller.Move(move * Time.deltaTime * currentPlayerSpeed);


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
        gameManager.instance.playerStamBar.fillAmount = sprintCharge / sprintChargeOrig;
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        HP = HPOrig;
        updatePlayerUI();
    }

    public void wpnPickUp(wpnStats wpnstat)
    {
        wpnList.Add(wpnstat);

        shootDamage = wpnstat.shootDmg;
        shootDist = wpnstat.shootDist;
        fireRate = wpnstat.fireRate;

        wpn.GetComponent<MeshFilter>().mesh = wpnstat.model.GetComponent<MeshFilter>().sharedMesh;
        wpn.GetComponent<MeshRenderer>().material = wpnstat.model.GetComponent<MeshRenderer>().sharedMaterial;
        selectedWpn = wpnList.Count - 1;
        updatePlayerUI();
    }

    void scrollWpns()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWpn < wpnList.Count - 1) 
        {
            selectedWpn++;
            changeWpnStats();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWpn > 0)
        {
            selectedWpn--;
            changeWpnStats();
        }
    }

    void changeWpnStats()
    {
        shootDamage = wpnList[selectedWpn].shootDmg;
        shootDist = wpnList[selectedWpn].shootDist;
        fireRate = wpnList[selectedWpn].fireRate;
        wpn.GetComponent<MeshFilter>().mesh = wpnList[selectedWpn].model.GetComponent<MeshFilter>().sharedMesh;
        wpn.GetComponent<MeshRenderer>().material = wpnList[selectedWpn].model.GetComponent<MeshRenderer>().sharedMaterial;
    }

}
