using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPlayer2Train : MonoBehaviour
{
    public Transform _TrainTransform;
    private bool _IsPlayerOnTrain = false;
    private Vector3 _LocalOffset;
    private Transform _PlayerTransform;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _PlayerTransform = other.transform;
            _LocalOffset = _PlayerTransform.position - _TrainTransform.position;
            _IsPlayerOnTrain = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _IsPlayerOnTrain = false;
            _PlayerTransform = null;
        }
    }

    public void FixedUpdateNetwork()
    {
        if (_IsPlayerOnTrain && _PlayerTransform != null)
        {
            _PlayerTransform.position = _TrainTransform.position + _LocalOffset;

            _PlayerTransform.rotation = _TrainTransform.rotation;
        }
    }
}
