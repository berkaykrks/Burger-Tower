using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burger_spawner : MonoBehaviour
{
    public GameObject burgerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 0.5f;

    // Oyunun baþýnda ilk burgeri spawnla
    void Start()
    {
        SpawnNextBurger();
    }

    // Yeni bir burger spawnla ve gecikme ekle
    public void SpawnNextBurger()
    {
        StartCoroutine(SpawnBurgerWithDelay());
    }

    // Gecikmeli burger spawnlama
    IEnumerator SpawnBurgerWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(burgerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
