using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Movement : MonoBehaviour
{
    //              Pause
    public float[] FetchedValues;
    public float[] ReceivedValues;
    public int CurrentUpdateIndex;
    public Pause p;
    //              Movement
    //      *Floats
    [SerializeField] public float XAxis;
    [SerializeField] private float PlayerSpeed;
    [SerializeField] private float MaxVelocity;
    [SerializeField] private float Acceleration;
    [SerializeField] private float DeAcceleration;
    [SerializeField] private float IncreasedDeAcceleration;
    [SerializeField] private float JumpCoolDown;
    //      *Bools
    [SerializeField] private bool CanMove;
    //              Physics
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float IncreasedGravityScale;
    [SerializeField] private float NormalGravityScale;
    [SerializeField] private float ReducedGravityScale;

    //      *Floats
    [SerializeField] private float WallJumpForce;
    //              Jumps

    //      *Bools
    [SerializeField] public bool Grounded;
    [SerializeField] private bool JumpKey;
    [SerializeField] private bool HoldJumpKey;
    [SerializeField] public bool InJump;
    [SerializeField] private bool StartTimer;
    [SerializeField] private bool BufferJump;
    [SerializeField] public bool BufferJumpException;
    //      *Floats
    [SerializeField] private float RayDistance;
    [SerializeField] private float JumpForce;
    [SerializeField] private float JumpHoldTimer;
    [SerializeField] private float DefaultHoldTime;
    [SerializeField] private float JumpUpSmooth;
    [SerializeField] private float FirstJumpCheck;
    [SerializeField] private float GravityScale;
    [SerializeField] private float JumpVelocityMultiply;
    [SerializeField] private float JumpBufferTime;
    [SerializeField] private float DefaultJumpBufferTime;
    [SerializeField] private float JumpsWhileFalling;

    //      *LayerMasks
    [SerializeField] private LayerMask GroundLayerMask;

    //          Classes
    [SerializeField] private WallJump wj;
    //              Functions

    //      *Defaults
    void Start()
    {
        JumpsWhileFalling = 0;
        CurrentUpdateIndex = 0;
        FirstJumpCheck = 0f;
        JumpHoldTimer = DefaultHoldTime;
        InJump = false;
    }
    private void Update()
    {
        if (JumpKey)
        {
            if (!Grounded)
            {
                StartTimer = true;
            }
        }
        if (StartTimer)
        {
            JumpBufferTime -= Time.deltaTime;
            if (Grounded && JumpBufferTime > 0 && JumpsWhileFalling <= 1 && XAxis != 0)
            {
                BufferJumpException = true;
                BufferJump = true;
            }
            else
            {
                BufferJump = false;
            }
        }
        if(BufferJump && Input.GetButtonDown("Jump"))
        {
            BufferJumpException = false;
            BufferJump = false;
        }
        MovementHandler(); // Handles Moving,Velocity Clamp,Gravity,Log Outputs
        GroundCheck();// Ground Detection Using Ray Cast
        JumpKeyCheck();
        Logs();
    }


    void FixedUpdate()
    {
        JumpCheck();
    }

    private void JumpCheck()
    {
        if (Grounded && JumpKey && !BufferJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpVelocityMultiply);
            rb.AddForce(new Vector2(0, JumpForce * 10));
            InJump = true;
        }
        if (HoldJumpKey && JumpHoldTimer > 0 && InJump && !BufferJump)
        {
            FirstJumpCheck++;
            if (FirstJumpCheck == 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpVelocityMultiply);
            }
            JumpHoldTimer -= Time.deltaTime;
            ReducedGravityScale = GravityScale;
            rb.AddForce(new Vector2(0, JumpForce));
            InJump = true;
        }
        if (Grounded && BufferJump)
        {
            BufferJumpException = false;
            BufferJump = false;
            Debug.LogError("Buffer Jump!");
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(1000 * XAxis, JumpForce * 10));
        }
    }


    private void JumpKeyCheck()
    {
        if (Input.GetButtonDown("Jump"))
        {
            JumpKey = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            if (InJump == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpUpSmooth);
            }
            BufferJumpException = false;
            BufferJump = false;
            JumpKey = false;
            HoldJumpKey = false;
        }
        if (Input.GetButton("Jump"))
        {
            HoldJumpKey = true;
            StartTimer = false;
            JumpBufferTime = DefaultJumpBufferTime;
        }

    }

    private void GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector3.down * RayDistance, Color.black);
        if (Physics2D.Raycast(transform.position, Vector3.down, RayDistance, GroundLayerMask))
        {
            Grounded = true;
            JumpsWhileFalling = 0;
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
            if (Input.GetButtonDown("Jump"))
            {
                JumpsWhileFalling++;
            }
            if (!WallJump.TouchingWall)
            {
                rb.gravityScale = IncreasedGravityScale;
            }
        }
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = NormalGravityScale + ReducedGravityScale;
        }

        if (rb.velocity.x <= MaxVelocity && rb.velocity.x >= -MaxVelocity && CanMove)
        {
            rb.velocity += new Vector2(XAxis * Time.deltaTime * Acceleration, 0);
            rb.AddForce(PlayerSpeed * Time.deltaTime * new Vector2(XAxis, 0));
        }
        if (rb.velocity.x > MaxVelocity && !BufferJumpException)
        {
            rb.velocity = new Vector2(MaxVelocity, rb.velocity.y);
        }
        if (rb.velocity.x < -MaxVelocity && !BufferJumpException)
        {
            rb.velocity = new Vector2(-MaxVelocity, rb.velocity.y);
        }
        if (Grounded && XAxis == 0)
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity -= new Vector2(DeAcceleration * Time.deltaTime, 0);
            }
            if (rb.velocity.x < 0)
            {
                rb.velocity += new Vector2(DeAcceleration * Time.deltaTime, 0);
            }
            if (rb.velocity.x > 0 && Mathf.Abs(rb.velocity.x) > MaxVelocity)
            {
                rb.velocity -= new Vector2(IncreasedDeAcceleration * Time.deltaTime, 0);
            }
            if (rb.velocity.x < 0 && Mathf.Abs(rb.velocity.x) > MaxVelocity)
            {
                rb.velocity += new Vector2(IncreasedDeAcceleration * Time.deltaTime, 0);
            }
        }
        if (!WallJump.TouchingWall)
        {
            CanMove = true;
        }
        else if (WallJump.TouchingWall && Grounded)
        {
            CanMove = true;
        }
        else
        {
            CanMove = false;
        }
    }

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
        FetchedValues[10] = rb.angularDrag;
        FetchedValues[11] = JumpForce;
        return FetchedValues;
    }

    public void UpdateValue()
    {
        CurrentUpdateIndex = 0;
        float[] ReceivedValues = new float[12];
        foreach (InputField d in p.fields)
        {
            ReceivedValues[CurrentUpdateIndex] = float.Parse(d.text);
            CurrentUpdateIndex++;

        }
        CurrentUpdateIndex = 0;
        PlayerSpeed = ReceivedValues[0];
        MaxVelocity = ReceivedValues[1];
        Acceleration = ReceivedValues[2];
        DeAcceleration = ReceivedValues[3];
        IncreasedGravityScale = ReceivedValues[4];
        NormalGravityScale = ReceivedValues[5];
        ReducedGravityScale = ReceivedValues[6];
        JumpHoldTimer = ReceivedValues[7];
        DefaultHoldTime = ReceivedValues[8];
        GravityScale = ReceivedValues[9];
        rb.angularDrag = ReceivedValues[10];
        JumpForce = ReceivedValues[11];
    }
}