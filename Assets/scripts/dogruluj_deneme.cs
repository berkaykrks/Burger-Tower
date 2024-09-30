using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogruluj_deneme : MonoBehaviour
{

    /*  (1)anlattigim yontemi yapmayi deneyecegim. sahnenin en altýna alt ekmegi ekledim
     ekmegin rigidbodysinde hareket etmesini engellemek için pozisyonlarý dondurdum.
    ekmeklere colider ekledim. simdi ortasýna gore puan hesaplama fonksiyonunu deneyecegim
    #emre
     */

    /*(2) yigin yapýsýný kullanarak ornek býr burger olusturacagým*/



    public GameObject[] taklit_edilcek_burger = new GameObject[5];
    public GameObject[] yapilan_burger = new GameObject[5];


    void Start()
    {

        #region Nesnenin ortasýný bulmak
        //Kod eklendiði objenin spriteýnýn orta noktasýný verir

        //SpriteRenderer sr = GetComponent<SpriteRenderer>();//objenin sprie rendererýna eriþme
        //Debug.Log(sr.bounds.center);//ortasýnýn kordinatlarýný consoleda yazdirma #emre
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
