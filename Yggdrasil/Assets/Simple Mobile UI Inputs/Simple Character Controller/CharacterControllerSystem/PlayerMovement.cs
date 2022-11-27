using UI_InputSystem.Base;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private GameObject controllerPlayer;

    private void FixedUpdate()
    {
        MovePlayer();
    }
    
    private void MovePlayer()
    {
        if (!playerTransform) return;
        // GameObject NewPosRot;
        // NewPosRot = PlayerMovementDirection();
        // controllerPlayer.Move(PlayerMovementDirection());
        Vector3 Vec3NewRotate = new Vector3(0f, 0f, PlayerMovementDirection().z);
       controllerPlayer.transform.rotation = Quaternion.LookRotation(Vector3.forward, PlayerMovementDirection());
        // controllerPlayer.transform.Rotate(Vec3NewRotate);
    }

    private Vector3 PlayerMovementDirection()
    {   
        var baseDirection = Vector3.left * (-UIInputSystem.ME.GetAxisHorizontal(JoyStickAction.Movement))
                            + Vector3.up * (UIInputSystem.ME.GetAxisVertical(JoyStickAction.Movement));

        // baseDirection *= playerHorizontalSpeed*Time.deltaTime;
        return baseDirection;
    }
}