using System.Collections;
using UnityEngine;

public class ActivatedTrap : MonoBehaviour
{
    [SerializeField] private Vector3 halfExtents;
    [SerializeField] private Transform sensorAnchor;
    [SerializeField] private LayerMask targetMask;

    [Space]
    [SerializeField] private int damage;

    [Space]
    [SerializeField] private float totalCooldown = 5;
    [SerializeField] private float impactDelay = 1;

    private Material material;
    private Color defaultColor;
    private Color orange = new Color(1, 0.47f, 0.1725f);
    private CountdownTimer generalTimer;
    private AreaSensor sensor;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        defaultColor = material.color;

        sensor = new BoxSensor(halfExtents, sensorAnchor, targetMask);
        generalTimer = new CountdownTimer(totalCooldown + impactDelay);

    }

    private void Update()
    {
        Triggering();
        generalTimer.Tick(Time.deltaTime);
    }

    private void Triggering()
    {
        if (sensor.Trigger && generalTimer.IsFinished)
        {
            generalTimer.Start();
            StartCoroutine(Impact());
        }
    }

    private IEnumerator Impact()
    {
        material.color = orange;
        yield return WaitFor.Seconds(impactDelay);

        StartCoroutine(MomentaryColorChange(Color.red));

        foreach (var col in sensor.ImpactedObjects)
        {
            if (col != null && col.TryGetComponent(out IDamagable health))
            {
                health.TakeDamage(damage);
            }
        }
    }

    private IEnumerator MomentaryColorChange(Color color)
    {
        material.color = color;
        yield return WaitFor.Seconds(0.2f);
        material.color = defaultColor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(sensorAnchor.position, halfExtents * 2);
    }
}
