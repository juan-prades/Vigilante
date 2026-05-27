using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dispara : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;
    public float shootRange = 8f;        // distancia para disparar

    [Header("Disparo")]
    public GameObject bulletPrefab;      // tu proyectil
    public Transform shootPoint;         // desde dónde dispara
    public float fireRate = 1f;          // disparos por segundo
    public float bulletSpeed = 10f;      // velocidad del proyectil

    private float fireCooldown = 0f;

    void Update()
    {
        if (player == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango
        if (distancia <= shootRange)
        {
            // Mirar hacia el jugador
            Vector3 direccion = (player.position - transform.position).normalized;
            Quaternion rotacion = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, 10f * Time.deltaTime);

            // Disparar con cooldown
            if (fireCooldown <= 0f)
            {
                Disparar();
                fireCooldown = 1f / fireRate; // tiempo entre disparos
            }
        }

        // bajar cooldown
        fireCooldown -= Time.deltaTime;
    }

    void Disparar()
    {
        GameObject bala = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // mover la bala hacia adelante
        Rigidbody rb = bala.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootPoint.forward * bulletSpeed;
        }
    }
}
