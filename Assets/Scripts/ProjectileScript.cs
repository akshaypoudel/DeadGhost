using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Rigidbody rb;
    public float projectileSpeed;
    public GameObject hitVfx;
    public static int hitCount;
    public AudioClip groundHitSfx;
    public int damage;
    public GameObject enemyHitVFX;
    public AudioClip enemyHitSfx;
    Enemy enemy;



    private void Start()
    {
        rb.velocity = transform.forward * projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Destroyable" && other.gameObject.tag != "Player")
        {
            GameObject g = Instantiate(hitVfx, transform.position,Quaternion.identity);
            AudioSource.PlayClipAtPoint(groundHitSfx, transform.position);
            Destroy(this.gameObject,2f);
            Destroy(g, 2f);


        }
        if (other.gameObject.tag == "Enemy")
        {
            AudioSource.PlayClipAtPoint(enemyHitSfx, transform.position);
            Instantiate(enemyHitVFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject,2f);
            enemy = other.transform.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }
}
