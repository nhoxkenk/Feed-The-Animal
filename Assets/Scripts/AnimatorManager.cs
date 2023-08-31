using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    int static_B;
    int speed_F;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        static_B = Animator.StringToHash("Static_b");
        speed_F = Animator.StringToHash("Speed_f");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float VerticalMovement)
    {
        if(horizontalMovement > 0 || VerticalMovement > 0)
        {
            animator.SetBool(static_B, false);
            animator.SetFloat(speed_F, 0.5f);
        }
        if(horizontalMovement > 0.5 || VerticalMovement > 0.5)
        {
            animator.SetFloat(speed_F, 1f);
        }
        else
        {
            animator.SetBool(static_B, true);
            animator.SetFloat(speed_F, 0);
        }
        
    }

    public void setDeathAnimation()
    {
        animator.SetBool("Death_b", true);
        animator.SetInteger("DeathType_int", 1);
    }
}
