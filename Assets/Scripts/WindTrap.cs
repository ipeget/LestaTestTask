using System.Collections;
using UnityEngine;

public class WindTrap : MonoBehaviour
{
    [SerializeField] private Vector3 halfExtens;
    [SerializeField] private Transform sensorAnchor;
    [SerializeField] private LayerMask targetMask;
    [Space]
    [SerializeField] private float capacity = 1;
    [Space]
    [SerializeField] private float leadTime = 2;

    private bool impactedSpeedZeroed = true;
    private AreaSensor sensor;

    private void Start()
    {
        sensor = new BoxSensor(halfExtens, sensorAnchor, targetMask);

        StartCoroutine(DirectionalChange());
    }

    private void Update()
    {
        Triggering();
    }

    private void Triggering()
    {
        if (sensor.Trigger)
        {
            Vector3 windVelosity = capacity * Time.deltaTime * (Quaternion.Euler(sensorAnchor.eulerAngles) * Vector3.forward);
            impactedSpeedZeroed = false;

            VisitEachImpacted(windVelosity);
        }
        else if (!impactedSpeedZeroed)
        {
            VisitEachImpacted(Vector3.zero);
            impactedSpeedZeroed = true;
        }
    }

    private void VisitEachImpacted(Vector3 velocity)
    {
        foreach (var col in sensor.ImpactedObjects)
        {
            if (col.TryGetComponent(out IVisitor visitor))
            {
                visitor.Visit(this, velocity);
            }
        }
    }

    private IEnumerator DirectionalChange()
    {
        while (true)
        {
            yield return WaitFor.Seconds(leadTime);
            sensorAnchor.eulerAngles = new Vector3 (0, Random.value * 360, 0);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(sensorAnchor.position, halfExtens * 2);
    }
}
