using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInput _playerInput;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _playerInput = GetComponent<PlayerInput>();
    }

    public void SwapActionMap(string actionMap)
    {
        Debug.Log("Action map: " + actionMap);
        _playerInput.SwitchCurrentActionMap(actionMap);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Player player = PlayerManager.Instance.MainPlayer;
        if (player == null)
        {
            return;
        }

        Vector2 direction = context.ReadValue<Vector2>();
        player.Move(direction);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        Player player = PlayerManager.Instance.MainPlayer;
        if (player == null)
        {
            return;
        }

        if (context.canceled)
        {
            player.PlayerGun.SetIsShooting(false);
            return;
        }

        Vector2 direction = context.ReadValue<Vector2>();
        if (direction == Vector2.zero)
        {
            direction = Vector2.up;
        }
        player.PlayerGun.SetShootDirection(direction);
        player.PlayerGun.SetIsShooting(true);
    }
}
