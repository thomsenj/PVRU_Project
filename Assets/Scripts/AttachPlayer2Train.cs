using Fusion;
using UnityEngine;

public class AttachPlayer2Train : NetworkBehaviour
{
    public Transform _TrainTransform;
    private bool _IsPlayerOnTrain = false;
    private Vector3 _LocalOffset;
    private NetworkTransform _PlayerNetworkTransform;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _PlayerNetworkTransform = other.GetComponent<NetworkTransform>();
            if (_PlayerNetworkTransform != null)
            {
                _LocalOffset = _PlayerNetworkTransform.transform.position - _TrainTransform.position;
                _IsPlayerOnTrain = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _IsPlayerOnTrain = false;
            _PlayerNetworkTransform = null;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (_IsPlayerOnTrain && _PlayerNetworkTransform != null)
        {
            Vector3 newPosition = _TrainTransform.position + _LocalOffset;
            Quaternion newRotation = _TrainTransform.rotation;

            _PlayerNetworkTransform.transform.position = newPosition;
            _PlayerNetworkTransform.transform.rotation = newRotation;

            // Optionally, sync the position and rotation manually
            // Runner.Move(_PlayerNetworkTransform, newPosition, newRotation);
        }
    }
}
