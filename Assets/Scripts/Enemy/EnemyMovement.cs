using FishNet.Object;
using UnityEngine;

public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private void Update()
    {
        if (!IsServerInitialized)
            return;

        // Bewegung nach -Z (von oben nach unten)
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;
    }
}

