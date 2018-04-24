using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTanks : MonoBehaviour {

    public float fireRate = 5.0F;
    private float nextSpawn = 0.0F;
    public GameObject tank;
    public Transform spawn;
    public int dist = 0;
    public Transform player;

    // Use this for initialization
    void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update () {
	    Spawn();
    }

    void Spawn()
    {
        if (Time.time > nextSpawn)
        {
            spawn.position = new Vector3(Random.Range(transform.position.x - 10, transform.position.x + 10), 0.5f, Random.Range(transform.position.z - 10, transform.position.z + 10));
            float dist = Vector3.Distance(spawn.position, transform.position);
            Collider[] hitCollider = Physics.OverlapSphere(spawn.position, 0.3f);
            if (hitCollider.Length < 1 && dist > 5)
            {
                GameObject clone = Instantiate(tank, spawn.position, spawn.rotation) as GameObject;
                clone.GetComponent<NavAI>().target = player;
                clone.GetComponent<Rigidbody>().velocity = clone.transform.forward * 1;
                nextSpawn = Time.time + fireRate;
            }
            else
            {
                print("NON");   
            }
        }
    }
}
