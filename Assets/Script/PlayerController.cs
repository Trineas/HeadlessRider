using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    public float SideSpeed;
    public float DampingSpeed;
    public float JumpPower;
    [Range(0, 1)] public float Gravity;
    [Range(0.8f, 2f)] public float RecoverTime;

    private float ScrollSpeed;

    private Vector3 forwardDir;
    private Vector3 rightDir;
    private Vector3 movement;
    private Vector3 knockbackDir;

    private float distToGround;

    private float ForwardSpeed;
    private float VerticalSpeed;

    private bool isHit = false;
    private bool isVulnerable = true;
    private float vulTimer = 0f;

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "DeathTrigger")
        {
            gameObject.SetActive(false);
            // things to do for game over transition
        }

        if(isVulnerable && collision.gameObject.tag.Contains("Obstacle"))
        {
            GameplayManager.Instance.PlayerHit();
            isHit = true;
            isVulnerable = false;
            knockbackDir = collision.contacts[0].normal;
        }

        if(collision.gameObject.tag == "WinTrigger" && Input.GetKey(KeyCode.F))
        {
            print("You Win");
        }
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
        if (isHit)
        {
            ForwardSpeed = 0f;
            StartCoroutine(Blink());
            Physics.IgnoreLayerCollision(9, 8);
            transform.position += knockbackDir * 4f * Time.deltaTime;
            vulTimer += Time.deltaTime;

            // make player move before recovery ends
            if(vulTimer >= RecoverTime / 2f)
            {
                ForwardSpeed = SideSpeed;
            }

            if(vulTimer >= RecoverTime)
            {
                Physics.IgnoreLayerCollision(9, 8, false);
                isHit = false;
                isVulnerable = true;
                vulTimer = 0f;
            }

        }
        else
        {
            ForwardSpeed = SideSpeed;
        }
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
        else
        {

        }
    }
    

    void UpdateMovement()
    {
        transform.position += ForwardSpeed/4f * movement * Time.deltaTime;
        transform.position += VerticalSpeed * Vector3.up * Time.deltaTime;
    }

    IEnumerator Blink()
    {
        Renderer thisRend = this.GetComponent<Renderer>();
        thisRend.enabled = false;

        yield return new WaitForSeconds(0.1f);
        thisRend.enabled = true;
    }
}
