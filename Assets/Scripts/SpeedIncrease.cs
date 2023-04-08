using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : MonoBehaviour
{
    [SerializeField] private int firstRequiredScore = 5;
    private int requiredScore;
    private float prevUpdateTime;
    private bool hasMaxSpeed;
    PlayerController player;
    private void Start()
    {
        Events.EatFoodEvent += IncreaseSpeed;
        Events.Respawn += ResetSpeed;
        player = GetComponent<PlayerController>();
        hasMaxSpeed = false;
        requiredScore = firstRequiredScore;
        prevUpdateTime = player.updateTime;
    }
    private void OnDestroy()
    {
        Events.EatFoodEvent -= IncreaseSpeed;
        Events.Respawn -= ResetSpeed;
    }
    public void IncreaseSpeed(Food.Disks diskType)
    {
        Score score = Interactors.GetInteractor<Score>("Score");
        
        if(score.GetScore() >= requiredScore && !hasMaxSpeed)
        {
            prevUpdateTime -= 0.05f;
           requiredScore += 10; 
        }
        player.updateTime = prevUpdateTime;
        if(diskType == Food.Disks.Speed)
        {
            DoubleSpeed();
        }
        else
        {
            
            if (prevUpdateTime <= 0.15f)
            {
                hasMaxSpeed = true;
            }
        }
        
        
    }

    public void DoubleSpeed()
    {

        prevUpdateTime = player.updateTime;
        player.updateTime /= 2;
        
    }
    public void ResetSpeed()
    {
        prevUpdateTime = player.updateTime;
        requiredScore = firstRequiredScore;
        hasMaxSpeed = false;
    }

}
