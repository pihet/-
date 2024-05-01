using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool aim;
        public bool shoot;
        public bool reload;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            if (GameManager.instance.isLive)
            {
                MoveInput(value.Get<Vector2>());
            }
        }

        public void OnLook(InputValue value)
        {
            if (GameManager.instance.isLive && cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            if (GameManager.instance.isLive)
            {
                JumpInput(value.isPressed);
            }
        }

        public void OnSprint(InputValue value)
        {
            if (GameManager.instance.isLive)
            {
                SprintInput(value.isPressed);
            }
        }
        public void OnAim(InputValue value)
        {
            if (GameManager.instance.isLive)
            {
                AimInput(value.isPressed);
            }
        }
        public void OnShoot(InputValue value)
        {
            if (GameManager.instance.isLive)
            {
                ShootInput(value.isPressed);
            }
        }
        public void OnReload(InputValue value)
        {
            if (GameManager.instance.isLive)
            {
                ReloadInput(value.isPressed);
            }
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            if (GameManager.instance.isLive)
            {
                move = newMoveDirection;
            }
        }

        public void LookInput(Vector2 newLookDirection)
        {
            if (GameManager.instance.isLive)
            {
                look = newLookDirection;
            }
        }

        public void JumpInput(bool newJumpState)
        {
            if (GameManager.instance.isLive)
            {
                jump = newJumpState;
            }
        }

        public void SprintInput(bool newSprintState)
        {
            if (GameManager.instance.isLive)
            {
                sprint = newSprintState;
            }
        }
        public void AimInput(bool newAimState)
        {
            if (GameManager.instance.isLive)
            {
                aim = newAimState;
            }
        }
        public void ShootInput(bool newShootState)
        {
            if (GameManager.instance.isLive)
            {
                shoot = newShootState;
            }
        }
        public void ReloadInput(bool newReloadState)
        {
            if (GameManager.instance.isLive)
            {
                reload = newReloadState;
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}