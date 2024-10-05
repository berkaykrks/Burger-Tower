using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burger_movement : MonoBehaviour
{
    public float speed;
    public float max_pos_x;
    public float min_pos_x;
    private Rigidbody2D rig;
    public bool is_falling = false;
    public float stackTolerance = 0.2f;  // Stackleme toleransý
    public static GameObject firstBurger = null;  // Ýlk düþen burger referansý
    public float yOffset = 0.5f;  // Stackleme yüksekliði (burgerin yüksekliði kadar)
    public bool hasStacked = false;  // Stacklenip stacklenmediðini kontrol etmek
    public float burgerHeight = 0.5f;  // Burger parçalarýnýn yüksekliði

    void Start()
    {
        rig = this.gameObject.GetComponent<Rigidbody2D>();
        rig.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        is_falling = false;
    }

    void Update()
    {
        if (!is_falling)
        {
            float pingPong = Mathf.PingPong(Time.time * speed, max_pos_x - min_pos_x) + min_pos_x;
            transform.position = new Vector2(pingPong, transform.position.y);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            is_falling = true;
            rig.constraints = RigidbodyConstraints2D.None;  // Y eksenindeki freeze'i kaldýr
        }
    }

    // Burger'in yere veya diðer burgerlere deðip deðmediðini kontrol etmek için bir event
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasStacked)  // Eðer daha önce stacklenmediyse
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Burger")
            {
                if (firstBurger == null)  // Ýlk burger yere düþtüyse onu referans al
                {
                    firstBurger = this.gameObject;
                    Debug.Log("Ýlk burger yere düþtü, referans olarak ayarlandý.");
                }
                else
                {
                    // Yeni burgerin ilk burgerin üstüne stacklenmesini saðla
                    float xDifference = Mathf.Abs(transform.position.x - firstBurger.transform.position.x);

                    if (xDifference <= stackTolerance)
                    {
                        Debug.Log("Stack baþarýlý! Burger üst üste geldi.");
                        // Yeni burgeri ilk burgerin üstüne hizala
                        transform.position = new Vector2(firstBurger.transform.position.x, firstBurger.transform.position.y + burgerHeight);
                        firstBurger = this.gameObject;  // Yeni burgeri referans burger yap

                        // Yeni burger stacklendikten sonra tüm hareketlerini dondur
                        rig.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                    else
                    {
                        Debug.Log("Stack baþarýsýz! Çok fazla kayma var.");
                        rig.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }

                // Stacklenme tamamlandý, bir daha stacklenmesin
                hasStacked = true;

                // Yeni burger spawnla
                FindObjectOfType<burger_spawner>().SpawnNextBurger();
            }
        }
    }
}
