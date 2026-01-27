using UnityEngine;
using TMPro;
using System.Collections;

public class WaveUI : MonoBehaviour
{
    public static WaveUI Instance;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private float displayTime = 1.5f;

    private void Awake()
    {
        Instance = this;
        waveText.gameObject.SetActive(false);
    }

    public void ShowWave(int waveNumber)
    {
        StartCoroutine(ShowRoutine(waveNumber));
    }

    private IEnumerator ShowRoutine(int wave)
    {
        waveText.text = $"WAVE {wave}";
        waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        waveText.gameObject.SetActive(false);
    }
}

