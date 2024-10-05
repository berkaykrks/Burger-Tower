using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerStacker : MonoBehaviour
{
    [SerializeField] private Transform burgerPrefab;
    [SerializeField] private Transform burgerHolder;
    [SerializeField] private Camera mainCamera; // Kameray� referans alaca��z

    private Transform currentBurger = null;
    private Rigidbody2D currentRigidbody;

    public float burgerSpeed = 8f;
    public float burgerSpeedIncrement = 0.5f;
    public int burgerDirection = 1;

    public Vector2 burgerStartPos; // Burgerin ba�lang�� pozisyonu
    public int xLimit = 5;

    // �lk burgeri referans olarak tutmak i�in
    private Transform firstBurger = null;
    public float tolerance = 0.5f; // Burgerlerin hizalanmas� i�in tolerans de�eri

    private int spawnCount = 0; // Her 3 spawn'da bir kontrol i�in saya�
    public float cameraMoveStep = 5f; // Kamera ve spawn pozisyonunun her hareketinde yukar� ��kaca�� mesafe
    public float moveDuration = 1f; // Kameran�n yukar�ya ��k�� s�resi (yumu�ak ge�i� i�in)

    private bool isMovingCamera = false; // Kamera hareket ederken ba�ka hareket olmas�n diye kontrol
    private Vector3 cameraTargetPosition; // Kameran�n gitmek istedi�i hedef pozisyon

    void Start()
    {
        SpawnNewBurger();
    }

    void Update()
    {
        if (currentBurger && !isMovingCamera) // Kamera hareket halindeyken burgerler hareket etmesin
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
                        Debug.Log("Burger is aligned correctly!");
                    }
                    else
                    {
                        Debug.Log("Burger is not aligned! X Difference: " + xDifference);
                    }
                }

                currentBurger = null; // Mevcut burgeri s�f�rla
                spawnCount++; // Her spawn sonras� saya� art���

                // Her 3 burger spawn'da bir kamera ve spawn pozisyonunu yukar� kayd�r
                if (spawnCount % 6 == 0)
                {
                    StartCoroutine(MoveCameraUp());
                }
                else
                {
                    SpawnNewBurger(); // Yeni burger spawn et
                }
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

    // Kamera ve spawn pozisyonunu yukar� hareket ettiren coroutine
    private IEnumerator MoveCameraUp()
    {
        isMovingCamera = true; // Kamera hareket ederken ba�ka i�lem yap�lmas�n

        Vector3 startPos = mainCamera.transform.position; // Kameran�n mevcut pozisyonu
        Vector3 targetPos = startPos + new Vector3(0, cameraMoveStep, 0); // Kameran�n hedef pozisyonu

        Vector2 newBurgerStartPos = burgerStartPos + new Vector2(0, cameraMoveStep); // Burgerin yeni spawn pozisyonu

        float elapsedTime = 0f;

        // Kameray� ve spawn pozisyonunu yumu�ak �ekilde yukar� ta��
        while (elapsedTime < moveDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration);
            burgerStartPos = Vector2.Lerp(burgerStartPos, newBurgerStartPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hareket bitti�inde tam olarak hedef pozisyona ayarla
        mainCamera.transform.position = targetPos;
        burgerStartPos = newBurgerStartPos;

        isMovingCamera = false; // Kamera hareketi bitti
        SpawnNewBurger(); // Yeni burger spawn et
    }
}
