using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject player;

    [Header("-----Stats-----")]
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, destroyTime);
        direction = (player.transform.position - transform.position).normalized;
        //Not by framerate so no need for time.deltaTime
        //rb.velocity = player.transform.position * speed;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
