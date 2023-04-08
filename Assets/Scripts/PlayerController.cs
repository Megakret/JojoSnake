using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    List<Transform> segments = new List<Transform>();
   
    public Transform segmentPrefab;
    public Vector2 input = Vector2.right;
    public int startSize = 4;
    public float Step;
    public float startUpdateTime;
    public float updateTime;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetState(); 
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A) && input != Vector2.right)
        {

            input = Vector2.left;
            
            
        }
        if (Input.GetKeyDown(KeyCode.D) && input != Vector2.left)
        {

            input = Vector2.right;
            

        }
        if (Input.GetKeyDown(KeyCode.W) && input != Vector2.down)
        {

            input = Vector2.up;
            

        }
        if (Input.GetKeyDown(KeyCode.S) && input != Vector2.up)
        {

            input = Vector2.down;
            

        }

    }
    
    public IEnumerator moveCoroutine()
    {
        while (Application.isPlaying)
        {
        float x = Mathf.Round(gameObject.transform.position.x) + input.x * Step;
        float y = Mathf.Round(gameObject.transform.position.y) + input.y * Step;
        StartCoroutine(SmoothMove(new Vector2(x,y), transform));
        for(int i = segments.Count - 1; i > 0; i--)
        {
            StartCoroutine(SmoothMove(segments[i-1].position, segments[i].transform));
        }
        
            if (input.x == 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg);
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {

                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject.transform.localScale = new Vector3(-input.x, 1, 1);
            }
        
        
        yield return new WaitForSeconds(updateTime);
        }
        
    }
    
    public IEnumerator SmoothMove(Vector2 NextPos, Transform ObjectToMove)
    {
        Vector2 currPos = ObjectToMove.position;
        float prevUpdateTime = updateTime;
        while((currPos - NextPos).magnitude > 0.5f)
        {
            
            yield return null;
            if(ObjectToMove == null || prevUpdateTime != updateTime)
            {
                break;
            }
            currPos = ObjectToMove.position;
            ObjectToMove.position = Vector2.MoveTowards(currPos, NextPos, (Step * Time.deltaTime)/ updateTime);
        }
        ObjectToMove.position = NextPos;
        yield break;
    }  
        
     
    public void ResetState()
    {
        Events.Respawn?.Invoke();
        updateTime = startUpdateTime;
        input = Vector2.right;
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        StopAllCoroutines();
        for(int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }
        segments.Clear();
        segments.Add(gameObject.transform);
        for(int i = 0; i < startSize; i++)
        {
            Grow();
        }
        StartCoroutine(moveCoroutine());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Snake"))
        {
            ResetState();
            Score score = Interactors.GetInteractor<Score>("Score");
            score.ResetScore();
        }
        
        
    }
    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab, new Vector3(100,100,0),new Quaternion(0,0,0,0));
        segment.position = segments[segments.Count - 1].position;
        segment.rotation = segments[segments.Count - 1].rotation;
        segments.Add(segment);

    }
}
