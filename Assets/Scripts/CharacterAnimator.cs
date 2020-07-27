using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterAnimator : MonoBehaviour
{

    protected const float locomationAnimationSmoothTime = .1f;
    protected Animator animator;
    protected CharacterCombat combat;
    protected float speed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        combat = GetComponent<CharacterCombat>();

        //GetComponent<CharacterStats>().OnHealthChanged += OnTakeDamage;
    }

    protected virtual void Update()
    {
        animator.SetFloat("speedPercent", speed, locomationAnimationSmoothTime, Time.deltaTime);
    }

    ////input ints are always blank, this is temporary fix
    //public void OnTakeDamage(int a, int b)
    //{
    //    animator.SetTrigger("takeDamage");
    //}
}
