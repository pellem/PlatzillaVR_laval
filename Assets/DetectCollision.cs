using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour {

    Rigidbody _rb;
    // Use this for initialization
    void Start () {
        _rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "hand")
        {
            _rb.isKinematic = false;
        }
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            _rb.isKinematic = true;
        }
    }*/
}
