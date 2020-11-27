using UnityEngine;

public class EthanParentConstraint : MonoBehaviour
{
    public GameObject ethanLookAtObject;
    public bool IsPaused { get; set; }
    GameObject ethan;
    Transform ethanLookAtTransform;
    SkinnedMeshRenderer ethanMeshRenderer;

    public float cameraSensitivity;
    Vector3 cameraOffset;
    Vector3 cameraClampMin;
    Vector3 cameraClampMax;
    Vector3 hitOffsetValue;

    // Start is called before the first frame update
    void Start()
    {
        ethanLookAtTransform = ethanLookAtObject.transform;
        ethan = GameObject.Find("Ethan");
        ethanMeshRenderer = ethan.transform.Find("EthanBody").GetComponent<SkinnedMeshRenderer>();
        cameraOffset = new Vector3(ethanLookAtTransform.position.x, ethanLookAtTransform.position.y, -3);
        hitOffsetValue = new Vector3(0.2f, 0.2f, 0.2f);
        cameraClampMin.y = ethanMeshRenderer.bounds.min.y + hitOffsetValue.y;
        cameraClampMax.y = float.PositiveInfinity;
        cameraClampMin.x = float.NegativeInfinity;
        cameraClampMin.z = float.NegativeInfinity;
        cameraClampMax.x = float.PositiveInfinity;
        cameraClampMax.y = float.PositiveInfinity;
        cameraClampMax.z = float.PositiveInfinity;
        IsPaused = true;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPaused)
        {
            Vector3 vectorFromEthan = ethanLookAtTransform.position - transform.position;
            Quaternion verticalRotation = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * cameraSensitivity, Vector3.up);
            Quaternion horizontalRotation = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * cameraSensitivity, Vector3.Cross(vectorFromEthan.normalized, Vector3.up));
            cameraOffset = verticalRotation * horizontalRotation * cameraOffset;
            transform.position = ethanLookAtTransform.position + cameraOffset;
            vectorFromEthan = transform.position - ethanLookAtTransform.position;

            Debug.DrawRay(ethanLookAtTransform.position, vectorFromEthan, Color.red);
            //var hitArr = Physics.RaycastAll(transform.position - hitOffsetValue, vectorToEthan, vectorToEthan.magnitude - 1);
            if (Physics.Raycast(ethanLookAtTransform.position, vectorFromEthan, out RaycastHit raytoEthanHit, vectorFromEthan.magnitude, ~LayerMask.GetMask("Ethan")))
            {
                //cameraClampMin.y = float.NegativeInfinity;
                Debug.DrawRay(ethanLookAtTransform.position, raytoEthanHit.point - ethanLookAtTransform.position, Color.green);
                Debug.DrawRay(raytoEthanHit.point, raytoEthanHit.normal, Color.blue);
                Vector3 hitOffset = Vector3.Project(hitOffsetValue, raytoEthanHit.normal);
                hitOffset *= hitOffset.magnitude / Vector3.Dot(hitOffsetValue, raytoEthanHit.normal);
                transform.position = raytoEthanHit.point + hitOffset;
            }

            Vector3 currCameraPosXZ = new Vector3(transform.position.x, ethanLookAtTransform.position.y, transform.position.z);
            //print("Curr XZ positions: " + currCameraPosXZ + ", Prev XZ positions: " + prevCameraPosXZ + ", Move Direction: " + cameraMoveDir);
            Vector3 minDistFromEthan = ethanLookAtTransform.position + (currCameraPosXZ - ethanLookAtTransform.position).normalized;
            Debug.DrawRay(ethanLookAtTransform.position, (new Vector3(transform.position.x, ethanLookAtTransform.position.y, transform.position.z) - ethanLookAtTransform.position).normalized, Color.yellow);
            Debug.DrawLine(new Vector3(transform.position.x, ethanLookAtTransform.position.y - 0.1f, transform.position.z), ethanLookAtTransform.position - new Vector3(0.0f, 0.1f, 0.0f), Color.cyan);
            if (transform.position.x > minDistFromEthan.x && transform.position.x < ethanLookAtTransform.position.x)
            {
                cameraClampMin.x = float.NegativeInfinity;
                cameraClampMax.x = minDistFromEthan.x;
            }
            else if (transform.position.x < minDistFromEthan.x && transform.position.x > ethanLookAtTransform.position.x)
            {
                cameraClampMin.x = minDistFromEthan.x;
                cameraClampMax.x = float.PositiveInfinity;
            }
            else
            {
                cameraClampMin.x = float.NegativeInfinity;
                cameraClampMax.x = float.PositiveInfinity;
            }
            if (transform.position.z > minDistFromEthan.z && transform.position.z < ethanLookAtTransform.position.z)
            {
                cameraClampMin.z = float.NegativeInfinity;
                cameraClampMax.z = minDistFromEthan.z;
            }
            else if (transform.position.z < minDistFromEthan.z && transform.position.z > ethanLookAtTransform.position.z)
            {
                cameraClampMin.z = minDistFromEthan.z;
                cameraClampMax.z = float.PositiveInfinity;
            }
            else
            {
                cameraClampMin.z = float.NegativeInfinity;
                cameraClampMax.z = float.PositiveInfinity;
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, cameraClampMin.x, cameraClampMax.x),
                Mathf.Clamp(transform.position.y, cameraClampMin.y, cameraClampMax.y),
                Mathf.Clamp(transform.position.z, cameraClampMin.z, cameraClampMax.z));
            //print("Clamp positions: "+cameraClampMin+", "+cameraClampMax +", LookAt: " + ethanLookAtTransform.position + ", Transform: " +transform.position + ", MinDist: " + minDistFromEthan + ", Magnitude: " +(transform.position - ethanLookAtTransform.position).magnitude+"\n");
            transform.LookAt(ethanLookAtTransform);
        }
    }
}
