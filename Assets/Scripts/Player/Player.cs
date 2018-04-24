using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private HealthBarManager healthBarManager;

	void Start () {
        healthBarManager = GetComponent<HealthBarManager>();
    }

	void Update () {
        // TODO: Do something when health bar is empty
        if (healthBarManager.IsHealthBarEmpty() == true)
        {
            Debug.Log("Player");
        }
    }
}
