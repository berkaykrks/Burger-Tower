using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerStacker : MonoBehaviour
{
    [SerializeField] private Transform burgerPrefab;
    [SerializeField] private Transform burgerHolder;
    [SerializeField] private Camera mainCamera; // Kamerayý referans alacaðýz

    private Transform currentBurger = null;
    private Rigidbody2D currentRigidbody;

    public float burgerSpeed = 8f;
    public float burgerSpeedIncrement = 0.5f;
    public int burgerDirection = 1;

    public Vector2 burgerStartPos; // Burgerin baþlangýç pozisyonu
    public int xLimit = 5;

    // Ýlk burgeri referans olarak tutmak için
    private Transform firstBurger = null;
    public float tolerance = 0.5f; // Burgerlerin hizalanmasý için tolerans deðeri

    private int spawnCount = 0; // Her 3 spawn'da bir kontrol için sayaç
    public float cameraMoveStep = 5f; // Kamera ve spawn pozisyonunun her hareketinde yukarý çýkacaðý mesafe
    public float moveDuration = 1f; // Kameranýn yukarýya çýkýþ süresi (yumuþak geçiþ için)

    private bool isMovingCamera = false; // Kamera hareket ederken baþka hareket olmasýn diye kontrol
    private Vector3 cameraTargetPosition; // Kameranýn gitmek istediði hedef pozisyon

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
                        Debug.Log("Burger is aligned correctly!");
                    }
                    else
                    {
                        Debug.Log("Burger is not aligned! X Difference: " + xDifference);
                    }
                }

                currentBurger = null; // Mevcut burgeri sýfýrla
                spawnCount++; // Her spawn sonrasý sayaç artýþý

                // Her 3 burger spawn'da bir kamera ve spawn pozisyonunu yukarý kaydýr
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
        // Yeni burger oluþtur
        currentBurger = Instantiate(burgerPrefab, burgerHolder);
        currentBurger.position = burgerStartPos;
        currentRigidbody = currentBurger.GetComponent<Rigidbody2D>();

        // Yeni burger düþtüðü anda fizik simülasyonunu devre dýþý býrak
        currentRigidbody.simulated = false;

        // Hýz artýþý
        burgerSpeed += burgerSpeedIncrement;
    }

    // Kamera ve spawn pozisyonunu yukarý hareket ettiren coroutine
    private IEnumerator MoveCameraUp()
    {
        isMovingCamera = true; // Kamera hareket ederken baþka iþlem yapýlmasýn

        Vector3 startPos = mainCamera.transform.position; // Kameranýn mevcut pozisyonu
        Vector3 targetPos = startPos + new Vector3(0, cameraMoveStep, 0); // Kameranýn hedef pozisyonu

        Vector2 newBurgerStartPos = burgerStartPos + new Vector2(0, cameraMoveStep); // Burgerin yeni spawn pozisyonu

        float elapsedTime = 0f;

        // Kamerayý ve spawn pozisyonunu yumuþak þekilde yukarý taþý
        while (elapsedTime < moveDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration);
            burgerStartPos = Vector2.Lerp(burgerStartPos, newBurgerStartPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hareket bittiðinde tam olarak hedef pozisyona ayarla
        mainCamera.transform.position = targetPos;
        burgerStartPos = newBurgerStartPos;

        isMovingCamera = false; // Kamera hareketi bitti
        SpawnNewBurger(); // Yeni burger spawn et
    }
}
