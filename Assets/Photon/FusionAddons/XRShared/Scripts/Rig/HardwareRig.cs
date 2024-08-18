using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Fusion.XR.Shared.Rig
{
    public enum RigPart
    {
        None,
        Headset,
        LeftController,
        RightController,
        Undefined
    }

    // Include all rig parameters in a structure
    public struct RigState
    {
        public Vector3 playAreaPosition;
        public Quaternion playAreaRotation;
        public Vector3 leftHandPosition;
        public Quaternion leftHandRotation;
        public Vector3 rightHandPosition;
        public Quaternion rightHandRotation;
        public Vector3 headsetPosition;
        public Quaternion headsetRotation;
        public HandCommand leftHandCommand;
        public HandCommand rightHandCommand;
    }

    /**
     * 
     * Hardware rig gives access to the various rig parts: head, left hand, right hand, and the play area, represented by the hardware rig itself
     *  
     * Can be moved, either instantanesously, or with a camera fade
     * 
     **/

    public class HardwareRig : MonoBehaviour
    {
        public HardwareHand leftHand;
        public HardwareHand rightHand;
        public HardwareHeadset headset;

        [Serializable]
        public class TeleportEvent : UnityEvent<Vector3, Vector3> { }
        public TeleportEvent onTeleport = new TeleportEvent();

        RigState _rigState = default;
        public Transform trainTransform;
        public Vector3 previousTrainPosition;
        public Vector3 initialRigPosition;

        public virtual RigState RigState
        {
            get
            {
                _rigState.playAreaPosition = transform.position;
                _rigState.playAreaRotation = transform.rotation;
                _rigState.leftHandPosition = leftHand.transform.position;
                _rigState.leftHandRotation = leftHand.transform.rotation;
                _rigState.rightHandPosition = rightHand.transform.position;
                _rigState.rightHandRotation = rightHand.transform.rotation;
                _rigState.headsetPosition = headset.transform.position;
                _rigState.headsetRotation = headset.transform.rotation;
                _rigState.leftHandCommand = leftHand.handCommand;
                _rigState.rightHandCommand = rightHand.handCommand;
                return _rigState;
            }
        }

        #region Locomotion
        // Update the hardware rig rotation. 
        public virtual void Rotate(float angle)
        {
            transform.RotateAround(headset.transform.position, transform.up, angle);
        }

        // Update the hardware rig position. 
        public virtual void Teleport(Vector3 position)
        {
            Vector3 headsetOffet = headset.transform.position - transform.position;
            headsetOffet.y = 0;
            Vector3 previousPosition = transform.position;
            transform.position = position - headsetOffet;
            if (onTeleport != null) onTeleport.Invoke(previousPosition, transform.position);
        }

        // Teleport the rig with a fader
        public virtual IEnumerator FadedTeleport(Vector3 position)
        {
            if (headset.fader) yield return headset.fader.FadeIn();
            Teleport(position);
            if (headset.fader) yield return headset.fader.WaitBlinkDuration();
            if (headset.fader) yield return headset.fader.FadeOut();
        }

        // Rotate the rig with a fader
        public virtual IEnumerator FadedRotate(float angle)
        {
            if (headset.fader) yield return headset.fader.FadeIn();
            Rotate(angle);
            if (headset.fader) yield return headset.fader.WaitBlinkDuration();
            if (headset.fader) yield return headset.fader.FadeOut();
        }
        #endregion

        private void FixedUpdate()
        {
            if (trainTransform != null)
            {
                UpdateRigPosition();
            }
        }

        public void StartTrackingTrain(Transform train)
        {
            trainTransform = train;
            previousTrainPosition = train.position;

            Vector3 headsetOffset = headset.transform.position - transform.position;
            headsetOffset.y = 0;
            initialRigPosition = trainTransform.position - headsetOffset;

            transform.position = initialRigPosition;
        }

        public void StopTrackingTrain()
        {
            trainTransform = null;
        }

        public void UpdateRigPosition()
        {
            if (trainTransform == null) return;

            Vector3 positionDelta = trainTransform.position - previousTrainPosition;

            transform.position += positionDelta;

            previousTrainPosition = trainTransform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("OnTrain"))
            {
                StartTrackingTrain(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("OnTrain"))
            {
                StopTrackingTrain();
            }
        }
    }
}
