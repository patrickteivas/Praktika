using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour
{
    Animator anim;
    GameObject player;

    float moveSpeed = 8;
    float jumpHeight = 3;
    float timeToJumpApex = .3f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    Vector2 wallJumpClimb;
    Vector2 wallJumpOff;
    Vector2 wallLeap;
    public float wallSlideSpeedMax = 3;
    float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float jumpVelocity;
    float velocityXSmoothing;

    Vector3 velocity;
    Controller2D controller;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");

        wallJumpClimb.x = 7.5f;
        wallJumpClimb.y = 16;

        wallJumpOff.x = 8.5f;
        wallJumpOff.y = 7;

        wallLeap.x = 18;
        wallLeap.y = 17;

        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);

        anim = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y <= -2)
        {
            player.transform.position = new Vector2(-6, 3);
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
                velocity.y = -wallSlideSpeedMax;

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                    timeToWallUnstick -= Time.deltaTime;
                else
                    timeToWallUnstick = wallStickTime;
            }
            else
                timeToWallUnstick = wallStickTime;
        }

        if (controller.collisions.above || controller.collisions.below)
            velocity.y = 0;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
                velocity.y = jumpVelocity;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) // Run
        {
            anim.SetInteger("State", 1);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) // Jump
        {
            anim.SetInteger("State", 2);
        }
        //if (Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    anim.SetInteger("State", 0);
        //}

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}