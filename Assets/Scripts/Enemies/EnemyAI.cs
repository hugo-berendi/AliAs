using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State {
        Roaming
    }

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootingRadius = 10f;
    [SerializeField] private float shootingCooldown = 5f;

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Animator animator;
    private Transform playerTransform;
    private float lastShotTime;

    private void Awake() {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
        animator = GetComponent<Animator>();
        lastShotTime = -shootingCooldown; // Allow immediate first shot
    }

    private void Start() {
        StartCoroutine(RoamingRoutine());
        playerTransform = GameObject.Find("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    private void Update()
    {
        if (CanShoot())
        {
            ShootProjectile();
            lastShotTime = Time.time;
        }
    }

    private IEnumerator RoamingRoutine() {
        while (state == State.Roaming) {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingPosition() {
        Vector2 movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        return movement;
    }

    private bool CanShoot()
    {
        if (playerTransform == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool playerInRange = distanceToPlayer <= shootingRadius;
        bool cooledDown = Time.time - lastShotTime >= shootingCooldown;

        return playerInRange && cooledDown;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab != null && playerTransform != null)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            
            if (projectile != null)
            {
                projectile.Initialize(playerTransform.position);
            }
            else
            {
                Debug.LogError("Projectile component not found on instantiated prefab!");
            }
        }
        else
        {
            Debug.LogError("Projectile prefab or player transform not set!");
        }
    }
}
