using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Test : MonoBehaviour
{

    public Animator animKnight;

    public float SideSpeed;
    public float DampingSpeed;
    public float JumpPower;
    [Range(0, 1)] public float Gravity;
    [Range(0.8f, 2f)] public float RecoverTime = 0.8f;

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

    private bool winGame = false;
    private bool hasDied = false;

    bool IsGrounded()
    {
        return Physics.Raycast(GetComponent<Collider>().transform.position, Vector3.down, distToGround + 0.01f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DeathTrigger")
        {
            gameObject.SetActive(false);
            hasDied = true;
        }

        if (isVulnerable && collision.gameObject.tag.Contains("Obstacle"))
        {
            animKnight.SetBool("isHit", true);
            isHit = true;
            isVulnerable = false;
            knockbackDir = collision.contacts[0].normal;
        }

        if (collision.gameObject.tag == "WinTrigger")
        {
            isVulnerable = false;
            winGame = true;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        animKnight = GameObject.Find("Body_Rigged").GetComponent<Animator>();

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

        animKnight.SetBool("Jump", Input.GetButton("Jump"));
        animKnight.SetBool("isGrounded", IsGrounded());
        transform.position += -Vector3.forward * ScrollSpeed * 0.8f;

        forwardDir = Vector3.forward * Input.GetAxis("Vertical");
        rightDir = Vector3.right * Input.GetAxis("Horizontal");

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
            //StartCoroutine(Blink());
            Physics.IgnoreLayerCollision(9, 8);
            transform.position += knockbackDir * 4f * Time.deltaTime;
            vulTimer += Time.deltaTime;

            // make player move before recovery ends
            if (vulTimer >= RecoverTime / 2f)
            {
                ForwardSpeed = SideSpeed;
            }

            if (vulTimer >= RecoverTime)
            {
                animKnight.SetBool("isHit", false);

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
            // make player always look forward
            Vector3 dest = Quaternion.LookRotation(Vector3.forward.normalized).eulerAngles;
            Quaternion rotation = Quaternion.Euler(dest.y * Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DampingSpeed);
        }
    }


    void UpdateMovement()
    {
        // Walking speed
        transform.position += Vector3.right * ForwardSpeed * movement.x * Time.deltaTime;

        // forward and backward movements are slowed
        transform.position += Vector3.forward * ForwardSpeed / 3f * movement.z * Time.deltaTime;

        // Jump Height
        transform.position += VerticalSpeed * Vector3.up * Time.deltaTime;
    }

    IEnumerator Blink()
    {
        Renderer bodyRend = GameObject.Find("Body").GetComponent<Renderer>();
        bodyRend.enabled = false;

        yield return new WaitForSeconds(0.1f);
        bodyRend.enabled = true;
    }

    public bool playerDied()
    {
        return hasDied;
    }

    public bool playerWon()
    {
        return winGame;
    }
}
