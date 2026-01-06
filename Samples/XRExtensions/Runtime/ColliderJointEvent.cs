using UnityEngine;
using UnityEngine.XR.Hands;
public class ColliderJointEvent
{
    public GameObject colliderjoint { get; private set; }
    public GameObject otherCollider { get; private set; }
    public Vector3 pointOfContact { get; private set; }
    public Rigidbody rigidbody { get; private set; }
    public XRHandJointID jointId { get; private set; }
    public TriggerState triggerState { get; private set; }
    public HandsSelector hand { get; private set; }

    public ColliderJointEvent(GameObject colliderjoint, Vector3 pointOfContact, GameObject otherCollider, Rigidbody rigidbody, XRHandJointID jointId, TriggerState triggerInOut, HandsSelector hand)
    {
        this.colliderjoint = colliderjoint;
        this.pointOfContact = pointOfContact;
        this.otherCollider = otherCollider;
        this.rigidbody = rigidbody;
        this.jointId = jointId;
        this.triggerState = triggerInOut;
        this.hand = hand;
    }
}