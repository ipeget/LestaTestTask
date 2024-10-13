using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private HealthView healthView;
    [SerializeField] private PlayerIntecation playerIntecation;
    [SerializeField] private ViewResultPanel resultPanel;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        Time.timeScale = 1;
        playerHealth.HealthChanged.AddListener(healthView.DisplayOnChange);
        playerHealth.Died.AddListener(resultPanel.Loss);
        playerIntecation.GameEnded.AddListener(resultPanel.Victory);
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }
}
