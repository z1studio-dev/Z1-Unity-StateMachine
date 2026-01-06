using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;
public class ColliderJointDetection : MonoBehaviour
{
    public XRHandJointID jointId;
    public HandsSelector hand;
    public UnityEvent<ColliderJointEvent> OnColliderEvent;

    private void OnTriggerEnter(Collider other){
        if(other.attachedRigidbody == null) return;
        OnColliderEvent.Invoke(new ColliderJointEvent(gameObject, other.ClosestPointOnBounds(transform.position), other.gameObject, other.attachedRigidbody, jointId, TriggerState.Enter, hand));
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody == null) return;
        OnColliderEvent.Invoke(new ColliderJointEvent(gameObject, other.ClosestPointOnBounds(transform.position), other.gameObject, other.attachedRigidbody, jointId, TriggerState.Stay, hand));
    }

    private void OnTriggerExit(Collider other){
        if(other.attachedRigidbody == null) return;
        OnColliderEvent.Invoke(new ColliderJointEvent(gameObject, other.ClosestPointOnBounds(transform.position), other.gameObject, other.attachedRigidbody, jointId, TriggerState.Exit, hand));
    }
}



public enum TriggerState{
    Enter,
    Exit,
    Stay
}
