using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private float RayDistance;
    [SerializeField] private float XWallJump;
    [SerializeField] private float YWallJump;
    [SerializeField] public static bool TouchingWall;
    [SerializeField] private LayerMask WallLayerMask;
    [SerializeField] private LayerMask GroundLayerMask;
    [SerializeField] private Movement mv;

    private void CheckCollision()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right,RayDistance,WallLayerMask))
        {
            TouchingWall = true;
            if (Input.GetButtonDown("Jump") && !mv.Grounded && !mv.InJump)
            {
                rb.velocity = Vector2.zero;
                if (mv.XAxis != 0)
                {
                    rb.AddForce(new Vector2(XWallJump * mv.XAxis, YWallJump));
                }
            }
        }
        else if (Physics2D.Raycast(transform.position, Vector2.left, RayDistance, WallLayerMask))
        {
            TouchingWall = true;
            if (Input.GetButtonDown("Jump") && !mv.Grounded && !mv.InJump)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(XWallJump * mv.XAxis, YWallJump));
            }
        }
        else
        {
            TouchingWall = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
    }
}
