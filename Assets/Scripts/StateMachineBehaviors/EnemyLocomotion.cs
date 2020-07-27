using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : StateMachineBehaviour
{

	//public float speed = 2.5f;
	//public float attackRange = 3f;

	Transform target;

    public float lookRadius = 10f;

    public NavMeshAgent agent;
    CharacterCombat myCombat;
    Vector3 originalPosition;
    public float Distance;
    const float locomationAnimationSmoothTime = .1f;
    Animator anim;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        //target = PlayerManager.instance.player.transform;
        anim = animator;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        float distance = Vector3.Distance(target.position, animator.transform.position);
        Distance = distance;
        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                if (targetStats != null)
                {
                    animator.SetTrigger("attack");
                    //combat.DoDamage(targetStats);
                }
                //enemy.FaceTarget(target);
                FaceTarget(target);
            }
        }
        else if (distance > lookRadius * 2)
        {
            //StartCoroutine(StopChasing());
        }

        //different for player, check the player animator script
        float speed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speed, locomationAnimationSmoothTime, Time.deltaTime);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//animator.ResetTrigger("Attack");
	}

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - anim.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        anim.transform.rotation = Quaternion.Slerp(anim.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
