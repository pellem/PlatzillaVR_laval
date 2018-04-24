using UnityEngine;

public class MissileLauncher : MonoBehaviour {

    private const int RELOAD_TIME = 3;
    private const int BULLET_VELOCITY = 5;
    private const int ROTATION_VELOCITY = 5;

    public GameObject missilePrefab;
    public Transform leftMissileTransform;
    public Transform rightMissileTransform;

    private float time = 0f;

    public void FireAt(Vector3 target)
    {
        if (time > RELOAD_TIME)
        {
            LaunchMissiles(target);
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    private void LaunchMissiles(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        GameObject leftMissile = Instantiate(missilePrefab, leftMissileTransform.position, Quaternion.LookRotation(direction));
        GameObject rightMissile = Instantiate(missilePrefab, rightMissileTransform.position, Quaternion.LookRotation(direction));

        leftMissile.GetComponent<Rigidbody>().velocity = direction * BULLET_VELOCITY;
        rightMissile.GetComponent<Rigidbody>().velocity = direction * BULLET_VELOCITY;
    }

}
