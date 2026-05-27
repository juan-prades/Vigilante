using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrullero : MonoBehaviour
{
    [Header("Patrulla")]
    public float patrolSpeed = 2f;
    public float radioDeambular = 10f; // Radio en el que busca un sitio al azar para ir

    [Header("Persecución")]
    public Transform player;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 6f;
    public float loseRange = 9f;

    NavMeshAgent agente; // El componente que mueve al enemigo esquivando paredes

    bool persiguiendo = false;

    void Start()
    {
        // Cogemos el NavMeshAgent que lleva este objeto
        agente = GetComponent<NavMeshAgent>();

        // Buscamos un primer destino al azar
        IrAOtroSitio();
    }

    void Update()
    {
        float distanciaJugador = Vector3.Distance(transform.position, player.position);

        // Si el jugador se acerca, empezamos a perseguir
        if (!persiguiendo && distanciaJugador <= detectionRange)
            persiguiendo = true;

        // Si el jugador se aleja, dejamos de perseguir
        if (persiguiendo && distanciaJugador > loseRange)
            persiguiendo = false;

        if (persiguiendo)
            Perseguir();
        else
            Deambular();
    }

    void Deambular()
    {
        // Velocidad de patrulla
        agente.speed = patrolSpeed;

        // Si ya ha llegado a su destino, busca otro sitio al azar
        if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance + 0.1f)
        {
            IrAOtroSitio();
        }
    }

    void IrAOtroSitio()
    {
        // Elegimos una posicion al azar alrededor del enemigo
        Vector3 posicionAlAzar = transform.position + Random.insideUnitSphere * radioDeambular;

        // Buscamos el punto valido del NavMesh mas cercano y vamos hacia el
        NavMeshHit hit;
        if (NavMesh.SamplePosition(posicionAlAzar, out hit, radioDeambular, NavMesh.AllAreas))
        {
            agente.SetDestination(hit.position);
        }
    }

    void Perseguir()
    {
        // Velocidad de persecución
        agente.speed = chaseSpeed;

        // Vamos hacia el jugador (el NavMesh rodea las paredes)
        agente.SetDestination(player.position);
    }
}
