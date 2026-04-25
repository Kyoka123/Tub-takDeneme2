using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlatformFall : MonoBehaviour
{
    public float fallDelay = 1f; // Time before the platform starts falling
    public float resetDelay = 5f; // Time before the platform resets to its original position
    public float fallSpeed = 5f; // Speed at which the platform falls
    public Vector3 resetPosition; // Original position of the platform
    public Vector3 resetVelocity = new Vector3(0, 0, 0);
    private Rigidbody rb;

    void Start()
    {
        resetPosition = transform.position; // Store the original position of the platform
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Make the platform static at the start    
    }


    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            StartCoroutine(FallDelay());
        }

        if (rb.isKinematic == false)
        {
            Invoke(nameof(Reset), resetDelay); // Schedule the platform to reset after the specified delay
            rb.linearVelocity = new Vector3(0, -fallSpeed, 0); // Make the platform fall downwards
        }
    }

    private void Reset()
    {
        rb.isKinematic = true; // Make the platform static again
        transform.position = resetPosition; // Reset the platform's position
        rb.linearVelocity = resetVelocity; // Reset the platform's velocity
    }

    // add a coroutine to make a lil animation before falling
    IEnumerator FallDelay()
    {
        float timer = fallDelay;

        while (timer > 0)
        {
            transform.position += Random.insideUnitSphere * 0.02f;
            timer -= Time.deltaTime;
            yield return null;
        }

        rb.isKinematic = false;
    }
}
