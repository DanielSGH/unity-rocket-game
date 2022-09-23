using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    private Vector3 _startingPosition;
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float period;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) return;
        
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float sinWave = Mathf.Sin(cycles * tau);

        Vector3 offset = movementVector * sinWave;
        transform.position = _startingPosition + offset;
    }
}
