using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roundbeargames
{
    public class InputHandler : Singleton<InputHandler>
    {

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool jump_Input;
        public bool attack_Input;
        public bool inventory_Input;
        public bool interact_Input;

        public bool jumpFlag, sprintFlag, rollFlag, attackFlag, inventoryFlag, interactFlag;
        public float rollInputTimer, menuInputTimer;

        public bool canMove = true;

        public PlayerControls inputActions;

        Vector2 movementInput;
        Vector2 cameraInput;

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }


        private void FixedUpdate()
        {
            TickInput(Time.deltaTime);
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleJumpInput(delta);
            HandleAttackInput(delta);
            HandleInventoryInput(delta);
            HandleInteractInput(delta);
        }

        private void MoveInput(float delta)
        {
            if(canMove)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
        }

        private void HandleRollInput(float delta)
        {
            //prevent sprinting while rolling
            if(canMove)
            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (b_Input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if(rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        void HandleJumpInput(float delta)
        {
            jump_Input = inputActions.PlayerActions.Jump.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if(jump_Input)
            {
                jumpFlag = true;
            }
        }

        void HandleAttackInput(float delta)
        {
            attack_Input = inputActions.PlayerActions.Attack.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if(attack_Input)
            {
                attackFlag = true;
            }
        }

        void HandleInventoryInput(float delta)
        {
            inventory_Input = inputActions.Menu.Inventory.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (inventory_Input)
            {
                menuInputTimer += delta;
                inventoryFlag = false;
            }
            else
            {
                if (menuInputTimer > 0 && menuInputTimer < 0.2f)
                {
                    inventoryFlag = true;
                }

                menuInputTimer = 0;
            }
        }

        void HandleInteractInput(float delta)
        {
            interact_Input = inputActions.PlayerActions.Interact.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (interact_Input)
            {
                interactFlag = true;
            }
        }

        private void LateUpdate()
        {
            rollFlag = false;
            sprintFlag = false;
            jumpFlag = false;
            attackFlag = false;
            inventoryFlag = false;
            interactFlag = false;
        }
    }  
}