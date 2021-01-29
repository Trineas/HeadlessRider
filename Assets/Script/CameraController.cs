using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float scrollSpeed;
    private Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        scrollSpeed = GameObject.Find("GameplayManager").GetComponent<GameplayManager>().ScrollSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += -Vector3.forward * scrollSpeed;
    }
}
