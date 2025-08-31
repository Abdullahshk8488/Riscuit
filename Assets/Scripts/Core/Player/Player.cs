using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpriteAndHealth
{
    public float startHealthPercentage;
    public string spriteAnimName;
}

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private Rigidbody2D rb;
    [field: SerializeField] public float moveSpeed { get; private set; }
    [field: SerializeField] public Gun PlayerGun { get; private set; }
    [field: SerializeField] public Dash PlayerDash { get; private set; }
    [field: SerializeField] public static bool IsMoving { get; private set; }
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [SerializeField] private List<SpriteAndHealth> spriteAndHealths;
    public static Vector2 MoveDirection { get; private set; }
    [field: SerializeField] public Animator PlayerAnimator { get; private set; }
    [SerializeField] private float playDefeatScreenInSeconds = 0.7f;

    private bool isPlayerDead = false;

    private void Awake()
    {
        PlayerManager.Instance.SetPlayer(this);
        CurrentHealth = MaxHealth;
    }

    public void Move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            MoveDirection = direction;
        }

        if (PlayerDash.IsDashing)
        {
            return;
        }

        rb.linearVelocity = direction * moveSpeed;
    }

    public void SetIsMoving(bool isMoving)
    {
        IsMoving = isMoving;
    }

    public void DamageTaken(float damageAmount)
    {
        if (isPlayerDead)
        {
            return;
        }

        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0.0f)
        {
            PlayerDies();
            return;
        }

        AnimateBasedOnHP();
    }

    [ContextMenu("Animate Ooga Booga")]
    public void AnimateBasedOnHP()
    {
        for (int i = spriteAndHealths.Count - 1; i >= 0; i--)
        {
            if ((CurrentHealth / MaxHealth) <= spriteAndHealths[i].startHealthPercentage)
            {
                PlayerAnimator.Play(spriteAndHealths[i].spriteAnimName);
                break;
            }
        }
    }

    private void PlayerDies()
    {
        isPlayerDead = true;
        PlayerAnimator.Play("Death");
        PlayerManager.Instance.SetPlayer(null);
        rb.linearVelocity = Vector2.zero;

        StartCoroutine(PlayLoseScreen());
    }
    
    private IEnumerator PlayLoseScreen()
    {
        yield return new WaitForSeconds(playDefeatScreenInSeconds);
        SceneController.Instance.LoadLevel("ScreenLose");
    }
}
