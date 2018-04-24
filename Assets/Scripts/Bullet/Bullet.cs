using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const string playerTag = "Player";

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == playerTag)
        {
            HealthBarManager playerHealthBar = collider.gameObject.GetComponent<HealthBarManager>();
            playerHealthBar.ReduceHealth(0.5f);

            // Destroy bullet
            Destroy(this.gameObject);
        } else if (collider.gameObject.tag == "helicopter")
        {
            //do nothing
        } else
        {
            Destroy(this.gameObject);
        }
    }
}
