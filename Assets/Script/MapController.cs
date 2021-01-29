using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    private float spawnDelay;
    private List<GameObject> mapAssets = new List<GameObject>();
    private float spawnTimer = 0f;
    private Vector3 nextSpawn = Vector3.zero;

    void SpawnAsset()
    {
        int mapIdx = Random.Range(0, mapAssets.Count);
        GameObject temp = Instantiate(mapAssets[mapIdx], nextSpawn, Quaternion.identity);

        // Set next Spawn Position;
        Renderer mapBound = temp.GetComponent<Renderer>();
        nextSpawn -= Vector3.forward * (mapBound.bounds.extents.z * 2f);
    }

    // Start is called before the first frame update
    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController p_Controller = player.GetComponent<PlayerController>();

        spawnDelay = p_Controller.ScrollSpeed * 3f;

        foreach (Transform child in transform)
        {
            if(nextSpawn.z > child.position.z)
            {
                nextSpawn = child.position;

                Renderer mapBound = child.GetComponent<Renderer>();
                nextSpawn -= Vector3.forward * (mapBound.bounds.extents.z *2f);
            }
            mapAssets.Add(child.gameObject);
        }
        
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            SpawnAsset();
            spawnTimer = 0;
        }
    }
}
