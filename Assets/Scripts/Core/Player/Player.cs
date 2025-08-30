using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpriteAndHealth
{
    public float startHealthPercentage;
    public string spriteAnimName;
}

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [field: SerializeField] public float moveSpeed { get; private set; }
    [field: SerializeField] public Gun PlayerGun { get; private set; }
    [field: SerializeField] public Dash PlayerDash { get; private set; }
    [field: SerializeField] public static bool IsMoving { get; private set; }
    [SerializeField] private List<SpriteAndHealth> spriteAndHealths;
    public static Vector2 MoveDirection { get; private set; }

    private void Awake()
    {
        PlayerManager.Instance.SetPlayer(this);
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

    private void AnimateBasedOnHP()
    {

    }
}
