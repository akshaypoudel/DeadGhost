using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public float speed;
    public CharacterController characterController;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;
    public float minClampX,maxClampX,minClampZ,maxClampZ;
    private bool canRotate = true;

    void Update()
    {
        MoveCharacter();
        CheckIfPressingShiftButton();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void CheckIfPressingShiftButton()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            MoveFast();
        }
        else
        {
            speed=5f;
        }
    }
    private void MoveFast()
    {
        speed =7.5f;
    }

    public void MoveCharacter()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            
            if(canRotate)
            {
                transform.rotation = Quaternion.Euler(0f,angle, 0f);
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized*speed*Time.deltaTime);

        }
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minClampX, maxClampX),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minClampZ, maxClampZ));
    }

    public void SetRotateOnMove(bool rotation)
    {
        canRotate = rotation;
    }
}
