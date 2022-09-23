using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private AudioClip crashSound, landingSound;
    [SerializeField] private ParticleSystem crashParticles, successParticles;
    private AudioSource _audioSource;

    private bool _isTransitioning;
    private bool _isCollisionDisabled;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ProcessDebugKeys();
    }

    void ProcessDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            _isTransitioning = true;
            _audioSource.Stop();
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Collision toggled!");
            _isCollisionDisabled = !_isCollisionDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTransitioning || _isCollisionDisabled) return;
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("this is friendly!");
                break;
            case "Finish":
                LoadNextLevelAfter(1f);
                break;
            default:
                StartCrashSequence(1f);
                break;
        }
    }

    private void LoadNextLevelAfter(float time)
    {
        TransitionSequences(successParticles, landingSound);
        Invoke(nameof(LoadNextLevel), time);
    }

    private void TransitionSequences(ParticleSystem particles, AudioClip clip)
    {
        _isTransitioning = true;
        particles.Play();
        _audioSource.Stop();
        _audioSource.PlayOneShot(clip);
        GetComponent<Rocket>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("You blew up!");
    }

    private void StartCrashSequence(float time)
    {
        TransitionSequences(crashParticles, crashSound);
        Invoke(nameof(ReloadLevel), time);
    }
}
