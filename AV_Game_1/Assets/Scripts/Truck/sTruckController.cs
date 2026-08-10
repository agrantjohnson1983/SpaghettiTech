using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class sTruckController : MonoBehaviour
{
    [Header("Driving")]
    public float acceleration = 10f;
    public float maxSpeed = 12f;
    public float reverseSpeed = 5f;


[Header("Steering")]
    public float steering = 80f;

    private Rigidbody rb;

    private float moveInput;
    private float steerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Drive();
        Steer();
        LimitSpeed();

        Debug.DrawRay(
            transform.position,
            Vector3.forward * 3f,
            Color.red
        );
    }

    private void Drive()
    {
        if (Mathf.Abs(moveInput) < 0.01f)
            return;

        Vector3 force =
            Vector3.forward *
            moveInput *
            acceleration;

        rb.AddForce(force, ForceMode.Acceleration);
    }

    private void Steer()
    {
        if (Mathf.Abs(steerInput) < 0.01f)
            return;

        float speed = rb.velocity.magnitude;

        if (speed < 0.1f)
            return;

        float turnAmount =
            steerInput *
            steering *
            Time.fixedDeltaTime;

        if (moveInput < 0f)
            turnAmount *= -1f;

        rb.MoveRotation(
            rb.rotation *
            Quaternion.Euler(0f, turnAmount, 0f)
        );
    }

    private void LimitSpeed()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;

        float currentMaxSpeed =
            moveInput < 0f
                ? reverseSpeed
                : maxSpeed;

        if (horizontalVelocity.magnitude > currentMaxSpeed)
        {
            horizontalVelocity =
                horizontalVelocity.normalized *
                currentMaxSpeed;

            rb.velocity = new Vector3(
                horizontalVelocity.x,
                rb.velocity.y,
                horizontalVelocity.z
            );
        }
    }


}
