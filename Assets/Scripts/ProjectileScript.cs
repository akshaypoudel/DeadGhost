using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Rigidbody rb;
    public float projectileSpeed;
    public ParticleSystem hitVfx;
    public static int hitCount;
    public AudioClip groundHitSfx;



    private void Start()
    {
        hitCount = 0;
        rb.velocity = transform.forward * projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.tag == "Destroyable" || other.gameObject.tag == "Enemy") && other.gameObject.tag != "Player")
        {
            Instantiate(hitVfx, transform.position,Quaternion.identity);
            AudioSource.PlayClipAtPoint(groundHitSfx, transform.position);
            Destroy(gameObject);
        }
    }
}
