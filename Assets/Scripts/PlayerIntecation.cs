using UnityEngine;
using UnityEngine.Events;

public class PlayerIntecation : MonoBehaviour, IVisitor
{
    [SerializeField] private float radius;
    [SerializeField] private Transform sensorAnchor;
    [SerializeField] private LayerMask interactionMask;
    [Space]
    [SerializeField] private float lesionHeight = -3.5f;

    private Rigidbody rb;
    private Health health;
    private AreaSensor sensor;

    private CountdownTimer pendulumInteractReload;

    [HideInInspector] public UnityEvent GameStarted;
    [HideInInspector] public UnityEvent<float> GameEnded;

    private float startTime;

    private void Start()
    {
        sensor = new SphereSensor(radius, sensorAnchor, interactionMask);
        pendulumInteractReload = new CountdownTimer(1);
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        UpdateTimer();
        DipCheck();
        CheckTrigger();
    }

    private void CheckTrigger()
    {
        if (sensor.Trigger)
        {
            if (sensor.FirstImpacted.CompareTag("StartLine"))
            {
                GameStarted.Invoke();
                startTime = Time.time;
            }
            else if (sensor.FirstImpacted.CompareTag("EndLine"))
            {
                GameEnded.Invoke(Time.time - startTime);
            }
            sensor.FirstImpacted.enabled = false;
        }
    }

    private void UpdateTimer()
    {
        pendulumInteractReload.Tick(Time.deltaTime);
    }

    public void Visit(Pendulum pendulum, Vector3 force)
    {
        if (pendulumInteractReload.IsFinished)
        {
            pendulumInteractReload.Start();
            rb.AddForce(force);
        }
    }

    public void Visit(WindTrap trap, Vector3 velocity)
    {
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    public void Visit(PopupBlock popupBlock, Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void DipCheck()
    {
        if (transform.position.y < lesionHeight)
        {
            health.TakeDamage(100);
        }
    }
}

public interface IVisitor
{
    void Visit(Pendulum pendulum, Vector3 force);
    void Visit(WindTrap trap, Vector3 velocity);

    void Visit(PopupBlock popupBlock, Vector3 force);
}