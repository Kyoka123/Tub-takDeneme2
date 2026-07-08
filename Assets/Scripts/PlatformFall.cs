using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UIElements.UxmlAttributeDescription;
public class PlatformFall : MonoBehaviour
{
    [SerializeField] private float mininterval = 1f;
    [SerializeField] private float maxinterval = 5f;
    [SerializeField] private float DurationItsGone = 3f;
    private GameObject[] platforms;
    
    private bool[] used;
    private void Awake()
    {
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        used = new bool[platforms.Length];
    }
    int GetRandomAvailableIndex()
    {
        List<int> available = new List<int>();

        for (int i = 0; i < used.Length; i++)
        {
            if (!used[i])
                available.Add(i);
        }

        if (available.Count == 0)
            return -1;

        return available[Random.Range(0, available.Count)];
    }

    private void Start()
    {
        StartCoroutine(ShakeAndSetPlatformFalse());
    }

    IEnumerator ShakeAndSetPlatformFalse()
    {
        while (true)
        {
            float timeToWait = Random.Range(mininterval, maxinterval);
            yield return new WaitForSeconds(timeToWait);
            int index = GetRandomAvailableIndex();
            if (index == -1)
                yield break;
            Vector3 originalPos = platforms[index].transform.localPosition;

            float duration = 1f;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;

                float x = Mathf.Sin(timer * 30f) * 0.1f;
                platforms[index].transform.localPosition = originalPos + new Vector3(x, 0, 0);

                yield return null;
            }

            // Eski konumuna dönsün
            platforms[index].transform.localPosition = originalPos;
            used[index] = true;
            platforms[index].SetActive(false);
            yield return new WaitForSeconds(DurationItsGone);
            platforms[index].SetActive(true);
            used[index] = false;
        }
    }


}
    
