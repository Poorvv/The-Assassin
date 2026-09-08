using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    public void SetMovementSpeed(float speed)
    {
        animator.SetFloat(Speed, speed);
    }
}
