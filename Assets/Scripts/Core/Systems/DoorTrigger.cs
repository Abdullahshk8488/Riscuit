using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [field: SerializeField] public Room room { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            room.RoomTrigger();
        }
    }
}
