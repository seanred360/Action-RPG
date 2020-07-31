using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roundbeargames
{
    [CreateAssetMenu(fileName = "New State", menuName = "Roundbeargames/AbilityData/Attack")]
    public class Attack : StateData
    {

        PlayerManager playerManager;
        Transform myTransform;
        Transform cameraObject;
        Rigidbody rigidbody;
        InputHandler inputHandler;
        AnimatorStateInfo animStateInfo;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool("Attack", false);
        }
    }   
}

