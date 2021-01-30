using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script manages every Event happening in game including obstacle spawning and boundary control between dark wall and invisible wall
/// </summary>
public class GameplayManager : MonoBehaviour
{
    public float ScrollSpeed;
    public float ApproachSpeed = 8f;

    [Range(3,9)] public int delayMin;
    [Range(5,12)] public int delayMax;
    public List<GameObject> obstacles;
    [Tooltip("Time it takes until the Dark Wall falls back . (Easy, Medium, Hard)")]
    public List<float> diff_Parameter_DW = new List<float>(3) {10f, 20f, 40f};

    private float obstacleLTimer = 0f;
    private float obstacleJRTimer = 0f;

    private double obstacleLDelay = 3.0;
    private double obstacleJRDelay = 8.0;

    private List<GameObject> obstaclesL = new List<GameObject>();
    private List<GameObject> obstaclesJR = new List<GameObject>();

    private GameObject DarkWall;
    private GameObject InvisWall;

    private bool playerHit;

    private Vector3 DW_pos;

    private float dwDelay = 2f;
    private float dwTimer = 0f;

    public static GameplayManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        ScrollSpeed = ScrollSpeed == 0 ? 0.3f : ScrollSpeed;

        DarkWall = GameObject.FindGameObjectWithTag("DeathTrigger");
        DW_pos = DarkWall.transform.position;

        InvisWall = GameObject.Find("BoundaryWall");

        delayMax = delayMin > delayMax ? delayMin + 1 : delayMax;

        foreach(GameObject obst in obstacles)
        {
            if(obst.tag == "Lying Obstacle")
            {
                obstaclesL.Add(obst);
            }

            if(obst.tag == "Jumping Obstacle" || obst.tag == "Rolling Obstacle")
            {
                obstaclesJR.Add(obst);
            }
        }

        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveWalls();

        // spawn obstacles
        obstacleLTimer += Time.deltaTime;
        obstacleJRTimer += Time.deltaTime;

        if (obstacleLTimer >= obstacleLDelay)
        {
            SpawnObstacle("L");
            obstacleLTimer = 0f;
            obstacleLDelay = Random.Range(8, 15)/10f;
        }

        if (obstacleJRTimer >= obstacleJRDelay)
        {
            SpawnObstacle("JR");
            obstacleJRTimer = 0f;
            obstacleJRDelay = Random.Range(delayMin, delayMax);
        }


    }

    void MoveWalls()
    {
        Vector3 origPos = DarkWall.transform.position;
        // make wall move along scroll
        DarkWall.transform.position -= Vector3.forward * ScrollSpeed;

        DW_pos -= Vector3.forward * ScrollSpeed;

        // if player not hit, move wall slowly back
        if (!playerHit && DarkWall.transform.position.z <= DW_pos.z)
        {
            DarkWall.transform.position += Vector3.forward * 2f * Time.deltaTime;
        }

        // if player is hit, move the dark wall towards player
        if (playerHit) 
        {
            DarkWall.transform.position -= Vector3.forward * ApproachSpeed * Time.deltaTime;
            dwTimer += Time.deltaTime;
            if(dwTimer >= dwDelay)
            {
                playerHit = false;
                dwTimer = 0f;
            }
        }

        // invisible wall moves a bit faster (note: wall is already moves with camera)
        Vector3 stopPoint = GameObject.Find("FinishPoint").transform.position;
        if (InvisWall.transform.position.z >= stopPoint.z)
        {
            InvisWall.transform.position -= Vector3.forward * 0.3f * Time.deltaTime;
        }
    }

    void SpawnObstacle(string mode)
    {
        if(mode == "L")
        {
            Vector3 cameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
            int obstIdx = Random.Range(0, obstaclesL.Count);
            Instantiate(obstaclesL[obstIdx], cameraPos - Vector3.down * 3f, obstaclesL[obstIdx].transform.rotation);
        }

        if(mode == "JR")
        {
            Vector3 cameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
            int obstIdx = Random.Range(0, obstaclesJR.Count);
            Instantiate(obstaclesJR[obstIdx], cameraPos - Vector3.down * 3f, obstaclesJR[obstIdx].transform.rotation);
        }
        
    }

    // public method to be called by player when he gets hit
    public void PlayerHit()
    {
        playerHit = true;
    }
}
