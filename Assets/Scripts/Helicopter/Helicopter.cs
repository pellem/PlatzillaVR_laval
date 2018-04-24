using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{

    private const int MIN_DISTANCE = 8;
    private const int MIN_DISTANCE_FROM_PLAYER = 10;
    private const int VELOCITY = 3;
    private const int ROTATION_VELOCITY = 3;
    private const int AVOIDANCE_VELOCITY = 5;
    private const int FLEE_VELOCITY = 1;

    private Rigidbody body;

    private MissileLauncher missileLauncher;
    private SteeringBehaviors steeringBehaviors;
    private Movement movement;
    private Sensor sensor;

    public Transform target;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;

        missileLauncher = GetComponentInChildren<MissileLauncher>();
        steeringBehaviors = GetComponent<SteeringBehaviors>();
        movement = GetComponent<Movement>();
        sensor = GetComponent<Sensor>();
    }

    void FixedUpdate()
    {
        LookAt(target.position);

        Vector3 direction = target.position - body.transform.position;
        if (direction.magnitude > MIN_DISTANCE_FROM_PLAYER)
        {
            SeekTarget(target.position);
        }
        else if (direction.magnitude < MIN_DISTANCE)
        {
            Flee(target.position);
        }
        else
        {
            Fire();
        }

    }

    private void SeekTarget(Vector3 targetPosition)
    {
        if (!IsAvoiding())
        {
            Vector3 steering = steeringBehaviors.Seek(body, targetPosition) * VELOCITY;
            movement.KeepConstantVelocity(body, VELOCITY);

            body.AddForce(steering);
        }
    }

    private void Flee(Vector3 targetPosition)
    {
        Vector3 steering = steeringBehaviors.Flee(body, targetPosition) * FLEE_VELOCITY;
        movement.KeepConstantVelocity(body, FLEE_VELOCITY);

        body.AddForce(steering);
    }

    private void Fire()
    {
        if (!IsAvoiding())
        {
            movement.Stop(body);
            movement.MoveAround(body, target.position, VELOCITY);
            missileLauncher.FireAt(target.position);
        }
    }

    private bool IsAvoiding()
    {
        VisibleObjectData visibleObject = sensor.FindClosestObject(body.transform.position);

        bool isAvoiding = (visibleObject != null);
        if (isAvoiding)
        {
            Avoid(visibleObject);
        }

        return isAvoiding;
    }

    private void Avoid(VisibleObjectData closestObject)
    {
        Vector3 steering = steeringBehaviors.Avoid(body, closestObject) * AVOIDANCE_VELOCITY;
        movement.ReduceSpeed(body);

        body.AddForce(steering);
    }

    private void LookAt(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - body.transform.position).normalized;
        direction.y = 0;

        // TODO: Fix model rotation
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation *= Quaternion.Euler(-90, 0, 0);

        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, rotation, ROTATION_VELOCITY * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Scrap") || other.gameObject.CompareTag("hand"))
        {
            Destroy(this);
        }
    }

}
