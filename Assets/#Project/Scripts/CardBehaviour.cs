using UnityEngine;
[RequireComponent(typeof(Animator))]

public class CardBehaviour : MonoBehaviour
{
    private const string FACEUP_ANIMATION = "IsFaceUp";
    [SerializeField] private Sprite spriteFaceDown;
    [SerializeField] private Sprite spriteFaceUp;
    #pragma warning disable CS0108 // do not warn about the mask effect on renderer
    [SerializeField] private SpriteRenderer renderer;
    public bool IsFaceUp{get; private set;} // fonction en lecture seule
    #pragma warning restore CS0108 // enable the warning again. 

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



}
