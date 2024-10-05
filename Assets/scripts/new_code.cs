using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class new_code : MonoBehaviour
{

    [SerializeField] private Transform burger_prefab;
    [SerializeField] private Transform burger_holder;

    private Transform current_burger = null;
    private Rigidbody2D current_rigidbody;

    public float burger_speed = 8f;
    public float burger_speed_ýncerement = 0.5f;
    public int burger_direction = 1;

    public Vector2 burger_start_pos;

    public int x_limit = 5;
    void Start()
    {

        spawn_new_burger();
    }


    void Update()
    {
        if (current_burger)
        {
            float move_amount = Time.deltaTime * burger_speed * burger_direction;
            current_burger.position += new Vector3(move_amount,0,0);
            if (Mathf.Abs(current_burger.position.x)> x_limit)
            {
                current_burger.position = new Vector3(burger_direction*x_limit,current_burger.position.y,0);

                burger_direction = -burger_direction;

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                current_burger = null;
                current_rigidbody.simulated = true;
                spawn_new_burger();
            }

        }
        
    }

    private void spawn_new_burger()
    {
        current_burger = Instantiate(burger_prefab,burger_holder);
        current_burger.position = burger_start_pos;
        current_rigidbody = current_burger.GetComponent<Rigidbody2D>();
        burger_speed += burger_speed_ýncerement;


        
    }
}
