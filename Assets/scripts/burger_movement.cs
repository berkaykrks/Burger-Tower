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
    public float stackTolerance = 0.2f;  // Stackleme tolerans�
    public static GameObject firstBurger = null;  // �lk d��en burger referans�
    public float yOffset = 0.5f;  // Stackleme y�ksekli�i (burgerin y�ksekli�i kadar)
    public bool hasStacked = false;  // Stacklenip stacklenmedi�ini kontrol etmek
    public float burgerHeight = 0.5f;  // Burger par�alar�n�n y�ksekli�i

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
            rig.constraints = RigidbodyConstraints2D.None;  // Y eksenindeki freeze'i kald�r
        }
    }

    // Burger'in yere veya di�er burgerlere de�ip de�medi�ini kontrol etmek i�in bir event
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasStacked)  // E�er daha �nce stacklenmediyse
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Burger")
            {
                if (firstBurger == null)  // �lk burger yere d��t�yse onu referans al
                {
                    firstBurger = this.gameObject;
                    Debug.Log("�lk burger yere d��t�, referans olarak ayarland�.");
                }
                else
                {
                    // Yeni burgerin ilk burgerin �st�ne stacklenmesini sa�la
                    float xDifference = Mathf.Abs(transform.position.x - firstBurger.transform.position.x);

                    if (xDifference <= stackTolerance)
                    {
                        Debug.Log("Stack ba�ar�l�! Burger �st �ste geldi.");
                        // Yeni burgeri ilk burgerin �st�ne hizala
                        transform.position = new Vector2(firstBurger.transform.position.x, firstBurger.transform.position.y + burgerHeight);
                        firstBurger = this.gameObject;  // Yeni burgeri referans burger yap

                        // Yeni burger stacklendikten sonra t�m hareketlerini dondur
                        rig.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                    else
                    {
                        Debug.Log("Stack ba�ar�s�z! �ok fazla kayma var.");
                        rig.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }

                // Stacklenme tamamland�, bir daha stacklenmesin
                hasStacked = true;

                // Yeni burger spawnla
                FindObjectOfType<burger_spawner>().SpawnNextBurger();
            }
        }
    }
}
