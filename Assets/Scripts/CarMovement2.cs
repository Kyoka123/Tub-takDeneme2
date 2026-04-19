using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement2 : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _force = 50f;
    [SerializeField] private float _turnSpeed = 220f;
    private float momentum;
    private float momentum_other;
    private float direction;
    private float velocityDirection;
    [SerializeField] private float collisionSpeed = 0.2f;
    [SerializeField] private float collisionSpeed_other = 0.4f;
    private Vector3 _input;
    private Vector2 inputRaw;

    private void Update()
    {
        GatherInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
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
        velocityDirection = Vector3.Dot(_rb.linearVelocity, transform.forward);

        if (_input == Vector3.zero || _rb.linearVelocity.magnitude < 0.1f) return;

        transform.Rotate(Vector3.up * _input.x * (velocityDirection > 0 ? 1 : -1) * _turnSpeed * Mathf.Sqrt(_rb.linearVelocity.magnitude) /5 * Time.deltaTime);
    }

    private void Move()
    {
        _rb.AddForce(transform.forward * _input.z * _force);
    }

    void OnCollisionEnter(Collision collision)
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