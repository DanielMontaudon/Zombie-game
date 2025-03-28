using UnityEngine;

public class HordeSpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public float hordeTimer = 10f;

    float currentTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= hordeTimer)
        {
            currentTime = 0;
            SpawnHorde();
        }
        
    }

    private void SpawnHorde()
    {
        //plot area to spawn first,
        //shoot raycast to player
        //if hit, do not spawn enemy
        //else (something blocking view), spawn and set target to player
        print("Spawned horde");
    }
}
