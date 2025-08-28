using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    [field: SerializeField] public BaseIcing BulletPrefab {  get; private set; }
    [field: SerializeField] public int AmmoAmount { get; private set; }
}
