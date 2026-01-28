using UnityEngine;

public class EnemyMask : MonoBehaviour
{
    [SerializeField] private GameObject mask;
    [SerializeField] private float maskHealth = 50f;

    private EnemyVitals enemyVitals;
    private bool isMaskDestroyed = false;

    void Start()
    {
        // Finding components
        if (mask == null) mask = transform.Find("Mask")?.gameObject;
        enemyVitals = GetComponent<EnemyVitals>();

        // Start as invincible if the mask exists
        if (enemyVitals != null && !isMaskDestroyed)
        {
            enemyVitals.BecomeInvincible();
        }
    }

    // Call this method from your player's combat script
    public void TakeMaskDamage(float damage)
    {
        if (isMaskDestroyed) return;

        maskHealth -= damage;

        if (maskHealth <= 0)
        {
            BreakMask();
        }
    }

    private void BreakMask()
    {
        isMaskDestroyed = true;

        if (mask != null)
        {
            Destroy(mask);
        }

        if (enemyVitals != null)
        {
            enemyVitals.BecomeVulnerable();
        }

        Debug.Log("The enemy's mask shattered!");
    }
}