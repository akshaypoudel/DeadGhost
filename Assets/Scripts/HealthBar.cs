using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }
    
    public void SetMaxhealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
}
