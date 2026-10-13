using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private InputSystem_Actions inputAction;

    public IInteractable interactable;

    private void Awake()
    {
        inputAction = new InputSystem_Actions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IInteractable>() != null)
        {
            interactable = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactable = null;
        }     
    }

    private void OnEnable()
    {
        inputAction.Enable();

        inputAction.Player.Interact.performed += OnInteract;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        inputAction.Disable();

        inputAction.Player.Interact.performed -= OnInteract;
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        interactable = null;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interactable?.Interact();
        Debug.Log("Tuþa Basýldý");
    }

}
