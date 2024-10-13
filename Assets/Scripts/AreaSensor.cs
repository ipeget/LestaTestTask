using UnityEngine;

public abstract class AreaSensor
{
    protected readonly Transform anchor;
    protected readonly LayerMask mask;
    protected Vector3 Position => anchor.position;
    protected Quaternion Rotation => anchor.rotation;
    public Collider[] ImpactedObjects { get; protected set; }

    public AreaSensor(Transform anchor, LayerMask mask)
    {
        this.anchor = anchor;
        this.mask = mask;
    }

    public abstract Collider[] Check();

    public Collider FirstImpacted => (ImpactedObjects != null && ImpactedObjects.Length > 0) ? ImpactedObjects[0] : null;

    public bool Trigger => Check().Length > 0;
}

public class SphereSensor : AreaSensor
{
    private readonly float radius;

    public SphereSensor(float radius, Transform anchor, LayerMask mask) : base(anchor, mask)
    {
        this.radius = radius;
    }

    public override Collider[] Check()
    {
        ImpactedObjects = Physics.OverlapSphere(Position, radius, mask);
        return ImpactedObjects;
    }
}

public class BoxSensor : AreaSensor
{
    private readonly Vector3 halfExtents;

    public BoxSensor(Vector3 halfExtents, Transform anchor, LayerMask mask) : base(anchor, mask)
    {
        this.halfExtents = halfExtents;
    }

    public override Collider[] Check()
    {
        ImpactedObjects = Physics.OverlapBox(Position, halfExtents, Rotation, mask);
        return ImpactedObjects;
    }
}
