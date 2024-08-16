using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        if (vrTarget != null && ikTarget != null)
        {
            ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
            ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    [Range(0, 1)]
    public float turnSmoothness = 0.1f;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;


    void Start()
    {
        AssignVRTargets();
        // head.vrTarget = GameObject.Find("Head VR Target")?.transform;
        // leftHand.vrTarget = GameObject.Find("Left Hand VR Target")?.transform;
        // rightHand.vrTarget = GameObject.Find("Right Hand VR Target")?.transform;
    }

    // LateUpdate is used when calculating the camera position
    // LateUpdate is called after all calculations for the player movement are done
    void LateUpdate()
    {
        if (head.vrTarget != null && head.ikTarget != null)
        {
            transform.position = head.ikTarget.position + headBodyPositionOffset;
            float yaw = head.vrTarget.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z), turnSmoothness);
        }

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }

    private void AssignVRTargets()
    {
        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length;

        head.vrTarget = GameObject.Find($"Head VR Target {playerCount}")?.transform;
        leftHand.vrTarget = GameObject.Find($"Left Hand VR Target {playerCount}")?.transform;
        rightHand.vrTarget = GameObject.Find($"Right Hand VR Target {playerCount}")?.transform;

        Debug.Log($"Assigned VR Targets for Player {playerCount}:");
        Debug.Log($"Head: {head.vrTarget?.name}");
        Debug.Log($"Left Hand: {leftHand.vrTarget?.name}");
        Debug.Log($"Right Hand: {rightHand.vrTarget?.name}");
    }
}
