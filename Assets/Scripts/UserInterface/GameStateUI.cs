using UnityEngine;

public class GameStateUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);

        GameManager.Instance.CurrentState.OnChange += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState oldState, GameState newState, bool asServer)
    {
        gameOverPanel.SetActive(newState == GameState.GameOver);
        victoryPanel.SetActive(newState == GameState.Victory);
    }
}
