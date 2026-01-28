using UnityEngine;

public abstract class Vitals : MonoBehaviour
{

    public int hp;
    public int maxHp;
    public int invFrames;
    int hp;
    public int maxHp;
    int invFrames;
    public int maxInvFrames;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        SetHp(GetMaxHp());
        SetInvFrames(0);
        SetInvFrames(GetMaxInvFrames());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (GetInvFrames() > 0)
        {
            SetInvFrames(GetInvFrames() - 1);
        }
    }

    public int GetHp() { return hp; }

    public int GetMaxHp() { return maxHp; }

    public int GetInvFrames() { return invFrames; }

    public int GetMaxInvFrames() { return maxInvFrames; }

    public void SetHp(int hp)
    {
        if (hp >= 0)
        {
            this.hp = hp;
        }
    }

    public void SetInvFrames(int invFrames)
    {
        if (invFrames >= 0)
        {
            this.invFrames = invFrames;
        }
    }

    public bool IsAlive()
    {
        return GetHp() > 0;
    }

    public void TakeDamage()
    {
        TakeDamage(1);
    }

    public virtual void TakeDamage(int dmg)
    {
        if (GetInvFrames() > 0)
            return;
        if (GetHp() > dmg)
        {
            SetHp(GetHp() - dmg);
        }
        else
        {
            SetHp(0);
        }
        SetInvFrames(GetMaxInvFrames());
    }

    public abstract void Die();
}
