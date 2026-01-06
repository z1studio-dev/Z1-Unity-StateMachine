using UnityEngine;
using System;
using UnityEngine.XR.Hands;

public class HandCollisionManager : MonoBehaviour
{
    public static event Action<ColliderJointEvent, Vector3> OnHandCollisionEvent;
    XRHandJointsUpdatedEventArgs leftHandJoints;
    XRHandJointsUpdatedEventArgs rightHandJoints;
    
    public bool useTriggerStay = false;

    void Awake()
    {
        leftHandJoints = new XRHandJointsUpdatedEventArgs();
        rightHandJoints = new XRHandJointsUpdatedEventArgs();
    }


    public void OnColliderJointEvent(ColliderJointEvent colliderJointEvent)
    {
        //We only trigger enter and leave to avoid unnecessary calculations
        switch(colliderJointEvent.triggerState){
            case TriggerState.Enter:
                HandleHandCollision(colliderJointEvent);
                break;
            case TriggerState.Exit:
                HandleHandCollision(colliderJointEvent);
                break;
            case TriggerState.Stay:
                if (useTriggerStay)
                { 
                    HandleHandCollision(colliderJointEvent);
                }
                break;
        }
    }

    void HandleHandCollision(ColliderJointEvent colliderJointEvent)
    {
            Vector3 linearVelocity;
            switch(colliderJointEvent.hand){
                case HandsSelector.Left:
                    leftHandJoints.hand.GetJoint(colliderJointEvent.jointId).TryGetLinearVelocity(out linearVelocity);
                    break;
                case HandsSelector.Right:
                    rightHandJoints.hand.GetJoint(colliderJointEvent.jointId).TryGetLinearVelocity(out linearVelocity);
                    break;
                default:
                    linearVelocity = Vector3.zero;
                    break;
            }

            linearVelocity = Vector3.ClampMagnitude(linearVelocity, 1.0f);
            OnHandCollisionEvent?.Invoke(colliderJointEvent, linearVelocity);
    }

    public void OnLeftJointUpdate(XRHandJointsUpdatedEventArgs xRHandJointsUpdatedEventArgs)
    {
        leftHandJoints = xRHandJointsUpdatedEventArgs;
    }

    public void OnRightJointUpdate(XRHandJointsUpdatedEventArgs xRHandJointsUpdatedEventArgs)
    {
        rightHandJoints = xRHandJointsUpdatedEventArgs;
    }
    

}
