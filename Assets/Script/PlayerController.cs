using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float ScrollSpeed;
    public float MovementSpeed;

    private float mov_x;
    private float mov_y;

    // Start is called before the first frame update
    void Awake()
    {
        ScrollSpeed = ScrollSpeed == 0 ? 0.3f : ScrollSpeed;
        MovementSpeed = MovementSpeed == 0 ? 3f : MovementSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += -Vector3.forward * ScrollSpeed;
        UpdateMovement();
        
    }

    void UpdateMovement()
    {
        mov_x = Input.GetAxis("Horizontal");
        //mov_y = Input.GetAxis("Vertical");

        transform.position += Vector3.right * mov_x * MovementSpeed * Time.deltaTime;
        //transform.position += Vector3.forward * mov_y * Time.deltaTime;


    }
}
