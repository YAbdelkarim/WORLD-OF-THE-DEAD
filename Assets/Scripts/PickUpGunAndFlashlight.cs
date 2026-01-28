using UnityEngine;

public class PickUpGunAndFlashlight : MonoBehaviour
{
    public GameObject pickupText1;   // UI text
    public GameObject pickupText2;
    public Light playerFlashlight;   // Player's flashlight
    public GameObject playerGun;
    public bool gunPickedUp;

    private bool playerInRange = false;

    void Start()
    {
        gunPickedUp = false;
        pickupText1.SetActive(false);
        pickupText2.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpGun();
        }
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && gunPickedUp)
        {
            PickUpFlashlight();
        }
    }

    void PickUpGun()
    {
        playerGun.SetActive(true); 
        pickupText2.SetActive(false);
        gunPickedUp=true;
        Destroy(playerGun);
    }

    void PickUpFlashlight()
    {
        playerFlashlight.enabled = true;
        pickupText1.SetActive(false);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && gunPickedUp)
        {
            playerInRange = true;
            pickupText1.SetActive(true);
        }
        else if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pickupText2.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gunPickedUp)
        {
            playerInRange = false;
            pickupText1.SetActive(false);
        }
        else if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pickupText2.SetActive(false);
        }
    }
}
