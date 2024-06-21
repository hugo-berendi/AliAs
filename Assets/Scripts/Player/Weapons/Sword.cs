using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    /**
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimationSpawnPoint;
    [SerializeField] private bool enableSlashAnimation = false;
    **/

    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float attackTimeout = 34.0f;

    private PlayerControls playerControls;
    private Animator animator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    private GameObject slashAnimation;

    private float currentAttackTimeout = 0.0f;

    private void Awake() {
        animator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    void Start() {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update() {
        if (currentAttackTimeout > 0.0f) {
            currentAttackTimeout = currentAttackTimeout - 1.0f;
        } else {
            MouseFollowWithOffset();
        }
    }

    private void Attack() {
        if (currentAttackTimeout <= 0.0f) {
            animator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
            currentAttackTimeout = attackTimeout;
        }

        /**
        if (enableSlashAnimation) {
            slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnPoint.position, Quaternion.identity);
            slashAnimation.transform.parent = this.transform.parent;
        }
        **/
    }

    public void DoneAttackingAnimationEvent() {
        weaponCollider.gameObject.SetActive(false);
    }

    public void StartAttackingAnimationEvent() {
        weaponCollider.gameObject.SetActive(true);
    }

    public void SwingUpFlipAnimationEvent() {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-100, 0, 0);

        if (playerController.facingLeft) {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimationEvent() {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController.facingLeft) {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset() {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = (Mathf.Atan2(mousePosition.y, mousePosition.x) + Mathf.Rad2Deg)-180;

        if (mousePosition.x < playerScreenPoint.x) {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, 0);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        } else {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, 0);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
