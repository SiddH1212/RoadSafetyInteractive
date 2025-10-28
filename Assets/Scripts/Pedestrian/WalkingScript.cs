using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public Animator animator;
    public Rigidbody rb;

    private bool shouldWalk = false;
    private bool hasBeenHit = false;
    private Vector3 startPosition;
    private Quaternion startRotation;
    // public AudioSource crashSound;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (shouldWalk && !hasBeenHit)
        {
            animator.SetBool("isWalking", true);
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
    }

    public void BeginWalking()
    {
        if (hasBeenHit) return;

        Debug.Log("Pedestrian started walking.");
        shouldWalk = true;
    }

    public void StopWalking()
    {
        shouldWalk = false;
        animator.SetBool("isWalking", false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasBeenHit) return;

        if (collision.gameObject.CompareTag("Car"))
        {
            Debug.Log("Pedestrian was hit!");
            // crashSound.Play();
            hasBeenHit = true;
            shouldWalk = false;
            animator.enabled = false;

            rb.isKinematic = false;
            rb.linearDamping = 2f;           // Adds resistance to linear motion
            rb.angularDamping = 1.5f;  // Slows down rotation

            Vector3 forceDir = (transform.position - collision.transform.position).normalized;
            forceDir.y = 0.3f; // Less upward force for realism
            rb.AddForce(forceDir * 500f, ForceMode.Impulse); // Reduced magnitude for controlled fall
        }
    }
    public void ResetPedestrian()
    {
        // Reset transform
        transform.position = startPosition;
        transform.rotation = startRotation;

        // Reset animator and rigidbody
        rb.isKinematic = true;
        // rb.velocity = Vector3.zero;
        // rb.angularVelocity = Vector3.zero;
        animator.enabled = true;
        animator.Rebind(); // Reset animator to default pose
        animator.Update(0f);

        // Reset state flags
        hasBeenHit = false;
        shouldWalk = false;
    }

}
