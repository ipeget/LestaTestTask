using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private TMP_Text healthText;

    public void DisplayOnChange(int health, int maxHealth)
    {
        healthImage.fillAmount = health / (float)maxHealth;
        healthText.text = $"{health}/{maxHealth}";
    }

}
