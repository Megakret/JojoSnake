using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Food : MonoBehaviour
{
    [SerializeField] private int xMin = 0;
    [SerializeField] private int xMax = 0;
    [SerializeField] private int yMin = 0;
    [SerializeField] private int yMax = 0;
    [SerializeField] private int GridMultiplier;
    [SerializeField] private int specialDiskChance;
    [Header("SuperGrowDisk")]
    [SerializeField] private Color superGrowColor;
    [Header("SuperSpeedDisk")]
    [SerializeField] private Sprite speedDiskSprite;

    public enum Disks
    {
        Default,
        SuperGrow,
        Speed

    }
    Disks currDisk;
    SpriteRenderer spr;
    Sprite defaultSprite;
    Color defaultColor;
    int growValue = 1;

    public void RandomizePos()
    {
        float x = Random.Range(xMin,xMax);
        float y = Random.Range(yMin, yMax);
        x = Mathf.Round(x) * GridMultiplier;
        y = Mathf.Round(y) * GridMultiplier;
        gameObject.transform.position = new Vector3(x,y,0);
        

    }
    private void RandomiseSpecial()
    {
        if (Random.Range(1,specialDiskChance) == 1)
        {
            int diskNum = Random.Range(1,3);
            if(diskNum == 1)
            {
                spr.color = superGrowColor;
                spr.sprite = defaultSprite;
                growValue = 5;
                currDisk = Disks.SuperGrow;
            }
            else
            {
                spr.sprite = speedDiskSprite;
                spr.color = defaultColor;
                currDisk = Disks.Speed;
            }


        }
        else
        {
            currDisk = Disks.Default;
            growValue = 1;
            spr.color = defaultColor;
            spr.sprite = defaultSprite;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomizePos();
        spr = GetComponent<SpriteRenderer>();
        defaultColor = spr.color;
        defaultSprite = spr.sprite;
        Events.Respawn += ResetDisk;
        

    }
    private void OnDestroy()
    {
        Events.Respawn -= ResetDisk;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Score score = Interactors.GetInteractor<Score>("Score");
        score.AddScore(growValue);
        RandomizePos();
        
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        Events.EatFoodEvent?.Invoke(currDisk);
        for (int i = 0; i < growValue; i++)
        {
            player.Grow();
        }
        
        RandomiseSpecial();

        
    }
    public void ResetDisk()
    {
        RandomizePos();
        RandomiseSpecial();
    }
}
