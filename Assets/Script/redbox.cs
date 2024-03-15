using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class redbox : MonoBehaviour
{
    [SerializeField] float countdownforpicture;
    [SerializeField] float defaultpic;
    [SerializeField] float countforexplode;
    [SerializeField] float explodeforce;
    [SerializeField] bool onbox;
    [SerializeField] Rigidbody2D rbplayer;
    [SerializeField] GameObject image;
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onbox)
        {
            countdownforpicture -= Time.deltaTime;
            if (countdownforpicture <= 0)
            {
                image.SetActive(true);
            }
            if (image.activeSelf == true)
            {
                countforexplode -= Time.deltaTime;
            }
            if (countforexplode <= 0)
            {
                Vector2 dir = player.transform.position - transform.position;
                rbplayer.AddForce(dir * explodeforce);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
            onbox = true;
            text.text = "redy!?";
            countdownforpicture -= Time.deltaTime;
            if(countdownforpicture <= 0) {
                image.SetActive(true);
            }
            if (image.activeSelf == true)
            {
                countforexplode -= Time.deltaTime;
            }
            if(countforexplode <= 0)
            { 
                rbplayer.AddForce(new Vector2(1,1) * explodeforce);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            onbox = false;
            countdownforpicture = defaultpic;
            text.text = "HEY NO GET BACK!!!!!!!!";

        }
    }
}
