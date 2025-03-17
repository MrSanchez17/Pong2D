using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;
    public float powerUpSpawnInterval = 10f;
    private float minX = -7f, maxX = 7f, minY = -3f, maxY = 3f;

    void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            yield return new WaitForSeconds(powerUpSpawnInterval);
            SpawnPowerUpObject();
        }
    }

    void SpawnPowerUpObject()
    {
        if (powerUpPrefabs.Length == 0) return;

        Vector2 spawnPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject powerUp = Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)], spawnPos, Quaternion.identity);

        Destroy(powerUp, 5f); // Se elimina si no se recoge
    }
}