using System;
using UnityEngine;

public class Player : PlayableObject
{
    private string nickName;
    
    [SerializeField] private Camera cam;
    [SerializeField] private float speed;

    [SerializeField] private float weaponDamage = 1;
    [SerializeField] private float bulletSpeed = 10;
    [SerializeField] private Bullet bulletPrefab;

    public Action OnDeath;

    private Rigidbody2D playerRB;

    private void Awake()
    {
        health = new Health(100, 0.5f, 50);
        playerRB = GetComponent<Rigidbody2D>();

        cam = Camera.main;

        //Setting up our player with a weapon
        weapon = new Weapon("Player Weapon", weaponDamage, bulletSpeed);
    }

    private void Update()
    {
        health.RegenHealth();
    }

    public override void Move(Vector2 direction, Vector2 target)
    {
        playerRB.linearVelocity = direction * speed * Time.deltaTime;

        var playerScreenPos = cam.WorldToScreenPoint(transform.position);
        target.x -= playerScreenPos.x;
        target.y -= playerScreenPos.y;

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void Shoot()
    {
        Debug.Log("Shooting a bullet");
        weapon.Shoot(bulletPrefab, this, "Enemy");
    }

    public override void Attack(float interval)
    {
        Debug.Log($"Attacking at {interval} interval");
    }

    public override void Die()
    {
        Debug.Log("Player died!");
        OnDeath?.Invoke();

        Destroy(gameObject);
    }

    public override void GetDamage(float damage)
    {
        Debug.Log($"Player getting damaged {damage} damage");
        health.DeductHealth(damage);

        if (health.GetHealth() <= 0)
            Die();
    }
}
