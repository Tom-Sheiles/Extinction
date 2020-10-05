using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject player;

    public GameObject spawnLocation;

    private void Awake()
    {
        var playerObject = Instantiate(player);
        playerObject.transform.position = spawnLocation.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
