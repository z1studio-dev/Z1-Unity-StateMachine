using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using System;
using Unity.XR.CoreUtils;

[System.Serializable]
public struct HandsData{
    public Handedness handedness;
    public XRHandTrackingEvents m_HandTrackingEvents;
    private Pose Pose;
    private bool isTracked;

    public Pose pose { get => Pose; set => Pose = value; }
    public bool istracked { get => isTracked; set => isTracked = value; }
}

public class HandsEventManager : MonoBehaviour
{
    private static HandsEventManager instance;
    public List<HandsData> handsDataList;
    private Dictionary<Handedness, HandsData> handsDataDict;
    public Transform xrOrigin;
    public static HandsEventManager GetInstance()
    {
        return instance;
    }

    public static event Action<XRHandJointsUpdatedEventArgs, ScriptableObject, Transform> OnGesturePerformed;
    public static event Action<XRHandJointsUpdatedEventArgs, ScriptableObject, Transform> OnGestureEnded;
    public static event Action<XRHandJointsUpdatedEventArgs, ScriptableObject, Transform> OnGestureStream;

    public static event Action<Handedness, GameObject, Vector3> OnRaycastHit;

    private void Awake()
    {
        instance = this;
        handsDataDict = new Dictionary<Handedness, HandsData>();
        foreach (HandsData handsData in handsDataList)
        {
            handsDataDict.Add(handsData.handedness, handsData);
        }

        foreach (KeyValuePair<Handedness, HandsData> handTrackingEvents in handsDataDict)
        {
            handTrackingEvents.Value.m_HandTrackingEvents.jointsUpdated.AddListener((pose) => UpdateHandsPose(handTrackingEvents.Key, pose));
        }
    }

    void OnDestroy()
    {
    foreach (KeyValuePair<Handedness, HandsData> handTrackingEvents in handsDataDict)
        {
            handTrackingEvents.Value.m_HandTrackingEvents.jointsUpdated.RemoveListener((pose) => UpdateHandsPose(handTrackingEvents.Key, pose));
        }
    }

    public void UpdateHandsPose(Handedness handedness, XRHandJointsUpdatedEventArgs pose)
    {
        if (handsDataDict.ContainsKey(handedness))
        {
            var handsData = handsDataDict[handedness];
            Pose palmPose;
        
            if (!pose.hand.GetJoint(XRHandJointID.Palm).TryGetPose(out palmPose))
            {
                Debug.LogWarning($"Failed to get palm pose for {handedness} hand");
                return;
            }

            Vector3 localHandPos = pose.hand.rootPose.position;
            Quaternion localHandRot = pose.hand.rootPose.rotation;

            Vector3 cameraYOffset = new Vector3(0, xrOrigin.transform.gameObject.GetComponent<XROrigin>().CameraYOffset, 0);
            
            Vector3 worldPosition = xrOrigin.TransformPoint(localHandPos) + cameraYOffset;
            Quaternion worldRotation = xrOrigin.rotation * localHandRot;
        
            handsData.pose = new Pose(worldPosition, worldRotation);
            handsDataDict[handedness] = handsData;
        }
        else
        {
            Pose newPose;
            pose.hand.GetJoint(XRHandJointID.Palm).TryGetPose(out newPose);
            handsDataDict.Add(handedness, new HandsData { handedness = handedness, pose = newPose });
        }        
    }

    public Pose GetHandPose(Handedness handedness)
    {
        if (handsDataDict.ContainsKey(handedness))
        {
            return handsDataDict[handedness].pose;
        }
        else
        {
            Log.LogWarning(this.name, "Hand pose not found for handedness: " + handedness);
            return new Pose();
        }
    }

    public void GesturePerformed(XRHandJointsUpdatedEventArgs eventArgs, ScriptableObject handShapeOrPose, Transform targetTransform)
    {
        OnGesturePerformed?.Invoke(eventArgs, handShapeOrPose, targetTransform);
    }

    public void GestureEnded(XRHandJointsUpdatedEventArgs eventArgs, ScriptableObject handShapeOrPose, Transform targetTransform)
    {
        OnGestureEnded?.Invoke(eventArgs, handShapeOrPose, targetTransform);
    }

    public void GestureStream(XRHandJointsUpdatedEventArgs eventArgs, ScriptableObject handShapeOrPose, Transform targetTransform)
    {
        OnGestureStream?.Invoke(eventArgs, handShapeOrPose, targetTransform);
    }

    public void RaycastHit(Handedness handedness, GameObject hitObject, Vector3 hitPoint)
    {
        OnRaycastHit?.Invoke(handedness, hitObject, hitPoint);
    }
}
