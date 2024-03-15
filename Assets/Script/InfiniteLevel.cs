using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteLevel : MonoBehaviour { 
   
    
   [SerializeField] GameObject Obstacle = new GameObject();
    [SerializeField] GameObject Stair = new GameObject();
    [SerializeField] Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LoadingZone")
        {
            float which = Random.Range(1, 2);
            Debug.Log(which);
        }
    }
}
