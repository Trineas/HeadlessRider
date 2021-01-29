using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public float ScrollSpeed;

    private GameObject DarkWall;

    public static GameplayManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        ScrollSpeed = ScrollSpeed == 0 ? 0.3f : ScrollSpeed;
        DarkWall = GameObject.FindGameObjectWithTag("DeathTrigger");

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DarkWall.transform.position -= Vector3.forward * ScrollSpeed;
    }
}
