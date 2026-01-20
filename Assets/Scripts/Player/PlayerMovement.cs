using FishNet.Object;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Inherit from NetworkBehaviour instead of MonoBehaviour
public class PlayerMovement : NetworkBehaviour
{
    public float MoveSpeed = 5f;
    private Vector2 _currentMovementInput;

    public override void OnStartClient()
    {
        base.OnStartClient();
        // This prevents from moving other players' objects.
        if (!IsOwner)
            return;
            GetComponent<PlayerInput>().enabled = true;
            TimeManager.OnTick += OnTick;
    }

    public void OnMove(InputValue value)
    {
        _currentMovementInput = value.Get<Vector2>();
    }

    [ServerRpc]
    private void MoveServerRpc(Vector3 direction)
    {
        transform.position += MoveSpeed * (float)TimeManager.TickDelta * direction;
    }

    private void OnTick()
    {
        Vector3 moveDirection = new Vector3(_currentMovementInput.x, 0f, _currentMovementInput.y);
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();
        MoveServerRpc(moveDirection);

    }
}

