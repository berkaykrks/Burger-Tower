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

    // Ýlk burgeri referans olarak tutmak için
    private Transform firstBurger = null;
    public float tolerance = 0.5f; // Burgerlerin hizalanmasý için tolerans deðeri

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

            // X limitlerine ulaþýnca yön deðiþtir
            if (Mathf.Abs(currentBurger.position.x) > xLimit)
            {
                currentBurger.position = new Vector3(burgerDirection * xLimit, currentBurger.position.y, 0);
                burgerDirection = -burgerDirection;
            }

            // Space tuþuna basýnca burgeri býrak ve yeni burger spawn et
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentRigidbody.simulated = true; // Burgerin düþmesini baþlat

                // Ýlk burgeri referans olarak sakla
                if (firstBurger == null)
                {
                    firstBurger = currentBurger;
                }
                else
                {
                    // Sonraki burgerler için tolerans kontrolü
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

                currentBurger = null; // Mevcut burgeri sýfýrla
                SpawnNewBurger(); // Yeni burger spawn et
            }
        }
    }

    private void SpawnNewBurger()
    {
        // Yeni burger oluþtur
        currentBurger = Instantiate(burgerPrefab, burgerHolder);
        currentBurger.position = burgerStartPos;
        currentRigidbody = currentBurger.GetComponent<Rigidbody2D>();

        // Yeni burger düþtüðü anda fizik simülasyonunu devre dýþý býrak
        currentRigidbody.simulated = false;

        // Hýz artýþý
        burgerSpeed += burgerSpeedIncrement;
    }
}
