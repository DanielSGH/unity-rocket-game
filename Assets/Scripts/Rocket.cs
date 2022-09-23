using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float mainThrust = 100f, mainRotate = 10f;
    public GameObject launchPad;
    private Rigidbody _rb;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private ParticleSystem leftThrustParticles, rightThrustParticles, mainThrustParticles;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MainThrustHandler();
        }
        else
        {
            _audioSource.Stop();
            mainThrustParticles.Stop();
        }
    }

    private void MainThrustHandler()
    {
        _rb.AddRelativeForce(Vector3.up * (mainThrust * Time.deltaTime));
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(mainEngine);
        }

        if (!mainThrustParticles.isPlaying)
        {
            mainThrustParticles.Play();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!leftThrustParticles.isPlaying) leftThrustParticles.Play();
            ApplyRotation(mainRotate);
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            if (!rightThrustParticles.isPlaying) rightThrustParticles.Play();
            ApplyRotation(-mainRotate);
        }
        else
        {
            leftThrustParticles.Stop();
            rightThrustParticles.Stop();
        }
    }

    void ApplyRotation(float rotation)
    {
        _rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * (rotation * Time.deltaTime));
        _rb.freezeRotation = false;
    }
}
