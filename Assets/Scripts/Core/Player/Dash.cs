using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float cooldown;
    [SerializeField] private float speed;
    [SerializeField] private float dashForSeconds;

    public bool IsDashing { get; private set; }
    private bool _onCooldown = false;

    public void StartDash(Vector2 direction)
    {
        if (_onCooldown || IsDashing)
        {
            return;
        }

        IsDashing = true;
        _onCooldown = true;
        rb.linearVelocity = direction * speed;
        StartCoroutine(StopDashing());
        StartCoroutine(Cooldown());
    }

    public void EndDash()
    {
        if (Player.IsMoving)
        {
            return;
        }

        IsDashing = false;
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
