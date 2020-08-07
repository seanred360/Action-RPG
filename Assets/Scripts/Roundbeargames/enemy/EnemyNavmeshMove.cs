
using UnityEngine;
using UnityEngine.AI;

namespace Roundbeargames
{
    [CreateAssetMenu(fileName = "New State", menuName = "Roundbeargames/AbilityData/EnemyNavmeshMove")]
    public class EnemyNavmeshMove : StateData
    {

        Transform target;

        public float lookRadius = 10f;

        public NavMeshAgent agent;
        CharacterCombat myCombat;
        Vector3 originalPosition;
        public float Distance;
        const float locomationAnimationSmoothTime = .1f;
        Animator anim;
        

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
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

        public void FaceTarget(Transform target)
        {
            Vector3 direction = (target.position - anim.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            anim.transform.rotation = Quaternion.Slerp(anim.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }
}
