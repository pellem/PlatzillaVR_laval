using System;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private const int RADIUS = 1;
    private const int VIEW_ANGLE = 120;

    public VisibleObjectData FindClosestObject(Vector3 currentPosition)
    {
        VisibleObjectData closestObject = null;
        float closestDistance = float.PositiveInfinity;
        List<VisibleObjectData> visibleObjects = FindVisibleObjects();

        foreach (VisibleObjectData data in visibleObjects)
        {
            float distance = Vector3.Distance(currentPosition, data.position);
            if (closestDistance >= distance)
            {
                closestObject = data;
            }
        }

        return closestObject;
    }

    private List<VisibleObjectData> FindVisibleObjects()
    {
        List<VisibleObjectData> visibleObjects = new List<VisibleObjectData>();
        Vector3 currentPosition = this.transform.position;
        Collider[] colliders = Physics.OverlapSphere(currentPosition, RADIUS);

        foreach (Collider collider in colliders)
        {
            Transform objectFound = collider.transform;
            Vector3 direction = (objectFound.position - currentPosition).normalized;

            float distance = Vector3.Distance(currentPosition, objectFound.position);

            RaycastHit hit;
            if (IsInFieldOfView(direction) && Physics.Raycast(currentPosition, direction, out hit, distance))
            {
                // TODO - Fix detection. For now, do not consider other helicopters or objects which are under specified height
                GameObject hittedObject = hit.transform.gameObject;
                if (hittedObject.tag != "helicopter")
                {
                    VisibleObjectData data = createDataFrom(hit);
                    visibleObjects.Add(data);
                }
            }
        }

        return visibleObjects;
    }

    private bool IsInFieldOfView(Vector3 direction)
    {
        //return (Vector3.Angle(this.transform.forward, direction) <= (VIEW_ANGLE / 2));

        // TODO: Fix model rotation
        return (Vector3.Angle(-this.transform.up, direction) <= (VIEW_ANGLE / 2));
    }

    private VisibleObjectData createDataFrom(RaycastHit raycast)
    {
        Vector3 position = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        try
        {
            position = raycast.transform.gameObject.transform.position;
            velocity = raycast.transform.gameObject.GetComponent<Rigidbody>().velocity;
        }
        catch (Exception e)
        {
            // Exception when the object doesn't have a rigid body 
            velocity = Vector3.zero;
        }

        return new VisibleObjectData(position, velocity);
    }

    // For debugging purposes
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, RADIUS);
    }
}
