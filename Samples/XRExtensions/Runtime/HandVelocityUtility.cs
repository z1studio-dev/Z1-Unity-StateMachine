using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.SubsystemsImplementation;  // for SubsystemManager

/// <summary>
/// HandVelocityUtility automatically locates the active XRHandSubsystem at runtime
/// and lets you query “global” hand velocities (wrist + fingertips only).
/// 
/// Usage (anywhere in your code):
///     Vector3 leftVel  = HandVelocityUtility.GetLeftHandVelocity();
///     Vector3 rightVel = HandVelocityUtility.GetRightHandVelocity();
///     Vector3 bothAvg  = HandVelocityUtility.GetBothHandsVelocity();
/// 
/// If a hand isn’t tracked (or the subsystem isn’t running), you’ll get Vector3.zero.
/// </summary>
public static class HandVelocityUtility
{
    // ——————————————————
    //  Configuration / Joint List
    // ——————————————————
    // We only sample the Wrist and all five fingertips.
    // (Tip joints: IndexTip, MiddleTip, RingTip, LittleTip, and ThumbTip.)
    static readonly XRHandJointID[] _sampleJoints = new XRHandJointID[]
    {
        XRHandJointID.Wrist,
        XRHandJointID.ThumbTip,
        XRHandJointID.IndexTip,
        XRHandJointID.MiddleTip,
        XRHandJointID.RingTip,
        XRHandJointID.LittleTip
    };

    // Cached reference to the running XRHandSubsystem (hand-tracking).
    // If null (or not yet found), we’ll try to locate it on first use.
    static XRHandSubsystem _handSubsystem;

    // ——————————————————
    //  Static constructor: try to grab whatever hand subsystem is available
    // ——————————————————
    static HandVelocityUtility()
    {
        // Attempt to find an active XRHandSubsystem
        // (there should typically be exactly one if you’ve enabled Unity’s XR Hands + OpenXR hand-tracking).
        List<XRHandSubsystem> allHands = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems<XRHandSubsystem>(allHands);

        if (allHands.Count > 0)
        {
            // In most setups, there’s only one active XRHandSubsystem.
            _handSubsystem = allHands[0];
        }
        else
        {
            Debug.LogWarning("[HandVelocityUtility] No XRHandSubsystem instance found. Hand velocities will always be zero.");
        }
    }

    // ——————————————————
    //  Public API:
    // ——————————————————

    /// <summary>
    /// Returns the average world‐space velocity of the left hand, sampling only
    /// Wrist + Fingertips. If the subsystem or left hand isn’t available, returns Vector3.zero.
    /// </summary>
    public static Vector3 GetLeftHandVelocity()
    {
        if (_handSubsystem == null) return Vector3.zero;
        var leftHand = _handSubsystem.leftHand;
        return ComputeAverageVelocity(leftHand);
    }

    /// <summary>
    /// Returns the average world‐space velocity of the right hand, sampling only
    /// Wrist + Fingertips. If the subsystem or right hand isn’t available, returns Vector3.zero.
    /// </summary>
    public static Vector3 GetRightHandVelocity()
    {
        if (_handSubsystem == null) return Vector3.zero;
        var rightHand = _handSubsystem.rightHand;
        return ComputeAverageVelocity(rightHand);
    }

    /// <summary>
    /// Returns the average of GetLeftHandVelocity() and GetRightHandVelocity().
    /// In other words: (left + right) / 2. If neither hand is tracked, returns Vector3.zero.
    /// </summary>
    public static Vector3 GetBothHandsVelocity()
    {
        Vector3 leftVel  = GetLeftHandVelocity();
        Vector3 rightVel = GetRightHandVelocity();

        // If both are zero, just return zero.
        if (leftVel == Vector3.zero && rightVel == Vector3.zero)
            return Vector3.zero;

        // Otherwise average them.
        return (leftVel + rightVel) * 0.5f;
    }

    // ——————————————————
    //  Internal helper: average velocities over the configured joints for one XRHand
    // ——————————————————
    static Vector3 ComputeAverageVelocity(XRHand hand)
    {
        // If the hand isn’t tracking or doesn't exist, bail out.
        if (!hand.isTracked)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;
        int    count = 0;

        // For each joint in our “wrist + fingertips” list:
        foreach (var jointID in _sampleJoints)
        {
            XRHandJoint joint = hand.GetJoint(jointID);
            // Try to read its linear velocity (in world‐space).
            if (joint.TryGetLinearVelocity(out Vector3 vel))
            {
                sum += vel;
                count++;
            }
            
        }

        if (count == 0)
            return Vector3.zero;

        return sum / count;
    }

        /// <summary>
    /// Returns the scalar speed (m/s) of the left hand (wrist + fingertips averaged). 
    /// Internally calls GetLeftHandVelocity().magnitude.
    /// </summary>
    public static float GetLeftHandSpeed()
    {
        return GetLeftHandVelocity().magnitude;
    }

    /// <summary>
    /// Returns the scalar speed (m/s) of the right hand.
    /// </summary>
    public static float GetRightHandSpeed()
    {
        return GetRightHandVelocity().magnitude;
    }

    /// <summary>
    /// Returns the average of both‐hands’ speeds as a float (i.e. ((|L| + |R|)/2) ).
    /// Note: this differs slightly from GetBothHandsVelocity().magnitude if left and right are in different directions.
    /// </summary>
    public static float GetBothHandsSpeed()
    {
        return (GetLeftHandSpeed() + GetRightHandSpeed()) * 0.5f;
    }
}
