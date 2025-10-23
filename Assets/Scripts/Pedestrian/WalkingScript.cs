using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public Animator animator;
    public Rigidbody rb;

    private bool shouldWalk = false;
    private bool hasBeenHit = false;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
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

}
