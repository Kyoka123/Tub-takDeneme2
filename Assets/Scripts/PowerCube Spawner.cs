using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PowerCubeSpawner : MonoBehaviour
{
    
    Vector3 spawnPos;
    bool isSpawning = false;
    List<GameObject> Cubes = new List<GameObject>();
    void Start()
    {
        GameObject speed = GameObject.FindGameObjectWithTag("speedPowerUpCube");
        GameObject str = GameObject.FindGameObjectWithTag("strengthPowerUpCube");
        Cubes.Add(speed);
        Cubes.Add(str);
        StartCoroutine(SpawnPowerUp());
    }
    IEnumerator SpawnPowerUp()
    {
        while (!isSpawning)
        {
            spawnPos = new Vector3(Random.Range(-20, 18), 2, Random.Range(2, 40));
            GameObject cube = Cubes[Random.Range(0, Cubes.Count)];
            GameObject spawnedCube = Instantiate(cube, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(5f);
            Destroy(spawnedCube); // Destroy the power-up after 5 seconds
            yield return new WaitForSeconds(7f); // Wait for 10 seconds before spawning the next power-up
            
        }
    }

   
}

