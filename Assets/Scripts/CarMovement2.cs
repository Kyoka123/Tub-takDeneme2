using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement2 : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _force = 50f;
    [SerializeField] private float _turnSpeed = 4.5f;
    [SerializeField] private float _mass = 1f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _gravityMultiplier = 1f;
    [SerializeField] private float _frictionCoefficient = 2.6f;
    [SerializeField] private float _angularDrag = 0.1f;
    private float _frictionForce;
    private float momentum;
    private float momentum_other;
    private float direction;
    private float velocityDirection;
    [SerializeField] private float collisionSpeed = 0.2f;
    [SerializeField] private float collisionSpeed_other = 0.4f;
    private Vector3 _input;
    private Vector2 inputRaw;


    bool isGrounded;

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.down * _gravity * _gravityMultiplier, ForceMode.Acceleration);

        if (isGrounded)
        {
            Vector3 _horizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

            GatherInput();
            Look();

            if (_horizontalVelocity == Vector3.zero)
            {
                _frictionForce = 0;
            }
            else
            {
                _frictionForce = _frictionCoefficient * _mass * _gravity;
            }

            _rb.AddForce(_horizontalVelocity.normalized * -_frictionForce);

            Move();
        }

        if (_input.x == 0)
        {
            float dampedY = Mathf.MoveTowards(_rb.angularVelocity.y, 0f, _angularDrag * Time.fixedDeltaTime);
            _rb.angularVelocity = new Vector3(0, dampedY, 0);
        }
    }

    private void GatherInput()
    {

        inputRaw = new Vector2(
               (Keyboard.current.rightArrowKey.isPressed ? 1 : 0) - (Keyboard.current.leftArrowKey.isPressed ? 1 : 0),
               (Keyboard.current.upArrowKey.isPressed ? 1 : 0) - (Keyboard.current.downArrowKey.isPressed ? 1 : 0));

        _input = new Vector3(inputRaw.x, 0f, inputRaw.y);
    }

    private void Look()
    {
        Vector3 _horizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        velocityDirection = Vector3.Dot(_rb.linearVelocity, transform.forward);

        if (_input.x == 0 || _horizontalVelocity.magnitude < 0.1f) return;

        float _angularVelocity = _input.x * (velocityDirection > 0 ? 1 : -1) * _turnSpeed * Mathf.Sqrt(_rb.linearVelocity.magnitude) / 5;
        _rb.angularVelocity = new Vector3(0f, _angularVelocity, 0f);
    }

    private void Move()
    {
        _rb.AddForce(transform.forward * _input.z * _force);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            Rigidbody otherRb = collision.rigidbody;
            momentum = _rb.mass * collision.relativeVelocity.magnitude * collisionSpeed;
            momentum_other = _rb.mass * collision.relativeVelocity.magnitude * collisionSpeed_other;

            ContactPoint contact = collision.contacts[0];
            Vector3 direction = contact.normal;

            _rb.AddForce(direction * momentum, ForceMode.Impulse);

            if (otherRb != null)
            {
                otherRb.AddForce(-direction * momentum_other, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}