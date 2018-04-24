using UnityEngine;
using System.Collections;

public class GrabObjects : MonoBehaviour
{

    [Tooltip("From where the ray will come. Recommended: Main Camera")]
    public Transform center = null;
    [Tooltip("From how far you can reach objects.")]
    public float reach = 3f;
    [Tooltip("The rate in which objects move towards the grab position. Higher = faster")]
    public float rate = 0.45f;
    [Tooltip("The rate in which the charge for throw / push goes up.")]
    public float chargeRate = 2f;
    [Tooltip("Maximum force of a throw/push")]
    public float maxThrowForce = 5f;
    [Tooltip("The time you must wait to pick up an object after throwing another. (Push does not affect this)")]
    public float grabCooldown = 1f;
    [Tooltip("The rate in which the object repositions itself.")]
    public float repositionRate = 50f;
    [Tooltip("The rate in which you can rotate an object.")]
    public float rotateRate = 10f;
    [Tooltip("Minimum reposition distance.")]
    public float minimumDistance = 1f;
    [Tooltip("Maximum reposition distance.")]
    public float maximumDistance = 3f;
    [Tooltip("Makes you able to grab objects.")]
    public bool canGrab = true;
    [Tooltip("Makes you able to throw objects that are grabbed.")]
    public bool canThrow = true;
    [Tooltip("Makes you able to push objects that are not grabbed.")]
    public bool canPush = true;
    [Tooltip("Makes you able to reposition objects that are grabbed.")]
    public bool canReposition = true;
    [Tooltip("Layermask for the ray detecting objects. Note: The objects must also have the tag 'liftTag'")]
    public bool canRotate = true;
    public LayerMask layerMask;
    [Tooltip("The tag used by liftable objects.")]
    public string liftTag = "Liftable";


    private float wantedPosition = 3f;
    private float grabTimer = 0f;
    private float throwForce = 0f;
    private bool wantGrab = true;
    private bool wantThrow = true;
    private bool wantPush = false;
    private bool wantReposition = false;
    public bool wantRotate = false;
    private bool canGrabAgain = true;
    private GameObject grabbed = null;
    public bool isRotating { get; private set; }


    void Update()
    {
        UpdateInput();
        UpdateLogic();
    }

    private void UpdateInput()
    {
        // Check if the player is trying to grab / release an object
        if (Input.GetButton("Fire1") && canGrab)
        {
            wantGrab = true;
        }
        else
        {
            wantGrab = false;
        }

        // Check if the player is charging a throw/push, also see if the player wants to throw/push
        if (Input.GetButton("Fire2"))
        {
            if (throwForce < maxThrowForce)
                throwForce += (chargeRate * Time.deltaTime);
            if (throwForce > maxThrowForce)
                throwForce = maxThrowForce;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            if (canPush && grabbed == null)
                wantPush = true;
            if (canThrow && grabbed != null)
                wantThrow = true;
        }

        // Check if the player wants to reposition the object
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0 && canReposition)
        {
            wantReposition = true;
        }
        else
        {
            wantReposition = false;
        }

        // Check if the player wants to rotate the object
        if (Input.GetButton("Fire3") && grabbed != null && canRotate)
        {
            wantRotate = true;
        }
        else
        {
            wantRotate = false;
        }
    }

    private void UpdateLogic()
    {

        // Wants to grab / keep grabbing an object
        if (wantGrab && canGrabAgain)
        {
            if (grabbed == null)
            {
                Ray ray = new Ray(center.position, center.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, reach, layerMask))
                {
                    if (hit.collider.tag == liftTag)
                    {
                        grabbed = hit.collider.gameObject;
                    }
                }
            }
            else
            {
                if (grabbed.GetComponent<Rigidbody>())
                {
                    Rigidbody rb = grabbed.GetComponent<Rigidbody>();
                    rb.velocity = ((center.position + (center.forward * wantedPosition)) - grabbed.transform.position) * rate;
                }
                else
                {
                    grabbed = null;
                }
            }
        }
        else
        {
            if (grabbed != null)
            {
                grabbed = null;
            }
            // If you activate this, the wantedPosition will reset everytime
            // You let go of an object. You could also make another if statement to see if grabbed == null
            // and have the code in there instead.
            //wantedPosition = reach;
        }

        // Wants to reposition the grabbed object

        if (wantReposition)
        {
            if (grabbed != null)
            {
                wantedPosition += (Input.GetAxis("Mouse ScrollWheel") * repositionRate) * Time.deltaTime;
                wantedPosition = Mathf.Clamp(wantedPosition, minimumDistance, maximumDistance);
            }
            else
            {
                wantReposition = false;
            }
        }

        // Wants to rotate the grabbed object

        if (wantRotate)
        {
            if (grabbed != null)
            {
                float xa = Input.GetAxis("Mouse X") * 10;
                float ya = Input.GetAxis("Mouse Y") * 10;
                grabbed.transform.Rotate(new Vector3(ya, -xa, 0), Space.World);
                isRotating = true;
            }
            else
            {
                isRotating = false;
            }
        }
        else
        {
            isRotating = false;
        }

        // Want to throw the grabbed object

        if (wantThrow)
        {
            if (grabbed != null)
            {
                Rigidbody rb = grabbed.GetComponent<Rigidbody>();
                rb.velocity += center.rotation * new Vector3(0, 0, throwForce);
                throwForce = 0f;
                grabbed = null;
                canGrabAgain = false;
            }
            wantThrow = false;
            throwForce = 0f;
        }

        // Want to push an object

        if (wantPush)
        {
            if (grabbed == null)
            {
                Ray ray = new Ray(center.position, center.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, reach, layerMask))
                {
                    if (hit.collider.tag == liftTag)
                    {
                        Rigidbody rb = hit.collider.transform.GetComponent<Rigidbody>();
                        rb.velocity = center.rotation * new Vector3(0, 0, throwForce);
                        throwForce = 0f;
                    }
                }
            }

            wantPush = false;
            throwForce = 0f;
        }

        // Has thrown a grabbed object and needs to cooldown
        if (!canGrabAgain)
        {
            if (grabTimer < grabCooldown)
            {
                grabTimer += Time.deltaTime;
            }
            else
            {
                canGrabAgain = true;
            }
        }
        else
        {
            grabTimer = 0f;
        }

    }
}
