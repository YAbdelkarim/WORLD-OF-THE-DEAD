using UnityEngine;

public class EnemyVitals : Vitals
{

    EnemyBehavior enemyBehavior;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        enemyBehavior = GetComponent<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        enemyBehavior.Aggro();
    }

    public override void Die()
    {
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }
    }
}
