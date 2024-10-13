using System.Collections;
using UnityEngine;

public class PopupBlock : MonoBehaviour
{
    [SerializeField] private bool selfActivation = false;
    [SerializeField] private float radius;
    [SerializeField] private Transform sensorAnchor;
    [SerializeField] private LayerMask targetMask;
    [Space]
    [SerializeField] private AnimationCurve motion—urve;
    [SerializeField] private float motionTime = 0.5f;
    [SerializeField] private float movementHeight = 1;
    [Space]
    [SerializeField] private float impactCooldown = 5;
    [SerializeField] private float delay = 0;
    [Space]
    [SerializeField] private int damage = 10;
    [SerializeField] private float power;

    private AreaSensor sensor;
    private Vector3 defaultPosition;
    private CountdownTimer timer;

    private void Start()
    {
        sensor = new SphereSensor(radius, sensorAnchor, targetMask);
        defaultPosition = transform.position;
        timer = new CountdownTimer(delay);
        timer.Start();
    }

    private void Update()
    {
        if (!selfActivation)
        {
            Triggering();
        }
        else
        {
            Activate();
        }

        timer.Tick(Time.deltaTime);
    }

    private void Triggering()
    {
        if (sensor.Trigger)
        {
            Activate();
        }
    }

    private void Activate()
    {
        if (!timer.IsRunning)
        {
            timer.Reset(impactCooldown);
            StartCoroutine(ImpactAnimation());
        }
    }

    private IEnumerator ImpactAnimation()
    {
        float t = 0;

        while (t < motionTime)
        {
            t += Time.deltaTime;
            float y = motion—urve.Evaluate(t / motionTime) * movementHeight;
            transform.position = defaultPosition + y * transform.up;

            yield return WaitFor.EndOfFrame;
        }

        Impact();

        yield return WaitFor.Seconds(0.8f);

        StartCoroutine(WasteAnimation());
    }

    private IEnumerator WasteAnimation()
    {
        float t = 0;
        Vector3 position = transform.position;

        while (t < motionTime * 3)
        {
            t += Time.deltaTime;
            float y = -motion—urve.Evaluate(t / (motionTime * 3)) * movementHeight;
            transform.position = position + y * transform.up;

            yield return WaitFor.EndOfFrame;
        }

        transform.position = defaultPosition;
    }

    private void Impact()
    {
        if (sensor.Check() == null) return;

        foreach (var col in sensor.ImpactedObjects)
        {
            if (col != null)
            {
                if (col.TryGetComponent(out IDamagable health))
                    health.TakeDamage(damage);

                if (col.TryGetComponent(out IVisitor visitor))
                    visitor.Visit(this, transform.up.normalized * power);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sensorAnchor.position, radius);
    }
}
