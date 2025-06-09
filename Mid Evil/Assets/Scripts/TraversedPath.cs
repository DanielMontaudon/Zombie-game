using UnityEngine;

public class TraversedPath : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private Vector3 boxSize;
    private BoxCollider boxCollider;
    private bool traversed = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.size = boxSize;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            traversed = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(traversed)
            Gizmos.color = new Color(0f, 0.75f, 1f, 0.8f);
        else
            Gizmos.color = new Color(1f, 0f, 0f, 0.8f);
        Gizmos.DrawCube(gameObject.transform.position, boxSize);
    }
}
