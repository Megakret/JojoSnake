using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Score : MonoBehaviour
{
    private int score;
    public TMP_Text text;
    public void AddScore(int amount)
    {
        score += amount;
        UpdateText();
    }
    public int GetScore()
    {
        return score;
    }
    public void ResetScore()
    {
        score = 0;
        UpdateText();
    }
    private void UpdateText()
    {
        text.text = $"Score: {score}";
    }
    private void Start()
    {
        Interactors.AddInteractor("Score", this);
    }

}
