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
        Vector2 screenCenterPoint = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit,Range,layerMask))
        {
            InstantiateProjectile(hit.point);
        }
    }
    private void InstantiateProjectile(Vector3 Pos)
    {
        AudioSource.PlayClipAtPoint(gunFireAudio, transform.position);
        Vector3 aimDirection = (Pos - firePoint.position);
        Instantiate(projectile,firePoint.position, Quaternion.LookRotation(aimDirection,Vector3.up));
    }
    private void Aim()
    {
        characterMovement.SetRotateOnMove(false);
        canShootProjectile = true;
        aimCinemachineCam.gameObject.SetActive(true);
        crosshair.SetActive(true);
        AimRotationHandler();
    }
    private void AimRotationHandler()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        RaycastHit hit;
        Vector3 newDir= Vector3.zero;
        if (Physics.Raycast(ray, out hit, Range, layerMask))
        {
            newDir = hit.point;
        }
        RotatePlayerInTheAimDirection(newDir);
    }

    private void RotatePlayerInTheAimDirection(Vector3 mouseWorldPos1)
    {
        Vector3 worldAimTarget = mouseWorldPos1;
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
