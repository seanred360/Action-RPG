using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roundbeargames
{
    [CreateAssetMenu(fileName = "New State", menuName = "Roundbeargames/AbilityData/MoveForward")]
    public class MoveForward : StateData
    {

        PlayerController playerManager;
        Transform myTransform;
        Transform cameraObject;
        Rigidbody rigidbody;
        InputHandler inputHandler;
        AnimatorStateInfo animStateInfo;

        [Header("Movement Stats")]
        [SerializeField]
        float walkSpeed = 5;
        [SerializeField]
        float sprintSpeed = 10;
        [SerializeField]
        float fallingSpeed = 45;

        public AnimationCurve SpeedGraph;
        Vector3 moveDir;

        float groundDetectionRayStartPoint;
        float minimumDistanceNeededToBeginFall;
        float groundDirectionRayDistance = .2f;
        LayerMask ignoreForGroundCheck;
        Vector3 targetPosition;
        Vector3 normalVector;

        float turnSmootheVelocity = 0;
        float turnSmoothTime = 0;



        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            playerManager = characterStateBase.GetPlayerManager(animator);
            animStateInfo = stateInfo;

            myTransform = playerManager.myTransform;
            cameraObject = playerManager.cameraObject;
            rigidbody = playerManager.rigidbody;
            inputHandler = InputHandler.Instance;

            animator.SetBool("Roll", inputHandler.rollFlag);
            animator.SetBool("Jump", inputHandler.jumpFlag);
            animator.SetBool("Attack", inputHandler.attackFlag);

            #region Interact

            Vector3 forward = myTransform.TransformDirection(Vector3.forward) * 1f;
            Debug.DrawRay(myTransform.position, forward, Color.green);

            if (inputHandler.interactFlag)
            {
                Ray ray = new Ray(myTransform.position, forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        Debug.Log("focused");
                        interactable.OnFocused(myTransform);
                    }
                }
            }
            #endregion

            HandleMovement(Time.deltaTime);
        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.moveAmount >= .1f)
            {
                Vector3 direction = new Vector3(inputHandler.horizontal, myTransform.position.y, inputHandler.vertical).normalized;
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraObject.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(myTransform.eulerAngles.y, targetAngle, ref turnSmootheVelocity, turnSmoothTime);
                myTransform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                Debug.DrawRay(myTransform.position, moveDir, Color.red);

                float moveSpeed;

                if (inputHandler.sprintFlag) { moveSpeed = sprintSpeed; } else { moveSpeed = walkSpeed; }

                myTransform.position += moveDir * moveSpeed * Time.deltaTime;
            }

            playerManager.UpdateAnimatorValues(inputHandler.moveAmount, 0);
        }
 
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }
}

