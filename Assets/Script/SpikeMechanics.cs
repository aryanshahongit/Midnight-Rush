using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMechanics : MonoBehaviour
{
    Vector2 RespawnPoint;
    [SerializeField] redbox box;
    [SerializeField] Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        RespawnPoint = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        { 
            die();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ResetSpawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bad")
        {
            die();
        }
    }
    public void UpdateRespawnPoint()
    {
        RespawnPoint = transform.position;
    }
    public void die()
    {
        box.TimerOn = true;
        box.timer = 0;
        rb.velocity = Vector2.zero;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = RespawnPoint;
    }
    public void ResetSpawn()
    {
        RespawnPoint = Vector2.zero;

    }
}
