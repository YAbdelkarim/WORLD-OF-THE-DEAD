using UnityEngine;

public class EnemyVitals : Vitals
{

    EnemyBehavior enemyBehavior;
    Gun gun;
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

    public void BecomeInvincible()
    {
        gun.damage = 0;
    }
    public void BecomeVulnerable()
    {
        gun.damage = 10;
    }

}

