using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ping_pong : MonoBehaviour
{
    public GameObject burger_prefab;  // Public yaparak Unity'den atama yapabilirsin
    public Transform spawn_point;     // Spawn noktasýný da Unity'den ayarlayabilirsin

    public bool is_falling = false;   // Burger düþüyor mu kontrolü
    public float speed;               // Saða sola hareket hýzý
    public float max_pos_x;           // X ekseninde maksimum nokta
    public float min_pos_x;           // X ekseninde minimum nokta
    public float spawn_delay = 0.5f;  // Spawn gecikmesi

    private Rigidbody2D current_rig;  // Mevcut burger'in rigidbody'si
    private GameObject current_burger;  // Mevcut burger objesi

    void Start()
    {
        // Ýlk burger parçasýný spawnla
        SpawnNewBurger();
    }

    void Update()
    {
        if (!is_falling && current_burger != null)
        {
            // Burger saða sola hareket ediyor
            float pingPong = Mathf.PingPong(Time.time * speed, max_pos_x - min_pos_x) + min_pos_x;
            current_burger.transform.position = new Vector2(pingPong, current_burger.transform.position.y);
        }

        // Space tuþuna basýldýðýnda burger'i düþür
        if (Input.GetKeyDown(KeyCode.Space) && !is_falling)
        {
            is_falling = true;
            // Y eksenindeki dondurmayý kaldýr, burgeri düþür
            current_rig.constraints = RigidbodyConstraints2D.None;
            // Yeni bir burger parçasý spawnlamadan önce delay eklemek için coroutine baþlat
            StartCoroutine(SpawnNewBurgerWithDelay());
        }
    }

    // Yeni burger parçasý spawnla
    void SpawnNewBurger()
    {
        current_burger = Instantiate(burger_prefab, spawn_point.position, Quaternion.identity);
        current_rig = current_burger.GetComponent<Rigidbody2D>();
        current_rig.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        is_falling = false;
    }

    // Coroutine: Gecikme ile yeni burger parçasý spawnlamak için
    IEnumerator SpawnNewBurgerWithDelay()
    {
        yield return new WaitForSeconds(spawn_delay);  // Gecikme süresi
        SpawnNewBurger();  // Yeni burger parçasýný spawnla
    }
}
