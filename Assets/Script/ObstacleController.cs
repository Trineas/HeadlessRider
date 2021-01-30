using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float BouncePower = 10f;
    public float FallSpeed = 5f;

    private float ScrollSpeed;
    private float JumpPower = 3f;
    private enum obstType { Jumping, Rolling, Lying };
    private Renderer currentTrack;

    [SerializeField] obstType ObstacleType = obstType.Jumping;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "DeathTrigger")
        {
            Rigidbody thisRB = GetComponent<Rigidbody>();
            thisRB.isKinematic = true;

            StartCoroutine(DestoryYourself());
        }

        if(ObstacleType == obstType.Jumping && collision.gameObject.tag == "RunTrack")
        {
            JumpPower = BouncePower;
        }

        if (ObstacleType == obstType.Rolling && collision.gameObject.tag == "RunTrack")
        {
            JumpPower = BouncePower/2f;
            ScrollSpeed += 8f;
        }

    }

    // Start is called before the first frame update
    void Awake()
    {
        ScrollSpeed = GameObject.Find("GameplayManager").GetComponent<GameplayManager>().ScrollSpeed;

        //decide Spawn point
        transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

        if(ObstacleType == obstType.Rolling)
        {
            transform.position -= Vector3.up * 3f;
            ScrollSpeed *= 20f;
        }

        RaycastHit trackLoc;
        if (Physics.Raycast(transform.position, Vector3.down, out trackLoc, transform.position.y + 20f))
        {
            currentTrack = trackLoc.transform.GetComponent<Renderer>();
        }

        float offsetSide = currentTrack.bounds.extents.x - 3f;
        float VertPos = Random.Range(-offsetSide, offsetSide);
        transform.position += Vector3.right * VertPos;

        if(ObstacleType == obstType.Lying)
        {
            Renderer thisObj = GetComponent<Renderer>();
            transform.position += Vector3.up * (trackLoc.transform.position.y - transform.position.y + thisObj.bounds.extents.y);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.forward * ScrollSpeed * Time.deltaTime;
        if(ObstacleType == obstType.Jumping || ObstacleType == obstType.Rolling)
        {
            transform.position += Vector3.up * JumpPower * Time.deltaTime;
            JumpPower -= Time.deltaTime * FallSpeed;
        }
    }

    IEnumerator DestoryYourself()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
