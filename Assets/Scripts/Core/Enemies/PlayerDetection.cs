using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private Enemy_Controller enemyController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyController.OnTriggerEnter2D(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyController.OnTriggerExit2D(collision);
        }
    }
}
