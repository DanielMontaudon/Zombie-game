using UnityEngine;

public class ProjectileMove : MonoBehaviour
{

    public float speed;
    private Vector3 spawnPoint;
    private float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        if(Vector3.Distance(spawnPoint,transform.position) >= 100)
        {
            Destroy(this.gameObject);
        }
    }
}
