using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudController : MonoBehaviour
{
    public TextMeshProUGUI LivesText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI PowerUpsText;

    public void SetTimeText(int levelTime)
    {
        int minutes = (int)levelTime / 60;
        int seconds = (int)levelTime % 60;

        TimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void SetLivesText (int lives)
    {
        LivesText.text = "Lives: " + lives.ToString();
    }

    public void SetPowerUpsTxt (int count)
    {
        PowerUpsText.text = "Power Ups: " + count.ToString();
    }
}
