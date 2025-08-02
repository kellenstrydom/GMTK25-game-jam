using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public float detectionRadius = 6;
    private Transform player;
    private bool isTargeted = false;
    public GameObject indecator;
    
    [SerializeField] private string neededObjectName;

    public UnityEvent onTake;


    public string objName;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player has the tag 'Player'.");
        }
    }

    void Update()
    {
        if (player == null) return;
        if (neededObjectName != "")
        {
            if (neededObjectName != player.GetComponent<Hold>().objectName) return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius && !isTargeted)
        {
            // Player came into range
            player.GetComponent<Interact>()?.SetTarget(this);
            isTargeted = true;
        }
        else if (distance > detectionRadius && isTargeted)
        {
            // Player left the range
            player.GetComponent<Interact>()?.ClearTarget();
            isTargeted = false;
        }
        
        indecator.SetActive(isTargeted);
        
    }

    public void InteractWith(Interact interact)
    {
        Debug.Log("Interact with: " + gameObject.name);
        if (neededObjectName != "")
        {
            interact.GetComponent<Hold>().Drop();
            onTake.Invoke();
            return;
        }
        interact.GetComponent<Hold>().PickUp(objName,GetComponent<SpriteRenderer>().sprite);
        Destroy(gameObject);
    }

}
