using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text scoreText;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        UpdateHP(100);
        UpdateScore(0);

        ScoreManager.Instance.Score.OnChange += OnScoreChanged;
        playerStats.Health.OnChange += OnHealthChanged;
    }

    private void OnHealthChanged(int oldValue, int newValue, bool asServer)
    {
        UpdateHP(newValue);
    }

    private void OnScoreChanged(int oldValue, int newValue, bool asServer)
    {
        UpdateScore(newValue);
    }

    private void UpdateHP(int hp)
    {
        hpText.text = $"HP: {hp}";
    }

    private void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}

