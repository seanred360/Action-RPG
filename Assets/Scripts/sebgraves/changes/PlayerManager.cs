using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerManager : MonoBehaviour
    {
        //InputHandler inputHandler;
        //CameraHandler cameraHandler;

        [Header("Player Flags")]
        //public bool isSprinting;
        //public bool isInteracting;
        public bool isInAir;
        public bool isGrounded;

        Vector3 normalVector;
        Vector3 targetPosition;
        Vector3 moveDirection;

        int vertical;
        int horizontal;

        public Transform myTransform;
        public Transform cameraObject;
        public Rigidbody rigidbody;
        public PlayerManager playerManager;
        public Animator anim;
        public CharacterController characterController;

        public float inAirTimer;

        private void Awake()
        {
            //cameraHandler = CameraHandler.singleton;
        }

        void Start()
        {
            myTransform = transform;
            cameraObject = Camera.main.transform;
            rigidbody = GetComponent<Rigidbody>();
            playerManager = GetComponent<PlayerManager>();
            characterController = GetComponent<CharacterController>();

            anim = GetComponentInChildren<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (InputHandler.Instance.sprintFlag)
            {
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        //public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        //{
        //    //anim.applyRootMotion = isInteracting;
        //    //anim.SetBool("isInteracting", isInteracting);
        //    anim.CrossFade(targetAnim, 0.2f);
        //}


        //void FixedUpdate()
        //{
        //    //float delta = Time.fixedDeltaTime;
        //    //isInteracting = anim.GetBool("isInteracting");

        //    //if (cameraHandler != null)
        //    //{
        //    //    cameraHandler.FollowTarget(delta);
        //    //    cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        //    //}

        //}

        private void LateUpdate()
        {
            InputHandler.Instance.rollFlag = false;
            InputHandler.Instance.sprintFlag = false;
            InputHandler.Instance.jumpFlag = false;
            //isSprinting = InputHandler.Instance.sprintFlag;

            if(isInAir)
            {
                inAirTimer += Time.deltaTime;
            }
        }
    }
}