using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System;

public class redbox : MonoBehaviour
{
    [SerializeField] float countdownforpicture;
    [SerializeField] float defaultpic;
    [SerializeField] float countforexplode;
    [SerializeField] float explodeforce;
    public float timer;
    [SerializeField] TextMeshProUGUI MilliSecond;
    [SerializeField] TextMeshProUGUI Minute;
    [SerializeField] TextMeshProUGUI Second;
    [SerializeField] bool onbox;
    private bool timerOn;
    [SerializeField] Rigidbody2D rbplayer;
    [SerializeField] GameObject image;
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI text;

    public bool TimerOn { get => timerOn; set => timerOn = value; }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        { 
            timer += Time.deltaTime;
            Minute.text = TimeSpan.FromSeconds(timer).ToString("mm':'ss'.'ff");
        }
        

        /*MilliSecond.text = (timer).ToString().Substring((timer).ToString().Length - 4); 
        Second.text = ((int)timer).ToString();
        Minute.text = ((int)timer / 60).ToString();*/

        if (onbox)
        {
            countdownforpicture -= Time.deltaTime;
            if (countdownforpicture <= 0)
            {
                TimerOn = false;
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
            text.text = "Ready!?";
            countdownforpicture -= Time.deltaTime;
            if(countdownforpicture <= 0) {
                image.SetActive(true);
                TimerOn = false;
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

        if (collision.gameObject.CompareTag("Player"))
        {
            onbox = false;
            countdownforpicture = defaultpic;
            text.text = "HEY NO GET BACK!!!!!!!!";

        }
    }
}
