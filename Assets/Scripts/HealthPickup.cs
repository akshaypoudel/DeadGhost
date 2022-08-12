using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthPickup : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject healthPickup;
    public float timeToWaitBeforeNextHealthPickup;
    public AudioClip consumeSFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && healthSlider.value != 1) 
        {
            AudioSource.PlayClipAtPoint(consumeSFX,transform.position);
            GiveFullHealthToPlayer();
        }
    }

    public void GiveFullHealthToPlayer()
    {
        healthSlider.value = 1;
        PlayerHealth.playerHealth = 100f;
        StartCoroutine(DisableHealthPickupForShortTime());
    }

    IEnumerator DisableHealthPickupForShortTime()
    {
        healthPickup.SetActive(false);
        yield return new WaitForSeconds(timeToWaitBeforeNextHealthPickup);
        healthPickup.SetActive(true);

    }
}
