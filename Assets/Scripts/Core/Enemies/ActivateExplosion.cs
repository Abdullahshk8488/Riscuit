using UnityEngine;

public class ActivateExplosion : MonoBehaviour
{
    [SerializeField] private Enemy_Controller enemyController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyController.enemyAnimator.SetBool("IsDead", true);
        }
    }
}
