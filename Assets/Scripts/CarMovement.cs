using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    /// <summary>
    /// script that handles the movement of the car, including acceleration, 
    /// turning, friction, and collision response. 
    /// It uses Rigidbody physics to apply forces and torques to the 
    /// car based on player input and interactions with other objects in the environment.
    /// </summary>
    /// gerekli deðerleri atamak için deðiþkenler oluþturduk
    /// hareket etme, dönme ve yer çekimlerini ayarlayacak deðiþkenler
    [SerializeField] private Material _material;
    public Rigidbody _rb;
    [SerializeField] private float _force = 50f;
    [SerializeField] private float _turnSpeed = 4.5f;
    [SerializeField] private float _mass = 1f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _gravityMultiplier = 1f;
    [SerializeField] private float _frictionCoefficient = 3.5f;
    [SerializeField] private float _sideGripCoefficient = 1.3f;
    [SerializeField] private float _angularDrag = 6f;
    [SerializeField] private float _minimumVelocity = 0.1f;
    public int inverter = 1;
    // çarpýþma sýrasýnda kullancaðýmýz deðerler
    private float _frictionForce;
    private float momentum;
    private float momentum_other;
    private float direction;
    private float velocityDirection;
    [SerializeField] private int activePlatformCount;
    private List<Collider> _objectsInTrigger = new List<Collider>();
    //çarpýþma gücünü belirlemek için deðiþkenler
    [SerializeField] private float collisionSpeed = 0.2f;
    [SerializeField] private float collisionSpeed_other = 0.4f;
    //hareket etmemizi saðlayacak vektör deðiþkenleri
    private Vector3 _input;
    private Vector2 inputRaw;

   //yere deðip deðmediðimizi gösterecek bool deðiþkeni
    private bool isGrounded;
    private bool speedPowerUpActive;
    private bool strengthPowerUpActive;


    private void FixedUpdate()
    {
        _objectsInTrigger.RemoveAll(item => item == null || !item.enabled || !item.gameObject.activeInHierarchy);
        activePlatformCount = _objectsInTrigger.Count;
        isGrounded = activePlatformCount >= 2;

        if (activePlatformCount >= 4)
        {
            _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.transform.position = new Vector3(_rb.transform.position.x, 0.07537979f, _rb.transform.position.z);
            _rb.transform.rotation = Quaternion.Euler(0, _rb.transform.rotation.eulerAngles.y, 0);
            _gravityMultiplier = 0f; // Yerçekimini devre dışı bırak
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.None;
            _gravityMultiplier = 1f; // Yerçekimini etkinleştir
        }


        //yer çekimini iþledik
        _rb.AddForce(Vector3.down * _gravity * _gravityMultiplier, ForceMode.Acceleration);

        if (isGrounded)
        {
            GatherInput();
            Look();

            // 1. Arabanın lokal hızlarını alıyoruz (X = Yan Kayma Hızı, Z = İleri/Geri Hızı)
            Vector3 localVelocity = transform.InverseTransformDirection(_rb.linearVelocity);

            // 2. İLERİ / GERİ SÜRTÜNME (Tekerlek Yuvarlanma Direnci)
            if (Mathf.Abs(localVelocity.z) > _minimumVelocity)
            {
                float forwardFriction = _frictionCoefficient * _mass * _gravity;
                // Sadece arabanın z eksenine (forward) ters yönde uygula
                _rb.AddForce(-transform.forward * Mathf.Sign(localVelocity.z) * forwardFriction);
            }

            // 3. YAN KAYMA SÜRTÜNMESİ (Yol Tutuşu / Grip)
            // Araba yana kayıyorsa (X hızı varsa) bunu sıfırlayacak ters bir yan kuvvet uygula
            if (Mathf.Abs(localVelocity.x) > _minimumVelocity)
            {
                // Buradaki multiplier (örn: 2f veya 5f) aracın ne kadar "drift" yapacağını belirler.
                // Yüksek olursa yapışır, düşük olursa kayar.
                float sideFriction = _frictionCoefficient * _mass * _gravity * _sideGripCoefficient;

                // Sadece arabanın X eksenine (right) ters yönde uygula
                _rb.AddForce(-transform.right * Mathf.Sign(localVelocity.x) * sideFriction);
            }

            Move();
        }

        //bu ne bilmiom
        if (_input.x == 0 && isGrounded)
        {
            float dampedY = Mathf.MoveTowards(_rb.angularVelocity.y, 0f, _angularDrag * Time.fixedDeltaTime);
            _rb.angularVelocity = new Vector3(_rb.angularVelocity.x, dampedY, _rb.angularVelocity.z);
        }
    }

    //input aldýk
    private void GatherInput()
    {

        inputRaw = new Vector2(
               inverter * ((Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0)),
               inverter * ((Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0))
               );

        _input = new Vector3(inputRaw.x, 0f, inputRaw.y);
    }

    //dönmeyi iþledik
    private void Look()
    {
        if (Mathf.Abs(_input.x) < 0.1f || _rb.linearVelocity.magnitude < 0.2f || !isGrounded)
        {
            return;
        }

        Vector3 _horizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        velocityDirection = Vector3.Dot(_rb.linearVelocity, transform.forward);

        if (_input.x == 0 || _horizontalVelocity.magnitude < 0.1f) return;

        float _angularVelocity = _input.x * (velocityDirection > 0 ? 1 : -1) * _turnSpeed * Mathf.Sqrt(_rb.linearVelocity.magnitude) / 5;
        _rb.angularVelocity = new Vector3(0f, _angularVelocity, 0f);
    }

    //aldýðýmýz deðerleri hareket etmek için kullandýk
    private void Move()
    {
        _rb.AddForce(transform.forward * _input.z * _mass * _force);
    }

    IEnumerator SpeedPowerUpRoutine()
    {
        speedPowerUpActive = true;
        _material.color = UnityEngine.Color.yellow * 4;
        _material.SetFloat("_Size", 0.3f);
        _force *= 1.4f; // Gücü artır
        collisionSpeed_other /= 1.4f; // Diğer aracın çarpışma etkisini azaltarak dengeler
        yield return new WaitForSeconds(5f); // 5 saniye bekle
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
        _material.SetFloat("_Size", 0.3f);
        collisionSpeed_other *= 3f; // Diğer aracın çarpışma etkisini arttırarak güçlenir
        yield return new WaitForSeconds(4f); // 4 saniye bekle
        collisionSpeed_other /= 3f; // Diğer aracın çarpışma etkisini azaltarak eski haline gelir
        _material.SetFloat("_Size", 0f);
        _material.color = UnityEngine.Color.black;
        strengthPowerUpActive = false;
    }

    //çarpýþma etkisini iþledik
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player"|| collision.gameObject.tag == "Player2")
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

    //yere deðip deðmediðine baktýk
    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.tag == "Surface" )
        {
            _objectsInTrigger.Add(other);
        }

        if(other.gameObject.tag == "speedPowerUpCube" && !speedPowerUpActive)
        {
            StartCoroutine(SpeedPowerUpRoutine());
        }

        if (other.gameObject.tag == "strengthPowerUpCube" && !strengthPowerUpActive)
        {
            StartCoroutine(StrengthPowerUpRoutine());
        }
    }
    //deðmiiyorsa false atadýk
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Surface")
        {
            _objectsInTrigger.Remove(other);
        }
    }

}

    