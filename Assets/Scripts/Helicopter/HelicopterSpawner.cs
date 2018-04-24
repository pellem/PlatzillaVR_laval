using UnityEngine;

public class HelicopterSpawner : MonoBehaviour
{

    private const float RADIUS = 10;
    private const float HEIGHT = 3f;
    private const float DISTANCE = 50;
    private const float SPAWN_RATE = 2f;

    private float nextSpawn = 0.0f;

    public GameObject helicopter;

    public Transform playerTransform;

    void Update()
    {
        if (nextSpawn > SPAWN_RATE)
        {
            SpawnHelicopter();
            nextSpawn = 0;
        }
        else
        {
            nextSpawn += Time.deltaTime;
        }
    }

    private void SpawnHelicopter()
    {
        float x = Random.Range(playerTransform.position.x - DISTANCE, playerTransform.position.x + DISTANCE);
        float z = Random.Range(playerTransform.position.z - DISTANCE, playerTransform.position.z + DISTANCE);
        Vector3 randomizedPosition = new Vector3(x, HEIGHT, z);

        Collider[] hitCollider = Physics.OverlapSphere(randomizedPosition, RADIUS);
        float distanceFromPlayer = Vector3.Distance(randomizedPosition, playerTransform.position);

        if (hitCollider.Length.Equals(0) && distanceFromPlayer >= DISTANCE)
        {
            GameObject createdHelicopter = Instantiate(helicopter, randomizedPosition, Quaternion.identity);
            createdHelicopter.GetComponent<Helicopter>().target = playerTransform;
        }
    }

}
