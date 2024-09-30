using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogruluj_deneme : MonoBehaviour
{

    /*  (1)anlattigim yontemi yapmayi deneyecegim. sahnenin en alt�na alt ekmegi ekledim
     ekmegin rigidbodysinde hareket etmesini engellemek i�in pozisyonlar� dondurdum.
    ekmeklere colider ekledim. simdi ortas�na gore puan hesaplama fonksiyonunu deneyecegim
    #emre
     */

    /*(2) yigin yap�s�n� kullanarak ornek b�r burger olusturacag�m*/



    public GameObject[] taklit_edilcek_burger = new GameObject[5];
    public GameObject[] yapilan_burger = new GameObject[5];


    void Start()
    {

        #region Nesnenin ortas�n� bulmak
        //Kod eklendi�i objenin sprite�n�n orta noktas�n� verir

        //SpriteRenderer sr = GetComponent<SpriteRenderer>();//objenin sprie renderer�na eri�me
        //Debug.Log(sr.bounds.center);//ortas�n�n kordinatlar�n� consoleda yazdirma #emre
        #endregion


        for(int i = 0; i < taklit_edilcek_burger.Length ; i++)
        {
            GameObject obj = taklit_edilcek_burger[i];
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            Debug.Log(sr.bounds.center + " "+obj.name);
        }


    }

    double base_x_pos = -1.417775;
    double toplam_hata = 0;
    double a = 0;

    void benzerlik_hesapla()
    {
        for (int i = 0; i<taklit_edilcek_burger.Length;i++)
        {

            if (yapilan_burger[i].name == taklit_edilcek_burger[i].name)
                a = 1;

           

        }

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {


        }



    }
}
