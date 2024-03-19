using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using Application = UnityEngine.Application;

public class Pause : MonoBehaviour
{
    public InputField PlayerSpeed;
    public InputField MaxSpeed;
    public InputField Acceleration;
    public InputField DeAcceleration;
    public InputField IncreasedGravityScale;
    public InputField NormalGravityScale;
    public InputField GravityScaleAddition;
    public InputField JumpHoldTime;
    public InputField DefaultHoldTime;
    public InputField GravityScale;
    public InputField AngularDrag;
    public InputField JumpForce;
    public InputField[] fields;
    public float[] FetchedValues;
    public int currentindex;
    public int currentupdateindex;
    public Movement mv;
    public float[] newvalues;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] redbox box;
    [SerializeField] bool IsPaused;
    // Start is called before the first frame update
    void Start()
    {
        IsPaused = false;
        PauseMenu.SetActive(false);
       float[] newvalues = new float[12];
        currentindex = 0;
        currentupdateindex = 0;
        float[] FetchedValues = new float[12];
        
        InputField[] fields = new InputField[12];
        fields[0] = PlayerSpeed;
        fields[1] = MaxSpeed;
        fields[2] = Acceleration;
        fields[3] = DeAcceleration;
        fields[4] = IncreasedGravityScale;
        fields[5] = NormalGravityScale;
        fields[6] = GravityScaleAddition;
        fields[7] = JumpHoldTime;
        fields[8] = DefaultHoldTime;
        fields[9] = GravityScale;
        fields[10] = AngularDrag;
        fields[11] = JumpForce;
        FetchedValues = mv.FetchValues();
        foreach (InputField f in fields)
        {
            f.text = FetchedValues[currentindex].ToString();
            currentindex++;
        }
        currentindex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            pause();
       
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            changevalues();
            mv.updatevalue();
            unpause();

        }
    }

    private void unpause()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        IsPaused = false;
    }

    private void pause()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        IsPaused = true;

    }

    public void changevalues()
    {
        fields = new InputField[12];
        fields[0] = PlayerSpeed;
        fields[1] = MaxSpeed;
        fields[2] = Acceleration;
        fields[3] = DeAcceleration;
        fields[4] = IncreasedGravityScale;
        fields[5] = NormalGravityScale;
        fields[6] = GravityScaleAddition;
        fields[7] = JumpHoldTime;
        fields[8] = DefaultHoldTime;
        fields[9] = GravityScale;
        fields[10] = AngularDrag;
        fields[11] = JumpForce;
        currentindex = 0;
        currentupdateindex = 0;
     
    }
}
