using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed = 1;
    [SerializeField] private float maxAngle;
    [Space]
    [SerializeField] private float force;
    [SerializeField] private Vector3 impactOffest = new Vector3(0, 0.7f, 0);
    [Space]
    [SerializeField] private Vector3 halfExtents;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private Transform rightAncor;
    [SerializeField] private Transform leftAncor;

    private float time = 0;
    private Dictionary<AreaSensor, Vector3> directionFromSensor;

    private void Start()
    {
        directionFromSensor = new Dictionary<AreaSensor, Vector3>()
        {
            { new BoxSensor(halfExtents, rightAncor, targetMask), transform.forward + impactOffest},
            { new BoxSensor(halfExtents, leftAncor, targetMask), -transform.forward + impactOffest},
        };

        transform.eulerAngles += -90 * direction.normalized;
    }

    private void Update()
    {
        Triggering();
        Rotating();
    }

    private void Triggering()
    {
        foreach (var item in directionFromSensor)
        {
            if (item.Key.Trigger)
            {
                foreach (var impacted in item.Key.ImpactedObjects)
                {
                    if (impacted.TryGetComponent(out IVisitor visitor))
                        visitor.Visit(this, item.Value.normalized * force);
                }
            }
        }
    }

    private void Rotating()
    {
        time += Time.deltaTime;
        transform.Rotate(Mathf.Sin(time * speed) * maxAngle * Time.deltaTime * direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(leftAncor.position, halfExtents * 2);
        Gizmos.DrawWireCube(rightAncor.position, halfExtents * 2);
    }
}
