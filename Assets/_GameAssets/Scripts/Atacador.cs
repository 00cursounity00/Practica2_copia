using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atacador : MonoBehaviour
{
    [SerializeField] float fuerza;
    [SerializeField] float dano;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1.5f) * fuerza;
                if (collision.gameObject.transform.rotation.y == 0)
                {
                    collision.gameObject.transform.Rotate(new Vector3(0, 180, 0));
                }
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1.5f) * fuerza;
                if (collision.gameObject.transform.rotation.y == 180)
                {
                    collision.gameObject.transform.Rotate(new Vector3(0, 180, 0));
                }
            }
            collision.gameObject.GetComponent<Player>().RecibirDano(dano);
        }
    }
}
