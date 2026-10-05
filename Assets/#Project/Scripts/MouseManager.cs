using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private const string ACTION_MAP = "Game";
    private const string ACTION_MOUSE_POSITION = "Mouse Position";
    private const string ACTION_MOUSE_CLICK = "Mouse Click";
    InputAction mouseClick;
    InputAction mousePosition;
    [SerializeField] private LevelManager levelManager;
    void Awake(){
        mousePosition = inputActions.FindActionMap(ACTION_MAP).FindAction(ACTION_MOUSE_POSITION);
        mouseClick = inputActions.FindActionMap(ACTION_MAP).FindAction(ACTION_MOUSE_CLICK);

        mouseClick.performed += ctx => OnMouseClick(ctx);

        if(levelManager == null)
        {
            levelManager = GetComponent<LevelManager>();
            if(levelManager == null)
            {
                Debug.LogError("Mouse manager needs à Level manager");
            }
        }
    }

    void OnEnable()
    {
        inputActions.FindActionMap(ACTION_MAP).Enable();
    }
    void OnDisable()
    {
        inputActions.FindActionMap(ACTION_MAP).Disable();
    }


    void Update()
    {
        //Debug.Log($"Souris position {mousePosition.ReadValue<Vector2>()}"); // Mouse track
        CheckMouseOver();
    }

    private void CheckMouseOver()
    {
        Vector2 position = mousePosition.ReadValue<Vector2>(); // sotcker position de la souris
        position = Camera.main.ScreenToWorldPoint(position); // stocker la position correspondante en caméra
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.up, 0.01f); // tracer un ray très court de la position vers le haut

        if (hit.collider != null)
        {
            CardBehaviour cardBehaviour = hit.collider.GetComponentInParent<CardBehaviour>();
            levelManager.MouseOverCard(cardBehaviour); // trigger la méthode MouseOverCard avec la carte concernée en paramêtre.
        
        }
    }

    private void OnMouseClick(InputAction.CallbackContext ctx)
    {
        levelManager.MouseClick();
    }

}
