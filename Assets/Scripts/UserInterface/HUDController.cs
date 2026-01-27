using TMPro;
using UnityEngine;
using FishNet.Object;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text scoreText;

    private PlayerStats playerStats;

    private void OnEnable()
    {
        PlayerStats.OnLocalPlayerSpawned += BindPlayer;
    }

    private void OnDisable()
    {
        PlayerStats.OnLocalPlayerSpawned -= BindPlayer;
    }

    private void BindPlayer(PlayerStats stats)
    {
        playerStats = stats;

        UpdateHP(playerStats.Health.Value);
        UpdateScore(ScoreManager.Instance.Score.Value);

        playerStats.Health.OnChange += OnHealthChanged;
        ScoreManager.Instance.Score.OnChange += OnScoreChanged;
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
