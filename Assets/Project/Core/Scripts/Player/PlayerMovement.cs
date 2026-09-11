using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputs _inputs;
    private Vector2 _moveInput;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerAnimation playerAnimation;
    private void Awake()
    {
        _inputs = new PlayerInputs();
    }
    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.Move.performed += OnMove;
        _inputs.Player.Move.canceled += OnMove;
    }
    private void OnDisable()
    {
        
        _inputs.Player.Move.performed -= OnMove;
        _inputs.Player.Move.canceled -= OnMove;
        _inputs.Disable();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    private void Update()
    {
        // Use _moveInput to move the player character
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        characterController.Move(moveDirection * speed * Time.deltaTime); // Move speed of 5 units per second
        if(moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotataion = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotataion, rotationSpeed * Time.deltaTime);//TODO: Fine tune rotation speed
        }
        playerAnimation.SetMovementSpeed(_moveInput.magnitude);
    }
}
