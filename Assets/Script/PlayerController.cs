using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    public float SideSpeed;
    public float DampingSpeed;
    public float JumpPower;
    [Range(0, 1)] public float Gravity;

    private float ScrollSpeed;

    private Vector3 forwardDir;
    private Vector3 rightDir;
    private Vector3 movement;

    private float distToGround;

    private float ForwardSpeed;
    private float VerticalSpeed;

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Get scroll speed from manager
        ScrollSpeed = GameObject.Find("GameplayManager").GetComponent<GameplayManager>().ScrollSpeed;

        // set default value in cause of 0
        SideSpeed = SideSpeed == 0 ? 3f : SideSpeed;
        DampingSpeed = DampingSpeed == 0 ? 1 : DampingSpeed;
        Gravity = Gravity == 0 ? 0.5f : Gravity;
        JumpPower = JumpPower == 0 ? 9f : JumpPower;

        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        transform.position += -Vector3.forward * ScrollSpeed;

        forwardDir = Vector3.forward * Input.GetAxis("Vertical");
        rightDir  = Vector3.right * Input.GetAxis("Horizontal");

        movement = forwardDir + rightDir;

        Forward();
        Vertical();
        Rotation();

        UpdateMovement();
    }

    void Forward()
    {
        ForwardSpeed = SideSpeed;
    }

    protected void Vertical()
    {

        if (IsGrounded())
        {
            VerticalSpeed = 0f;
        }
        else
        {
            // player is falling
            VerticalSpeed -= Gravity;
        }

        if (IsGrounded() && Input.GetButton("Jump"))
        {
            VerticalSpeed = JumpPower;
        }

    }

    void Rotation()
    {

        // as long as direction is not neutral
        if (movement.x != 0 || movement.z != 0)
        {
            Vector3 dest = Quaternion.LookRotation(-movement.normalized).eulerAngles;
            Quaternion rotation = Quaternion.Euler(dest.y * Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DampingSpeed);
        }
    }

    

    void UpdateMovement()
    {
        transform.position += ForwardSpeed * movement * Time.deltaTime;
        transform.position += VerticalSpeed * Vector3.up * Time.deltaTime;
    }
}
