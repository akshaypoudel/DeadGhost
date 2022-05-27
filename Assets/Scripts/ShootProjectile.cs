using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float Range;
    [SerializeField] private CinemachineFreeLook mainCinemachineCam;
    [SerializeField] private CinemachineFreeLook aimCinemachineCam;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GhostMovement characterMovement;
    [SerializeField] private LayerMask layerMask = new LayerMask();
    [SerializeField] private AudioClip gunFireAudio;
    [SerializeField] private float impactForce;
    [SerializeField] private int damage;
    [SerializeField] private GameObject enemyHitVFX;



    private Vector3 mouseWorldPos;
    private static bool canShootProjectile = false;
    private bool canResetAimCamera = true;

    void Update()
    {
        CheckInputAndShoot();
        CheckInputAndAim();
    }

    private void CheckInputAndAim()
    {
        if (Input.GetMouseButton(1))
        {
            if (canResetAimCamera)
            {
                ResetAimingCameraDirection();
                canResetAimCamera = false;
            }
            Aim();
        }
        else
        {
            CancelAim();
        }
    }

    private void CheckInputAndShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canShootProjectile)
                Shoot();
        }
    }

    private void Shoot()
    {
        characterMovement.SetRotateOnMove(true);

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit,Range,layerMask))
        {
            mouseWorldPos = hit.point;
        }
        InstantiateProjectile();
    }
    private void InstantiateProjectile()
    {
        AudioSource.PlayClipAtPoint(gunFireAudio, transform.position);
        Vector3 aimDirection = (mouseWorldPos - firePoint.position).normalized;
        Instantiate(projectile, firePoint.position, Quaternion.LookRotation(aimDirection,Vector3.up));
   
    }
    private void Aim()
    {
        canShootProjectile = true;
        aimCinemachineCam.gameObject.SetActive(true);
        crosshair.SetActive(true);
        characterMovement.SetRotateOnMove(false);
        AimRotationHandler();
        RotatePlayerInTheAimDirection();
    }
    private void AimRotationHandler()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width/2f,Screen.height/2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Range, layerMask))
        {
            mouseWorldPos = hit.point;
        }
    }

    private void RotatePlayerInTheAimDirection()
    {
        Vector3 worldAimTarget = mouseWorldPos;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget  - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
    }

    private void CancelAim()
    {
        canShootProjectile = false;
        aimCinemachineCam.gameObject.SetActive(false);
        crosshair.SetActive(false);
        characterMovement.SetRotateOnMove(true);
        canResetAimCamera = true;
    }
    
    private void ResetAimingCameraDirection()
    {
        aimCinemachineCam.m_XAxis.Value = mainCinemachineCam.m_XAxis.Value;
        aimCinemachineCam.m_YAxis.Value = mainCinemachineCam.m_YAxis.Value;


    }
}
