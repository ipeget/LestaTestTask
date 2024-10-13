using UnityEngine;

public class TorsionBlock : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private Space space = Space.World;

    private void Update()
    {
        Rotating();
    }

    private void Rotating()
    {
        transform.Rotate(speed * Time.deltaTime * direction, space);
    }
}
