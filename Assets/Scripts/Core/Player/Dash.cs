using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Player player;
    [SerializeField] private float cooldown;
    [SerializeField] private float speed;
    [SerializeField] private float dashForSeconds;

    public bool IsDashing { get; private set; }
    private bool _onCooldown = false;

    public void StartDash()
    {
        if (_onCooldown || IsDashing)
        {
            return;
        }

        IsDashing = true;
        _onCooldown = true;

        Vector2 direction = Player.MoveDirection;
        rb.linearVelocity = direction * speed;
        StartCoroutine(StopDashing());
        StartCoroutine(Cooldown());
    }

    public void EndDash()
    {
        IsDashing = false;
        
        if (Player.IsMoving)
        {
            rb.linearVelocity = Player.MoveDirection * player.moveSpeed;
            return;
        }
        rb.linearVelocity = Vector3.zero;
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashForSeconds);
        EndDash();
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        _onCooldown = false;
    }
}
