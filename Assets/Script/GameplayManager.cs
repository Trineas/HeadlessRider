using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public float ScrollSpeed;
    [Range(3,9)] public int delayMin;
    [Range(5,12)] public int delayMax;
    public List<GameObject> obstacles;
    public List<float> diff_Parameter_DW;

    private float obstacleLTimer = 0f;
    private float obstacleJRTimer = 0f;

    private double obstacleLDelay = 3.0;
    private double obstacleJRDelay = 8.0;

    private List<GameObject> obstaclesL = new List<GameObject>();
    private List<GameObject> obstaclesJR = new List<GameObject>();

    private GameObject DarkWall;
    private GameObject InvisWall;

   

    public static GameplayManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        ScrollSpeed = ScrollSpeed == 0 ? 0.3f : ScrollSpeed;
        DarkWall = GameObject.FindGameObjectWithTag("DeathTrigger");
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
        DarkWall.transform.position -= Vector3.forward * ScrollSpeed;

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
}
