using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoringController : Singleton<ScoringController>
{
    // Scoring label.
    [SerializeField]
    private TMP_Text scoreLabel;

    private float score;

    public void AddScore(float newScore)
    {
        score += newScore;

        if (scoreLabel != null)
        {
            scoreLabel.text = Mathf.RoundToInt(score).ToString(CultureInfo.InvariantCulture);
        }
    }
}

