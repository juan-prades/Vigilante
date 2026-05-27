using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 10f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision other)
    {
        // Para hacer dañito... if (other.gameObject.CompareTag("Player")) { ... }

        Destroy(gameObject);
    }
}
