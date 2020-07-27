using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(fileName = "New State", menuName = "Roundbeargames/AbilityData/MoveForward")]
    public class MoveForward : StateData
    {

        PlayerManager playerManager;
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
        float rotationSpeed = 200000;
        [SerializeField]
        float fallingSpeed = 45;

        public AnimationCurve SpeedGraph;
        Vector3 moveDirection;

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

            HandleMovement(Time.deltaTime);
            //HandleFalling(Time.deltaTime, moveDirection);
            
        }

        public void HandleMovement(float delta)
        {
            //Vector3 moveDirection = cameraObject.forward * inputHandler.vertical;
            //moveDirection += cameraObject.right * inputHandler.horizontal;
            //moveDirection.Normalize();
            //moveDirection.y = 0;

            //if (inputHandler.sprintFlag || inputHandler.rollFlag)
            //{
            //    moveDirection *= sprintSpeed;
            //}
            //else
            //{
            //    moveDirection *= movementSpeed;
            //}

            //Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, Vector3.zero);

            //rigidbody.velocity = projectedVelocity * SpeedGraph.Evaluate(animStateInfo.normalizedTime * delta);
            if (inputHandler.moveAmount >= .1f)
            {
                //myTransform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

                Vector3 direction = new Vector3(inputHandler.horizontal, myTransform.position.y, inputHandler.vertical).normalized;
                Debug.Log("hor " + inputHandler.horizontal);
                Debug.Log("vert " + inputHandler.vertical);
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

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = myTransform.forward;

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        //    public void HandleRollingAndSprinting(float delta, Animator anim)
        //    {
        //        if (anim.GetBool("Roll"))
        //            return;

        //        if (inputHandler.rollFlag)
        //        {
        //            moveDirection = cameraObject.forward * inputHandler.vertical;
        //            moveDirection += cameraObject.right * inputHandler.horizontal;

        //            if (inputHandler.moveAmount > 0)
        //            {
        //               // animatorHandler.PlayTargetAnimation("Rolling", true);
        //                moveDirection.y = 0;
        //                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
        //                myTransform.rotation = rollRotation;
        //            }
        //            else
        //            {
        //                //animatorHandler.PlayTargetAnimation("Backstep", true);
        //            }
        //        }
        //    }
        //}

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if(playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed / 5f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);

            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (playerManager.inAirTimer > 0.5f)
                    {
                        Debug.Log("in the air" + playerManager.inAirTimer);

                    }
                }
                else
                {
                    playerManager.inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }

            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    //if (playerManager.isInteracting == false)
                    //{
                    //    animatorHandler.PlayTargetAnimation("Falling", true);
                    //}

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (walkSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

                if (playerManager.isGrounded)
                {
                    if (inputHandler.moveAmount > 0)
                    {
                        myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                    }
                    else
                    {
                        myTransform.position = targetPosition;
                    }
                }
            }
        }
    }

