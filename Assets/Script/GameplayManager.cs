using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public float ScrollSpeed;
    [Range(3,9)] public int delayMin;
    [Range(5,12)] public int delayMax;
    public List<GameObject> obstacles;

    private float obstacleLTimer = 0f;
    private float obstacleJRTimer = 0f;

    private float obstacleLDelay = 3f;
    private float obstacleJRDelay = 8f;

    private List<GameObject> obstaclesL = new List<GameObject>();
    private List<GameObject> obstaclesJR = new List<GameObject>();

    private GameObject DarkWall;

    // Start is called before the first frame update
    void Awake()
    {
        ScrollSpeed = ScrollSpeed == 0 ? 0.3f : ScrollSpeed;
        DarkWall = GameObject.FindGameObjectWithTag("DeathTrigger");

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
            obstacleLDelay = 1.5f;
        }

        if (obstacleJRTimer >= obstacleJRDelay)
        {
            SpawnObstacle("JR");
            obstacleJRTimer = 0f;
            obstacleJRDelay = 2f;
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
