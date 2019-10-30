﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float velocidad;
    [SerializeField] float fuerza;
    [SerializeField] float fuerzaSalto;
    [SerializeField] GameObject prefabProyectil;
    [SerializeField] Transform puntoDisparoSuelo;
    [SerializeField] Transform puntoDisparoAire;
    [SerializeField] Transform detectorSuelo;
    [SerializeField] LayerMask layerSuelo;
    [SerializeField] PhysicsMaterial2D pm2d;
    private AudioSource[] audios;
    private float x, y;
    private Rigidbody2D rb;
    private GameManager gm;
    private Animator animator;
    private const int AUDIO_SHURIKEN = 0;
    private const int AUDIO_JUMP = 1;
    private bool enSuelo = false;
    public enum EstadoPlayer {normal, recibiendoDano};
    public EstadoPlayer estadoPlayer = EstadoPlayer.normal;
    private Vector2 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        audios = GetComponents<AudioSource>();
        IniciarPosicion();
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        ObtenerEnSuelo();
        if (x > 0)
        {
            //x = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        else if (x < 0)
        {
            //x = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("disparando", true);
            Invoke("QuitarDisparar", 0.1f);
            Disparar();
            //audios[AUDIO_SHURIKEN].Play();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Saltar();
            //audios[AUDIO_JUMP].Play();
        }
    }

    void FixedUpdate()
    {
        if (estadoPlayer == EstadoPlayer.normal)
        {
            if (Mathf.Abs(x) > 0.1f)
            {
                animator.SetBool("corriendo", true);
                rb.velocity = new Vector2(x * velocidad, rb.velocity.y);
            }
            else
            {
                animator.SetBool("corriendo", false);
                //rb.velocity = new Vector2(0, 0);
            }
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            transform.SetParent(collision.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            transform.SetParent(null);
        }
    }

    public void RecibirDano(float dano)
    {
        if (gm.QuitarVida(dano))
        {
            IniciarPosicion();
            gm.ResetGame();
        }
        estadoPlayer = EstadoPlayer.recibiendoDano;
        animator.SetBool("recibiendoDano", true);
        Invoke("QuitarRecibirDano", 0.5f);
    }

    private void QuitarRecibirDano()
    {
        estadoPlayer = EstadoPlayer.normal;
        animator.SetBool("recibiendoDano", false);
    }

    private void Disparar()
    {
        if (animator.GetBool("enSuelo"))
        {
            //print("suelo");
            GameObject proyectil = Instantiate(prefabProyectil, puntoDisparoSuelo.position, puntoDisparoSuelo.rotation);
            //proyectil.GetComponent<Rigidbody2D>().AddForce(puntoDisparo.right * fuerza);
        }
        else
        {
            //print("aire");
            GameObject proyectil = Instantiate(prefabProyectil, puntoDisparoAire.position, puntoDisparoAire.rotation);
        }
    }

    private void QuitarDisparar()
    {
        animator.SetBool("disparando", false);
    }

    private void Saltar()
    {
        if (ObtenerEnSuelo() || ObtenerEnAgua())
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            animator.SetBool("saltando", true);
            Invoke("QuitarSaltar", 0.1f);
            //rb.AddForce(new Vector2(0, 1) * fuerzaSalto);
        }
    }

    private void QuitarSaltar()
    {
        animator.SetBool("saltando", false);
    }

    private bool ObtenerEnAgua()
    {
        return animator.GetBool("enAgua");
    }

    private bool ObtenerEnSuelo()
    {
        Collider2D cd = Physics2D.OverlapBox(detectorSuelo.position, new Vector2(0.8f,0.1f), 0, layerSuelo);

        if (cd != null)
        {
            foreach (CapsuleCollider2D cc in GetComponents<CapsuleCollider2D>())
            {
                cc.sharedMaterial = null;
            }
            GetComponent<BoxCollider2D>().sharedMaterial = null;
            animator.SetBool("enSuelo", true);
            return true;
        }

        foreach (CapsuleCollider2D cc in GetComponents<CapsuleCollider2D>())
        {
            cc.sharedMaterial = pm2d;
        }

        GetComponent<BoxCollider2D>().sharedMaterial = pm2d;
        animator.SetBool("enSuelo", false);
        return false;
    }

    private void IniciarPosicion()
    {
        transform.position = gm.ObtenerPosicionPlayer(posicionInicial);
        GameObject.Find("ParallaxBackground").transform.position = transform.position;
    }
}
