using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;
    private PlayerMotor motor;
    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]
    private float thrusterForce = 1200f;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [SerializeField]
    private LayerMask envMask;

    private Animator animator;
    private ConfigurableJoint joint;

    [Header("Spring Settings;")]
    [SerializeField]
    private float jointSpring=15f;
    [SerializeField]
    private float jointMaxForce = 40f;
    


    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSettings(jointSpring);
    }

    void Update()
    {

        RaycastHit _hit;
        if(Physics.Raycast(transform.position,Vector3.down,out _hit, 100f,envMask))
        {
            joint.targetPosition = new Vector3(0f,-_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov; // (1,0,0)
        Vector3 _movVertical = transform.forward * _zMov; // (0,0,1)

        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        //Animate Movement
        animator.SetFloat("ForwardVelocity", _zMov);

        motor.Move(_velocity);

        //turning around
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        motor.Rotate(_rotation);

        //camera rotation
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationx = _xRot * lookSensitivity;

        motor.RotateCamera(_cameraRotationx);

        //Thruster
        Vector3 _thrusterForce=Vector3.zero;
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if(thrusterFuelAmount>=0.01f)
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }

        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

            SetJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        motor.ApplyThruster(_thrusterForce);

    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }

}
