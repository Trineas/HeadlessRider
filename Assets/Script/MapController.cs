using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    private float spawnDelay;
    private float destroyDelay;

    private List<GameObject> mapAssets = new List<GameObject>();
    private float spawnTimer = 0f;
    private float destroyTimer = 0f;
    private Vector3 nextSpawn = Vector3.zero;

    // Spawn a map Asset
    void SpawnAsset()
    {
        int mapIdx = Random.Range(0, mapAssets.Count);
        GameObject temp = Instantiate(mapAssets[mapIdx], nextSpawn, Quaternion.identity);
        mapAssets.Add(temp);

        // Set next Spawn Position;
        Renderer mapBound = temp.GetComponent<Renderer>();
        nextSpawn -= Vector3.forward * (mapBound.bounds.extents.z * 2f);
    }

    // Start is called before the first frame update
    void Awake()
    {
        GameObject player = GameObject.Find("GameplayManager");
        GameplayManager p_Controller = player.GetComponent<GameplayManager>();

        spawnDelay = p_Controller.ScrollSpeed * 3f;
        destroyDelay = p_Controller.ScrollSpeed * 10f;

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
        destroyTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            SpawnAsset();
            spawnTimer = 0;
        }

        // Destory Tracks that are not needed
        if (destroyTimer >= destroyDelay)
        {
            Destroy(mapAssets[0]);
            mapAssets.RemoveAt(0);

            destroyTimer = 0f;
        }
    }

}
