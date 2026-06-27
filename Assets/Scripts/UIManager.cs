using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Health playerHealth;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthBar;
    }

    private void Awake()
    {
        if (playerHealth == null)
        {
            Debug.Log("playerHealth = null");
            return;
        }
    }

    private void UpdateHealthBar(float amount)
    {
        slider.value = amount;
    }
}
