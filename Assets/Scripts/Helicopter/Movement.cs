using UnityEngine;

public class Movement : MonoBehaviour {

    public void KeepConstantVelocity(Rigidbody body, int velocity)
    {
        if (body.velocity.magnitude > velocity)
        {
            body.velocity = body.velocity.normalized * velocity;
        }
    }

    public void Stop(Rigidbody body)
    {
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }

    public void ReduceSpeed(Rigidbody body)
    {
        Vector3 oppositeForce = -body.velocity;
        body.AddRelativeForce(oppositeForce.x, oppositeForce.y, oppositeForce.z);
    }

    public void MoveAround(Rigidbody body, Vector3 point, float speed)
    {
        body.transform.RotateAround(point, Vector3.up, speed * Time.deltaTime);
    }

}
