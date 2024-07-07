using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 7f;
    [SerializeField] private float despawnTime = 2f;

    private Vector2 direction;
    private float spawnTime;

    public void Initialize(Vector2 targetPosition)
    {
        direction = (targetPosition - (Vector2)transform.position).normalized;
        spawnTime = Time.time;
    }
    
    private void Update()
    {
        MoveProjectile();
        CheckDespawn();
    }

    private void MoveProjectile()
    {
        transform.Translate(direction * projectileSpeed * Time.deltaTime);
    }

    private void CheckDespawn()
    {
        if (Time.time - spawnTime >= despawnTime)
        {
            Destroy(gameObject);
        }
    }
}
