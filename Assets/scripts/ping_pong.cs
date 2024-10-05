using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ping_pong : MonoBehaviour
{
    public GameObject burger_prefab;  // Public yaparak Unity'den atama yapabilirsin
    public Transform spawn_point;     // Spawn noktas�n� da Unity'den ayarlayabilirsin

    public bool is_falling = false;   // Burger d���yor mu kontrol�
    public float speed;               // Sa�a sola hareket h�z�
    public float max_pos_x;           // X ekseninde maksimum nokta
    public float min_pos_x;           // X ekseninde minimum nokta
    public float spawn_delay = 0.5f;  // Spawn gecikmesi

    private Rigidbody2D current_rig;  // Mevcut burger'in rigidbody'si
    private GameObject current_burger;  // Mevcut burger objesi

    void Start()
    {
        // �lk burger par�as�n� spawnla
        SpawnNewBurger();
    }

    void Update()
    {
        if (!is_falling && current_burger != null)
        {
            // Burger sa�a sola hareket ediyor
            float pingPong = Mathf.PingPong(Time.time * speed, max_pos_x - min_pos_x) + min_pos_x;
            current_burger.transform.position = new Vector2(pingPong, current_burger.transform.position.y);
        }

        // Space tu�una bas�ld���nda burger'i d���r
        if (Input.GetKeyDown(KeyCode.Space) && !is_falling)
        {
            is_falling = true;
            // Y eksenindeki dondurmay� kald�r, burgeri d���r
            current_rig.constraints = RigidbodyConstraints2D.None;
            // Yeni bir burger par�as� spawnlamadan �nce delay eklemek i�in coroutine ba�lat
            StartCoroutine(SpawnNewBurgerWithDelay());
        }
    }

    // Yeni burger par�as� spawnla
    void SpawnNewBurger()
    {
        current_burger = Instantiate(burger_prefab, spawn_point.position, Quaternion.identity);
        current_rig = current_burger.GetComponent<Rigidbody2D>();
        current_rig.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        is_falling = false;
    }

    // Coroutine: Gecikme ile yeni burger par�as� spawnlamak i�in
    IEnumerator SpawnNewBurgerWithDelay()
    {
        yield return new WaitForSeconds(spawn_delay);  // Gecikme s�resi
        SpawnNewBurger();  // Yeni burger par�as�n� spawnla
    }
}
