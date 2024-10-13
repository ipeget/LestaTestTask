using TMPro;
using UnityEngine;

public class ViewResultPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text gameResultText;
    [SerializeField] private TMP_Text timerText;

    public void Loss()
    {
        timerText.gameObject.SetActive(false);

        gameResultText.text = "Поражение!";
        
        AdditionalSet();
    }

    public void Victory(float elapsedTime)
    {
        timerText.gameObject.SetActive(true);

        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        string timeText = minutes == 0 ? $"{seconds}c" : $"{minutes}м {seconds}c";

        gameResultText.text = "Победа!";
        timerText.text = $"Время прохождения: " + timeText;

        AdditionalSet();
    }

    private void AdditionalSet()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        panel.SetActive(true);
    }
}
