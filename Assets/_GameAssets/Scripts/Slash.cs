using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] float dano;
    [SerializeField] float tiempoDestruccion;
    private int direccion;

    private void Start()
    {
        Collider2D[] cds = Physics2D.OverlapCapsuleAll(transform.position, new Vector2(0.7f, 2f), CapsuleDirection2D.Vertical, 90);
        foreach (Collider2D cd in cds)
        {
            if (cd.gameObject.GetComponent<Enemigo>() != null)
            {
                cd.gameObject.GetComponent<Enemigo>().RecibirDano(dano);
            }
        }
        Destroy(gameObject);
    }
}
