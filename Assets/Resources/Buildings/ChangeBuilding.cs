using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBuilding : MonoBehaviour {

    public GameObject fullbuilding;
    public GameObject destroyedbuildinhg;
    public GameObject newbuilding;
    public GameObject score;
    private int count;

    private void Start()
    {
        count = 0;
        SetCountText();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("hand"))
        {
            count += 10;
            SetCountText();
            newbuilding = Instantiate(destroyedbuildinhg, fullbuilding.transform.position, Quaternion.Euler(fullbuilding.transform.rotation.x, -90, fullbuilding.transform.rotation.z), fullbuilding.transform.parent) as GameObject;
            Destroy(fullbuilding);
        }
    }

    void SetCountText()
    {
        score.GetComponent<Text>().text = "score: " + count.ToString();
    }
}
