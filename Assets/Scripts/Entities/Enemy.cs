using UnityEngine;
using System;

public class Enemy : PlayableObject
{
    //private string name;
    [SerializeField] protected float speed;
    protected Transform target;

    private EnemyType enemyType;


    protected virtual void Start()
    {
        //handling exception
        try
        {
            target = GameObject.FindWithTag("Player").transform;
        }
        catch(NullReferenceException ex)
        {
            Debug.Log("There is no player in the scene. Enemy destroying self" + ex);
            //Destroy(gameObject);
            GameManager.GetInstance().SetEnemySpawnState(false);
        }
        
    }

    protected virtual void Update()
    {
        if(target != null)
        {
            Move(target.position);
        }
        else
        {
            Move(speed);
        }
    }

    /// <summary>
    /// A generic move method for direction and target
    /// </summary>
    /// <param name="direction"> vector2 direction</param>
    /// <param name="target">vector2 target</param>
    public override void Move(Vector2 direction, Vector2 target)
    {
        //Debug.Log("Enemy is moving");
    }

    public override void Move(float speed) //To be used if there is no player in the scene
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public override void Move(Vector2 direction) //Move the enemy in the direction of the player
    {
        direction.x -= transform.position.x;
        direction.y -= transform.position.y;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public override void Shoot()
    {
        Debug.Log("Shooting a bullet");
    }

    public override void Attack(float interval)
    {
        Debug.Log($"Attacking at {interval} interval");
    }

    /// <summary>
    /// Enemy die method
    /// </summary>
    public override void Die()
    {
        Debug.Log("Enemy died!");
        GameManager.GetInstance().NotifyDeath(this);
        Destroy(gameObject);
    }

    public void SetEnemyType(EnemyType enemyType)
    {
        this.enemyType = enemyType;
    }

    public override void GetDamage(float damage)
    {
        Debug.Log($"Enemy damaged!");
        health.DeductHealth(damage);
        GameManager.GetInstance().scoreManager.IncrementScore();

        if (health.GetHealth() <= 0)
            Die();
    }
}
