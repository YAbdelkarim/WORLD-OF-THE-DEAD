using UnityEngine;

public class Animation : MonoBehaviour
{
    Animator animator;
    int isIdle = 0;
    int isWalking = 1;
    //int isRunning = 2;
    int isStagger = 3;
    int isPunching = 4;

    EnemyState state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        state = GetComponent<EnemyBehavior>().state;
        switch (state)
        {
            case EnemyState.IDLE:
                animator.SetInteger("State", isIdle);
                break;
            case EnemyState.AGGRO:
                animator.SetInteger("State", isWalking);
                break;
            case EnemyState.ATTACKING:
                animator.SetInteger("State", isPunching);
                break;
            case EnemyState.DYING:
                animator.SetInteger("State", isStagger);
                break;
        }
    }
   

}
