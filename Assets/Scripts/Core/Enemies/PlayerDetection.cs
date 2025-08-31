using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private Enemy_Controller enemyController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Explosive"))
            {
                enemyController.SwitchState(enemyController.ExplosiveState);
                Destroy(gameObject);
            }
            enemyController.OnTriggerEnter2D(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Explosive"))
            {
                
            }
            enemyController.OnTriggerExit2D(collision);
        }
    }
}
