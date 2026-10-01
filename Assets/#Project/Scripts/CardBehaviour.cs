using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    [SerializeField] private Sprite spriteFaceDown;
    [SerializeField] private Sprite spriteFaceUp;
    #pragma warning disable CS0108 // do not warn about the mask effect on renderer
    [SerializeField] private SpriteRenderer renderer;
    #pragma warning restore CS0108 // enable the warning again. 
    void Start()
    {

        if (renderer == null)
        {
            renderer = GetComponentInChildren<SpriteRenderer>(); // AUSSI sur les enfants
            if (renderer == null) {
                Debug.LogError("Card Behaviour needs a SpriteRenderer to work");
            }

        }
        // ici après on lui set facedown par défaut, si pas précisé dans l'inspect. 
        if(spriteFaceDown == null)
        {
            spriteFaceDown = renderer.sprite;
        }
        else
        {
            renderer.sprite = spriteFaceDown;
        }

    }



    public void FaceUp()
    {
        renderer.sprite = spriteFaceUp;
    }

}
