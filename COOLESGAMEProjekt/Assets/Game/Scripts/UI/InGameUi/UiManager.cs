using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Generell.LevelManager1;
using UI;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public LevelManager LevelManager;

    public TextMeshProUGUI CoinsText;

    public TextMeshProUGUI LevelTimerText;

    private void Start()
    {
        LevelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        CoinDisplay();
        TimerDisplay();
    }

    private void CoinDisplay()
    {
        CoinsText.text = LevelManager.Coins.ToString();
    }

    private void TimerDisplay()
    {
        int timer = LevelManager.Timer;
        int Minuten = (int)timer / 60;
        int Sekunden = timer - (Minuten * 60);
        string text = Minuten + ":" + Sekunden;
        LevelTimerText.text = text;
    }
}
