using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement2 : MonoBehaviour
{
    [SerializeField] private Material _material;
    public Rigidbody _rb;
    [SerializeField] private float _force = 50f;
    [SerializeField] private float _turnSpeed = 4.5f;
    [SerializeField] private float _mass = 1f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _gravityMultiplier = 1f;
    [SerializeField] private float _frictionCoefficient = 2.6f;
    [SerializeField] private float _angularDrag = 0.1f;
    [SerializeField] private float _minimumVelocity = 0.1f;
    public int inverter = 1;
    private float _frictionForce;
    private float momentum;
    private float momentum_other;
    private float direction;
    private float velocityDirection;
    private int activePlatformCount;
    [SerializeField] private float collisionSpeed = 0.2f;
    [SerializeField] private float collisionSpeed_other = 0.4f;
    private Vector3 _input;
    private Vector2 inputRaw;

    bool isGrounded;
    bool speedPowerUpActive;
    bool strengthPowerUpActive;


    private void FixedUpdate()
    {

        /*if (_rb.linearVelocity.y < 0)
        {
            Vector3 _verticalAngularVelocity = new Vector3(0f, _rb.angularVelocity.y, 0f);
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _verticalAngularVelocity.y * 5f, _rb.linearVelocity.z);

        } */

        _rb.AddForce(Vector3.down * _gravity * _gravityMultiplier, ForceMode.Acceleration);

        if (isGrounded)
        {
            Vector3 _horizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

            GatherInput();
            Look();

            if (_horizontalVelocity.magnitude < _minimumVelocity)
            {
                _frictionForce = 0;
            }
            else
            {
                _frictionForce = _frictionCoefficient * _mass * _gravity;
                _rb.AddForce(_horizontalVelocity.normalized * -_frictionForce);
            }

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
               inverter * ((Keyboard.current.rightArrowKey.isPressed ? 1 : 0) - (Keyboard.current.leftArrowKey.isPressed ? 1 : 0)),
               inverter * ((Keyboard.current.upArrowKey.isPressed ? 1 : 0) - (Keyboard.current.downArrowKey.isPressed ? 1 : 0))
               );

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
        _rb.AddForce(transform.forward * _input.z * _mass * _force);
    }

    IEnumerator SpeedPowerUpRoutine()
    {
        speedPowerUpActive = true;
        _material.color = UnityEngine.Color.yellow * 4;
        _material.SetFloat("_Size", 0.6f);
        _force *= 1.4f; // Gücü artır
        collisionSpeed_other /= 1.4f; // Diğer aracın çarpışma etkisini azaltarak dengeler
        yield return new WaitForSeconds(3f); // 3 saniye bekle
        _force /= 1.4f; // Gücü eski haline getir
        collisionSpeed_other *= 1.4f; // Diğer aracın çarpışma etkisini eski haline getirerek düzeltir
        _material.SetFloat("_Size", 0f);
        _material.color = UnityEngine.Color.black;
        speedPowerUpActive = false;
    }

    IEnumerator StrengthPowerUpRoutine()
    {
        strengthPowerUpActive = true;
        _material.color = new UnityEngine.Color(0.03f, 0f, 0.003f) * 4;
        _material.SetFloat("_Size", 0.6f);
        collisionSpeed_other *= 3f; // Diğer aracın çarpışma etkisini arttırarak güçlenir
        yield return new WaitForSeconds(2f); // 2 saniye bekle
        collisionSpeed_other /= 3f; // Diğer aracın çarpışma etkisini azaltarak eski haline gelir
        _material.SetFloat("_Size", 0f);
        _material.color = UnityEngine.Color.black;
        strengthPowerUpActive = false;
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
        if (other.gameObject.tag == "Platform")
        {
            activePlatformCount++;
            isGrounded = true;
        }

        if (other.gameObject.tag == "speedPowerUpCube" && !speedPowerUpActive)
        {
            StartCoroutine(SpeedPowerUpRoutine());
        }

        if (other.gameObject.tag == "strengthPowerUpCube" && !strengthPowerUpActive)
        {
            StartCoroutine(StrengthPowerUpRoutine());
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            activePlatformCount--;
            if (activePlatformCount <= 0)
            {
                activePlatformCount = 0;
                isGrounded = false;
            }
        }
    }
}