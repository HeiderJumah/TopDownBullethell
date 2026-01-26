using FishNet.Object;
using UnityEngine;

public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        if (!IsServerInitialized)
            return;

        transform.position += Vector3.back * moveSpeed * Time.deltaTime;
    }
}

