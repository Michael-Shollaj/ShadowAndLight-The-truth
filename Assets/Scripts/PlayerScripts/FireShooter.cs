using UnityEngine;

public class FireShooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float bulletForce = 20f;
    public Animator anim;
    public float fireRate = 1f; // Rate of fire

    private float nextFireTime = 0f; // Time until next fire is allowed

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (enabled && Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        anim.SetTrigger("Shoot");

        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 shootDirection = GetShootDirection();

        rb.AddForce(shootDirection * bulletForce, ForceMode2D.Impulse);
    }

    Vector2 GetShootDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        if (direction == Vector2.zero)
        {
            direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        return direction;
    }
}
