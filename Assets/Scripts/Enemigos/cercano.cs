using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cercano : MonoBehaviour
{
    public Transform player;
    public float activationRange = 8f; // Se activa
    public float stopRange = 12f;      // Deja de seguir
    public float speed = 3f;

    private bool active = false;

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Se activa al estar cerca
        if (!active && dist <= activationRange)
            active = true;

        // Se desactiva si se aleja demasiado
        if (active && dist > stopRange)
            active = false;

        // Si está activo, persigue
        if (active)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
