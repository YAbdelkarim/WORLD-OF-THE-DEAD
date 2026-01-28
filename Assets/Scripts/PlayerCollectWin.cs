using UnityEngine;

public class PlayerCollectWin : MonoBehaviour
{

    int masksCollected;
    int totalMasks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalMasks = GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            masksCollected++;
        }
    }

    public bool HasWon()
    {
        return totalMasks == masksCollected;
    }

    public int GetCollectedMasks()
    {
        return masksCollected;
    }

}
