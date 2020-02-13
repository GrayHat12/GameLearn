using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

    private Vector3 velocity = Vector3.zero;
    private Rigidbody rb;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationx = 0f;
    [SerializeField]
    private Camera cam;
    private Vector3 thrusterForce = Vector3.zero;
    [SerializeField]
    private float cameraRotationLimit = 85f;
    private float currentCameraRotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        //velocity = Vector3.zero;
    }

    public void RotateCamera(float _cameraRotationx)
    {
        cameraRotationx = _cameraRotationx;
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if(cam!=null)
        {
            currentCameraRotationX -= cameraRotationx;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); ;
        }
    }

    void PerformMovement()
    {
        if(velocity!=Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity*Time.fixedDeltaTime);
        }
        if(thrusterForce!=Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime,ForceMode.Acceleration);
        }
    }

}
