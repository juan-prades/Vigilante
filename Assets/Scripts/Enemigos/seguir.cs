using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class seguir : MonoBehaviour
{
    public Animator animator;
    public float MoveSpeed = 3f;    // Velocidad de persecucion
    public float rangoAtaque = 2f;  // Distancia a la que ataca al jugador
    public AudioClip sonidoFantasma; // Sonido del fantasma al atacar

    NavMeshAgent agente; // Mueve al fantasma esquivando paredes
    AudioSource audioFantasma; // Fuente de sonido del fantasma
    bool atacando = false; // Para que el sonido no se repita cada frame

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();

        // Preparar el sonido de ataque
        audioFantasma = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Buscamos al jugador y vamos hacia el rodeando las paredes
        var Player = GameObject.FindWithTag("Player").transform;

        agente.speed = MoveSpeed;
        agente.SetDestination(Player.position);

        // Si estamos muy cerca del jugador, atacamos
        float distancia = Vector3.Distance(transform.position, Player.position);
        if (distancia <= rangoAtaque)
        {
            animator.SetTrigger("ataca");

            if (!atacando && sonidoFantasma != null)
            {
                audioFantasma.PlayOneShot(sonidoFantasma);
                atacando = true;
            }

            // Si te alcanza, has perdido
            if (UIManager.Instance != null)
                UIManager.Instance.MostrarPerdiste();
        }
        else
        {
            atacando = false;
        }
    }

}
