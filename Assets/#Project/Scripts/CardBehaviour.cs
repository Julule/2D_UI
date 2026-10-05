using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Animator))]

public class CardBehaviour : MonoBehaviour
{
    private const string FACEUP_ANIMATION = "IsFaceUp";
    private const string MOUSE_OVER_ANIMATION = "IsFocused";
    [SerializeField] private Sprite spriteFaceDown;
    [SerializeField] private Sprite spriteFaceUp;
    #pragma warning disable CS0108 // do not warn about the mask effect on renderer
    [SerializeField] private SpriteRenderer renderer;
    public bool IsFaceUp{get; private set;} = false; // fonction en lecture seule
    public bool IsFocused{get; private set;} = false;
    #pragma warning restore CS0108 // enable the warning again. 

    [SerializeField] private LevelManager manager; // ??
     public int Id {get; private set;} // ??


    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();

        if (renderer == null)
        {
            renderer = GetComponentInChildren<SpriteRenderer>(); // AUSSI sur les enfants
            if (renderer == null) {
                Debug.LogError("Card Behaviour needs a SpriteRenderer to work");
            }

        }

        // ici après on lui set facedown par défaut, si pas précisé dans l'inspect. 
        if(spriteFaceDown == null) // si pas spéficié
        {
            spriteFaceDown = renderer.sprite;
        }
        else
        {
            renderer.sprite = spriteFaceDown;
        }
    }

    void Update()
    {
        TurnFaceUP();
        TurnFaceDOWN();
    }

    public void Turn(bool isFaceUp)
    {
        animator.SetBool(FACEUP_ANIMATION, isFaceUp); // change le paramêtre de l'animateur en true 
        IsFaceUp = isFaceUp;
    }

    private void SetFocus(bool isFocused)
    {
        IsFocused = isFocused;
        animator.SetBool(MOUSE_OVER_ANIMATION, isFocused);
    }

    public void Focus() => SetFocus (true);

    public void Unfocus() => SetFocus (false);


    public void TurnFaceUP() => Turn(true);
    public void TurnFaceDOWN() => Turn(false);
    // {
    //     animator.SetBool(FACEUP_ANIMATION, false); // change le paramêtre de l'animateur en false
    //     IsFaceUp = false;
    // }

    private void FaceUp()
    {
        renderer.sprite = spriteFaceUp;
    }

    private void FaceDown()
    {
        renderer.sprite = spriteFaceDown;

    }

    public void ConnectToManager(LevelManager manager, int id)
    {
        this.manager = manager;
        Id = id;
    }

}
