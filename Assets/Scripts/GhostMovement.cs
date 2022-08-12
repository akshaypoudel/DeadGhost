using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private CharacterController characterController;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    [SerializeField]private ParticleSystem playerHitVfx;
    [SerializeField]private Transform cam;
    [SerializeField]private float minClampX,maxClampX,minClampZ,maxClampZ;
    private bool canRotate = true;
    private PlayerHealth health;

    private void Start()
    {
        health = GetComponent<PlayerHealth>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        transform.Rotate(0f, 0f, 0f);

        CheckIfPressingShiftButton();
        MoveCharacter();
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
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (canRotate)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyProjectile")
        {
            playerHitVfx.Play();
            health.DamagePlayer(Enemy.damageToPlayer);
            Destroy(other.gameObject);
        }
    }
}
