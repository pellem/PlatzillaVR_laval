using UnityEngine;

public class SteeringBehaviors : MonoBehaviour
{

    public Vector3 Seek(Rigidbody body, Vector3 targetPosition)
    {
        return (targetPosition - body.transform.position).normalized;
    }

    public Vector3 Flee(Rigidbody body, Vector3 targetPosition)
    {
        return (body.transform.position - targetPosition).normalized;
    }

    public Vector3 Avoid(Rigidbody body, VisibleObjectData data)
    {
        Vector3 offset = body.position - data.position;
        Vector3 direction = Vector3.ProjectOnPlane(offset, body.transform.forward);

        return direction.normalized;
    }

}