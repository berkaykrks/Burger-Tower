using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerStacker : MonoBehaviour
{
    [SerializeField] private Transform burgerPrefab;
    [SerializeField] private Transform burgerHolder;

    private Transform currentBurger = null;
    private Rigidbody2D currentRigidbody;

    public float burgerSpeed = 8f;
    public float burgerSpeedIncrement = 0.5f;
    public int burgerDirection = 1;

    public Vector2 burgerStartPos;
    public int xLimit = 5;

    // �lk burgeri referans olarak tutmak i�in
    private Transform firstBurger = null;
    public float tolerance = 0.5f; // Burgerlerin hizalanmas� i�in tolerans de�eri

    void Start()
    {
        SpawnNewBurger();
    }

    void Update()
    {
        if (currentBurger)
        {
            // Burgerin hareket etmesi
            float moveAmount = Time.deltaTime * burgerSpeed * burgerDirection;
            currentBurger.position += new Vector3(moveAmount, 0, 0);

            // X limitlerine ula��nca y�n de�i�tir
            if (Mathf.Abs(currentBurger.position.x) > xLimit)
            {
                currentBurger.position = new Vector3(burgerDirection * xLimit, currentBurger.position.y, 0);
                burgerDirection = -burgerDirection;
            }

            // Space tu�una bas�nca burgeri b�rak ve yeni burger spawn et
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentRigidbody.simulated = true; // Burgerin d��mesini ba�lat

                // �lk burgeri referans olarak sakla
                if (firstBurger == null)
                {
                    firstBurger = currentBurger;
                }
                else
                {
                    // Sonraki burgerler i�in tolerans kontrol�
                    float xDifference = Mathf.Abs(currentBurger.position.x - firstBurger.position.x);

                    if (xDifference <= tolerance)
                    {
                        Debug.Log("iyi!");
                    }
                    else
                    {
                        Debug.Log("fark " + xDifference);
                    }
                }

                currentBurger = null; // Mevcut burgeri s�f�rla
                SpawnNewBurger(); // Yeni burger spawn et
            }
        }
    }

    private void SpawnNewBurger()
    {
        // Yeni burger olu�tur
        currentBurger = Instantiate(burgerPrefab, burgerHolder);
        currentBurger.position = burgerStartPos;
        currentRigidbody = currentBurger.GetComponent<Rigidbody2D>();

        // Yeni burger d��t��� anda fizik sim�lasyonunu devre d��� b�rak
        currentRigidbody.simulated = false;

        // H�z art���
        burgerSpeed += burgerSpeedIncrement;
    }
}
