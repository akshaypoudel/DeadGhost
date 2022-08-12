using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static float playerHealth = 100;
    private bool isGameOver;
    public Slider healthSlider;
    [SerializeField]private GameObject gameOverScreen;
    [SerializeField] private HealthPickup pickup;
    public Vector3 resetPosition;
    public AudioClip playerHit;
    public static int damageToEnemy = 10;

    private bool isInvincible;
    public ParticleSystem invincibleVFX;
    public Slider invincibleTimerSlider;
    [SerializeField] private float invincibilityTime;
    private bool stopInvinciblityTimer;
    private float time;


    void Start()
    {
        isGameOver = false;
        isInvincible = false;
        healthSlider.value = 1;
        stopInvinciblityTimer = false;
        invincibleTimerSlider.maxValue = invincibilityTime;
        invincibleTimerSlider.value = invincibilityTime;
        time = invincibilityTime;
    }

    // Update is called once per frame

    void Update()
    {
        if(isGameOver && !isInvincible)
        {
            GameOver();
        }
        if(isInvincible && !stopInvinciblityTimer)
        {
            InvinciblityTimer();
        }
    }


    public void DamagePlayer(int damage)
    {
        AudioSource.PlayClipAtPoint(playerHit,transform.position);
        if(!isInvincible)
        {
            playerHealth -= damage;
            float value = playerHealth / 100;
            healthSlider.value = value;
            if(playerHealth <= 0)
            {
                isGameOver=true;
            }
        }
    }

    private void GameOver()
    {
        isGameOver = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
    }
    public void Respawn()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameOverScreen.SetActive(false);
        this.gameObject.transform.position = resetPosition;
        pickup.GiveFullHealthToPlayer();
        StartCoroutine(Invincibility());
    }

    public IEnumerator Invincibility()
    {

        isInvincible = true;
        invincibleTimerSlider.gameObject.SetActive(true);
        PlayerHealth.damageToEnemy = 100;
        invincibleVFX.Play();

        yield return new WaitForSeconds(invincibilityTime);

        isInvincible = false;
        invincibleVFX.Stop();
        PlayerHealth.damageToEnemy = 10;

        yield return new WaitForSeconds(3f);

        invincibleTimerSlider.gameObject.SetActive(false);
        invincibleTimerSlider.value = invincibilityTime;
        time = invincibilityTime;
        stopInvinciblityTimer = false;


    }
    private void InvinciblityTimer()
    {
        if(time>=0)
        {
            time -= Time.deltaTime;
            invincibleTimerSlider.value = time;
        }
        else 
        {
            stopInvinciblityTimer = true;
        }
    }

}
