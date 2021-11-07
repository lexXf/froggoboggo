using UnityEngine;
using System.Collections;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts:")]
    public GrappleRope grappleRope;

    [Header("Layer Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera")]
    public Camera m_camera;

    [Header("Transform Refrences:")]
    public GameObject gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 100)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = true;
    [SerializeField] private float maxDistance = 4;

    [Header("Launching")]
    public bool launchToPoint = true;
    [SerializeField] private LaunchType Launch_Type = LaunchType.Transform_Launch;
    [Range(0, 5)] [SerializeField] private float launchSpeed = 5;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoCongifureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequency = 3;
    [Header("Test")]
    public RaycastHit2D HitPoint;
    public Transform CurrentGrappledObject;


    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch,
    }

    [Header("Component Refrences:")]
    public SpringJoint2D m_springJoint2D;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 DistanceVector;
    Vector2 Mouse_FirePoint_DistanceVector;

    public Rigidbody2D ballRigidbody;

    //stuff for max distance + not hitting anything to latch onto
    public LineRenderer line;
    private Vector2 mousePos;
    private Vector2 offset;
    private Vector2 direction;
    private bool isLineDrawn = false;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        ballRigidbody.gravityScale = 1;
    }

    private void Update()
    {
        Mouse_FirePoint_DistanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;



        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grappleRope.GetComponent<GrappleRope>().isGrabbable = false;

            gunHolder.GetComponent<Rigidbody2D>().isKinematic = false;

            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            ballRigidbody.gravityScale = 1;

            isLineDrawn = false;
        }
        else
        {
            RotateGun(m_camera.ScreenToWorldPoint(Input.mousePosition), true);
        }

        line.enabled = isLineDrawn;

        if (isLineDrawn)
        {
            line.SetPosition(0, transform.position);
            //after a small amount of time, stop drawing!!!
            StartCoroutine(limitGrappleTime());
        }

    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            Quaternion startRotation = gunPivot.rotation;
            gunPivot.rotation = Quaternion.Lerp(startRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    private bool test = false;
    void SetGrapplePoint()
    {
        if (Physics2D.Raycast(firePoint.position, Mouse_FirePoint_DistanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, Mouse_FirePoint_DistanceVector.normalized);
            if ((_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll) && ((Vector2.Distance(_hit.point, firePoint.position) <= maxDistance) || !hasMaxDistance))
            {
                grapplePoint = _hit.point;


                DistanceVector = grapplePoint - (Vector2)gunPivot.position;
                grappleRope.enabled = true;

                HitPoint = _hit;

                CurrentGrappledObject = _hit.transform;
                //grapplePoint = new Vector2(CurrentGrappledObject.position.x, CurrentGrappledObject.position.y);


            }
            else if (_hit.transform.gameObject.tag == "Grappable" || grappleToAll)
            {



                grapplePoint = _hit.point;
                DistanceVector = grapplePoint - (Vector2)gunPivot.position;
                grappleRope.enabled = true;

                grappleRope.GetComponent<GrappleRope>().isGrabbable = true;

                HitPoint = _hit;


                // Move the two lines in Grab()

                //ADD FORCE TOWARDS THE PLAYER


            }
            else
            {
                nothingHit();
            }



            //If you didn't hit anything, or hit the wrong thing, then but still want to draw rope
            {
                //set grapplePoint in direction of mouse position
                //Do something with distanceVector
                //Set grappleRope enable to true

                //Set variable, to make sure you don't grapple
                //test = true;
            }
        }
        else
        {
            nothingHit();
        }
    }

    public void Grab()
    {
        HitPoint.transform.gameObject.GetComponent<Renderer>().material.color = Color.green;
        HitPoint.transform.gameObject.GetComponent<Rigidbody2D>().AddForce(GameObject.FindGameObjectWithTag("Player").transform.position - HitPoint.transform.position);


        //launchToPoint = false;

        Debug.Log("GRABBYGRAB");
        gunHolder.GetComponent<Rigidbody2D>().isKinematic = true;

        if (test)
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            ballRigidbody.gravityScale = 1;

            test = false;

            return;
        }
        if (!launchToPoint && !autoCongifureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequency;
        }

        if (!launchToPoint)
        {
            if (autoCongifureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }
            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }

        else
        {
            if (Launch_Type == LaunchType.Transform_Launch)
            {
                ballRigidbody.gravityScale = 0;
                ballRigidbody.velocity = Vector2.zero;
            }
            if (Launch_Type == LaunchType.Physics_Launch)
            {
                m_springJoint2D.connectedAnchor = grapplePoint;
                m_springJoint2D.distance = 0;
                m_springJoint2D.frequency = launchSpeed;
                m_springJoint2D.enabled = true;
            }
        }
    }

    public void Grapple()
    {
        gunHolder.GetComponent<Rigidbody2D>().isKinematic = false;

        Debug.Log("HOOKEDASFUCK");


        if (test)
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            ballRigidbody.gravityScale = 1;

            test = false;

            return;
        }
        if (!launchToPoint && !autoCongifureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequency;
        }

        if (!launchToPoint)
        {
            if (autoCongifureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }
            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }

        else
        {
            /*
            if (Launch_Type == LaunchType.Transform_Launch)
            {
                ballRigidbody.gravityScale = 0;
                ballRigidbody.velocity = Vector2.zero;
            }
            */
            if (Launch_Type == LaunchType.Physics_Launch)
            {
                m_springJoint2D.connectedAnchor = grapplePoint;
                m_springJoint2D.distance = 0;
                m_springJoint2D.frequency = launchSpeed;
                m_springJoint2D.enabled = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

    IEnumerator limitGrappleTime()
    {
        yield return new WaitForSeconds(.4f);
        isLineDrawn = false;
    }

    void nothingHit()
    {
        isLineDrawn = true;
        Debug.Log("notice me senpai");
        mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        offset = mousePos - (Vector2)transform.position;
        direction = offset.normalized;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + (Vector3)direction * maxDistance);
    }

}

