using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class seguir : MonoBehaviour
{
    public Animator animator;
    public float MoveSpeed = 3f;    // Velocidad de persecucion
    public float rangoAtaque = 2f;  // Distancia a la que ataca al jugador

    NavMeshAgent agente; // Mueve a la araña esquivando paredes

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
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

            // Si te alcanza, has perdido
            if (UIManager.Instance != null)
                UIManager.Instance.MostrarPerdiste();
        }
    }
}
