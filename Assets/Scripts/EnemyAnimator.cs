using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : CharacterAnimator
{
    NavMeshAgent agent;

    protected override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        //animator.SetBool("inCombat", combat.InCombat);
        float speedPercent = agent.velocity.magnitude / agent.speed;

        base.Update();
    }
}
