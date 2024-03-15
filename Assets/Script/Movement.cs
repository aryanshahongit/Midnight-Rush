using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Movement : MonoBehaviour
{
    //              Pause
    public float[] FetchedValues;
    public float[] recivedvalues;
    public int currentupdateindex;
    public Pause p;
    //              Movement
    //      *Floats
    [SerializeField] private float XAxis;
    [SerializeField] private float PlayerSpeed;
    [SerializeField] private float MaxVelocity;
    [SerializeField] private float Acceleration;
    [SerializeField] private float DeAcceleration;
    //              Physics
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float IncreasedGravityScale;
    [SerializeField] private float NormalGravityScale;
    [SerializeField] private float ReducedGravityScale;
    //              WallJumps
    //      *Bools
    [SerializeField] private bool WallJumpRightCollosion;
    [SerializeField] private bool WallJumpLeftCollosion;

    //      *Floats
    [SerializeField] private float WallJumpForce;
    //              Jumps

    //      *Bools
    [SerializeField] private bool Grounded;
    [SerializeField] private bool JumpKey;
    [SerializeField] private bool HoldJumpKey;
    [SerializeField] private bool InJump;
    //      *Floats
    [SerializeField] private float RayDistance;
    [SerializeField] private float JumpForce;
    [SerializeField] private float JumpHoldTimer;
    [SerializeField] private float DefaultHoldTime;
    [SerializeField] private float JumpUpSmooth;
    [SerializeField] private float FirstJumpCheck;
    [SerializeField] private float GravityScale;
    [SerializeField] private float JumpVelocityMultiply;

    //      *LayerMasks
    [SerializeField] private LayerMask GroundLayerMask;
    [SerializeField] private LayerMask WallLayerMask;

    //              Functions

    //      *Defaults
    void Start()
    {
        currentupdateindex = 0;
        FirstJumpCheck = 0f;
        JumpHoldTimer = DefaultHoldTime;
        InJump = false;
    }
    private void Update()
    {
        MovementHandler(); // Handles Moving,Velocity Clamp,Gravity,Log Outputs
        GroundCheck();// Ground Detection Using Ray Cast
        JumpKeyCheck();
        Logs();
        /*WallJumpCollisions();*/
    }
    void FixedUpdate()
    {
        
        JumpCheck();
    }

    private void JumpCheck()
    {
        if (Grounded && JumpKey)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpVelocityMultiply );
            rb.AddForce(new Vector2(0, JumpForce * 10));
            InJump = true;
        }
        if (HoldJumpKey && JumpHoldTimer > 0 && InJump)
        {
            FirstJumpCheck++;
            if(FirstJumpCheck == 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpVelocityMultiply);
            }
            JumpHoldTimer -= Time.deltaTime;
            ReducedGravityScale = GravityScale;
            rb.AddForce(new Vector2(0, JumpForce));
            InJump = true;
        }
    }

    private void JumpKeyCheck()
    {
        if (Input.GetButtonDown("Jump"))
        {
            JumpKey = true;
        }
        if(Input.GetButtonUp("Jump"))
        {
            if(InJump == true) {
                rb.velocity = new Vector2(rb.velocity.x, JumpUpSmooth);
            }            
            JumpKey = false;
            HoldJumpKey = false;
        }
        if (Input.GetButton("Jump"))
        {
            HoldJumpKey = true;
        }

    }

    private void GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector3.down * RayDistance,Color.black);
        if (Physics2D.Raycast(transform.position, Vector3.down, RayDistance,GroundLayerMask))
        {
            Grounded = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            JumpHoldTimer = DefaultHoldTime;
            ReducedGravityScale = 0;
            InJump = false;
            FirstJumpCheck = 0;
        }
        else
        {
            Grounded = false;
        }
    }

    
    private void MovementHandler()
    {
        XAxis = Input.GetAxisRaw("Horizontal");
        if (rb.velocity.y < 0)
        {
            InJump = false;
            rb.gravityScale = IncreasedGravityScale;
        }
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = NormalGravityScale + ReducedGravityScale;
        }

        if (rb.velocity.x < MaxVelocity && rb.velocity.x > -MaxVelocity)
        {
            rb.velocity += new Vector2(XAxis * Time.deltaTime * Acceleration,0);
            rb.AddForce(new Vector2(XAxis, 0) * Time.deltaTime * PlayerSpeed);
        }
        if (rb.velocity.x > MaxVelocity)
        {
            rb.velocity = new Vector2(MaxVelocity, rb.velocity.y);
        }
        if (rb.velocity.x < -MaxVelocity)
        {
            rb.velocity = new Vector2(-MaxVelocity, rb.velocity.y);
        }
        if(Grounded && XAxis == 0)
        {
           if(rb.velocity.x > 0)
            {
                rb.velocity -= new Vector2(DeAcceleration * Time.deltaTime, 0);
            }
            if (rb.velocity.x < 0)
            {
                rb.velocity += new Vector2(DeAcceleration * Time.deltaTime, 0);
            }
        }
    }
    /*private void WallJumpCollisions()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, RayDistance,WallLayerMask) && Input.GetButton("Jump")){
            WallJumpRightCollosion = true;
            rb.AddForce(new Vector2(-1 * WallJumpForce, JumpForce * 5));
        }
        if (Physics2D.Raycast(transform.position, Vector2.left, RayDistance,WallLayerMask))
        {
            WallJumpLeftCollosion = true;
            rb.AddForce(new Vector2(1 * WallJumpForce, JumpForce * 5));
        }
    }*/

    private void Logs()
    {
        Debug.Log(rb.velocity);
    }


    public float[] FetchValues()
    {
        float[] FetchedValues = new float[12];
        FetchedValues[0] = PlayerSpeed;
        FetchedValues[1] = MaxVelocity;
        FetchedValues[2] = Acceleration;
        FetchedValues[3] = DeAcceleration;
        FetchedValues[4] = IncreasedGravityScale;
        FetchedValues[5] = NormalGravityScale;
        FetchedValues[6] = ReducedGravityScale;
        FetchedValues[7] = JumpHoldTimer;
        FetchedValues[8] = DefaultHoldTime;
        FetchedValues[9] = GravityScale;
        FetchedValues[10]= rb.angularDrag;
        FetchedValues[11] = JumpForce;
        return FetchedValues;
    }

    public void updatevalue()
    {
        currentupdateindex = 0;
        float[] recivedvalues = new float[12];
        foreach (InputField d in p.fields)
        {
            recivedvalues[currentupdateindex] = float.Parse(d.text);
            currentupdateindex++;
            
        }
        currentupdateindex = 0;
        PlayerSpeed = recivedvalues[0];
        MaxVelocity = recivedvalues[1];
        Acceleration = recivedvalues[2];
        DeAcceleration = recivedvalues[3];
        IncreasedGravityScale = recivedvalues[4];
        NormalGravityScale = recivedvalues[5];
        ReducedGravityScale = recivedvalues[6];
        JumpHoldTimer = recivedvalues[7];
        DefaultHoldTime = recivedvalues[8];
        GravityScale = recivedvalues[9];
        rb.angularDrag = recivedvalues[10];
        JumpForce = recivedvalues[11];
    }
}





