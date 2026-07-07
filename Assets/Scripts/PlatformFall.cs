using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlatformFall : MonoBehaviour
{
    private GameObject[] platforms;
    [SerializeField] private float minInterval = 2f;
    [SerializeField] private float maxInterval = 5f;
    [SerializeField] private float fallDelay = 0.5f;   // warning time before it drops
    [SerializeField] private float fallDuration = 2f;  // how long it stays fallen
    [SerializeField] private float respawnTime = 1f;   // time to move back into place

    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;
    private bool[] isBusy;

    private void Awake()
    {
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        originalPositions = new Vector3[platforms.Length];
        originalRotations = new Quaternion[platforms.Length];
        isBusy = new bool[platforms.Length];

        for (int i = 0; i < platforms.Length; i++)
        {
            originalPositions[i] = platforms[i].transform.position;
            originalRotations[i] = platforms[i].transform.rotation;
        }
    }

    private void Start()
    {
        
        StartCoroutine(RandomEventLoop());
    }

    private IEnumerator RandomEventLoop()
    {
        while (true)
        {
            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);

            int index = GetRandomAvailableIndex();
            if (index != -1)
            {
                StartCoroutine(DropAndRespawn(index));
            }
        }
    }

    private int GetRandomAvailableIndex()
    {
        // Avoid picking a platform that's already falling/respawning
        int startIndex = Random.Range(0, platforms.Length);
        for (int offset = 0; offset < platforms.Length; offset++)
        {
            int idx = (startIndex + offset) % platforms.Length;
            if (!isBusy[idx]) return idx;
        }
        return -1; // all busy
    }

    private IEnumerator DropAndRespawn(int index)
    {
        isBusy[index] = true;
        GameObject platform = platforms[index];
        Rigidbody rb = platform.GetComponent<Rigidbody>();

        // Optional: warning shake before it falls
        yield return new WaitForSeconds(fallDelay);

        // Let physics take over
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        yield return new WaitForSeconds(fallDuration);

        // Reset physics and snap/move back
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        yield return StartCoroutine(MoveBack(platform, originalPositions[index], originalRotations[index], respawnTime));

        isBusy[index] = false;
    }

    private IEnumerator MoveBack(GameObject platform, Vector3 targetPos, Quaternion targetRot, float duration)
    {
        Vector3 startPos = platform.transform.position;
        Quaternion startRot = platform.transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            platform.transform.position = Vector3.Lerp(startPos, targetPos, t);
            platform.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }

        platform.transform.position = targetPos;
        platform.transform.rotation = targetRot;
    }

}