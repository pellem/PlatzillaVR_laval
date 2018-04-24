using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour {

    public Transform target;
    public NavMeshAgent agent;
    public Vector3 destination;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Vector3.Distance(destination, target.position) > 1.0f)
        {
            destination = target.position;
            agent.destination = destination;
        }
    }
}
