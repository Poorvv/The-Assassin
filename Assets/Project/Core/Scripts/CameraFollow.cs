using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] Transform target;
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
