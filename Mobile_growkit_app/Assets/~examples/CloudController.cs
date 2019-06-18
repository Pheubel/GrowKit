using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField] Vector3 _velocity;
    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("CloudMove", -1, Random.Range(0.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _velocity *Time.deltaTime;
    }
}
