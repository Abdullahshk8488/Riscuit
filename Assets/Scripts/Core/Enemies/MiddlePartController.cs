using UnityEngine;

public class MiddlePartController : MonoBehaviour
{
    [SerializeField] private Enemy_Controller enemyController;
    [SerializeField] private SpriteRenderer sprite;
    private void Update()
    {
        if (!enemyController.OnCooldown)
        {
            sprite.enabled = false;
        }
        else
        {
            sprite.enabled = true;
        }
    }
}
