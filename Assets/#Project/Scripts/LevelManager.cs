
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField] [Range(2, 24)] private int cardNumber = 6;
    [SerializeField] private int cardsByLine = 4;
    [SerializeField] private CardBehaviour prefab;
    [SerializeField] private Vector2 offset = Vector2.one * 0.2f;
    private List<CardBehaviour> cardBehaviours = new();
    private int firstCardFaceUpId = -1;
    private int secondCardFaceUpId = -1;
    private int cardFocusedId = -1; // attribut
    private CardBehaviour CardFocused => cardFocusedId >= 0 ? cardBehaviours[cardFocusedId] : null; // propriété équiv à la carte focused si elle est pas nulle
    private CardBehaviour FirstCardFaceUp => firstCardFaceUpId >= 0 ? cardBehaviours[firstCardFaceUpId] : null; // renvoie la première carte, si elle existe pas alors : rien.
    private CardBehaviour SecondCardFaceUp => secondCardFaceUpId >= 0 ? cardBehaviours[secondCardFaceUpId] : null; // renvoie la seconde carte, si elle existe pas alors : rien.
    [SerializeField] private Sprite[] spriteFaceUp;
    [SerializeField] private float timeBeforeFaceDown;
    private int successCount = 0;

    public MouseManager MouseManager {get; private set;}
    
    private void Start(){
        if(cardNumber % 2 != 0) 
        {
            cardNumber += 1;
            Debug.LogError("Card number needs to be an even number");
        }

        InstantiateCards();
    }

    public void ConnectMouseManager(MouseManager mouseManager)
    {
        MouseManager = mouseManager;
    }

    private void InstantiateCards()
    {
        // lister les faces possibles
        List<int> facesPoolIndex = new();
        for(int index=0; index < cardNumber/2; index++)
        {
            facesPoolIndex.Add(index);
            facesPoolIndex.Add(index);
        }

        BoxCollider2D collider = prefab.GetComponentInChildren<BoxCollider2D>();
        float width = collider.size.x;
        float height = collider.size.y;



        int lines = cardNumber / cardsByLine;
        if(cardNumber % cardsByLine != 0) lines++;

        float px = (cardsByLine * (width + offset.x) - offset.x) / -2f;
        float py = (lines * (height + offset.y) - offset.y) / -2f;

        Vector2 origin = new(px, py);
        
        for (int x = 0; x < cardsByLine; x++)
        {
            for (int y = 0; y < lines; y++)
            {
                Vector2 position = origin + y * (height + offset.y) * Vector2.up;
                position += x * (width + offset.x) * Vector2.right;
                CardBehaviour cardBehaviour = Instantiate(prefab, position, Quaternion.identity);

                // choper une face
                int rndIndex = Random.Range(0, facesPoolIndex.Count);
                int faceIndex = facesPoolIndex[rndIndex];
                cardBehaviour.SetFace(spriteFaceUp[faceIndex], faceIndex);
                facesPoolIndex.RemoveAt(rndIndex);

                cardBehaviour.ConnectToManager(this, cardBehaviours.Count);

                cardBehaviours.Add(cardBehaviour);
                if(cardBehaviours.Count >= cardNumber) return;
            }
        }
 
   }
   
    public void MouseOverCard(CardBehaviour cardBehaviour)
    {
        if(cardBehaviour == null)
        {
            if(cardFocusedId != -1) // -1 : la seule valeur qui ne correspond a aucun indice pour toujours
            {
                CardFocused.Unfocus();
                cardFocusedId = -1;
            } 
                
        }
        else if(cardBehaviour.Id != cardFocusedId)
        {
            if (CardFocused != null)
            {
                CardFocused.Unfocus();
            }
            cardFocusedId = cardBehaviour.Id;
            cardBehaviour.Focus();
        }
    }

    public void MouseClick()
    {
        if(CardFocused != null) // si une carte est focused
        {
            CardFocused.TurnFaceUP(); // on la retourne
            if (FirstCardFaceUp == null) // si y'a pas de première carte
            {
                firstCardFaceUpId = cardFocusedId; // alors on la carte focus est la première carte
            }
            else
            {
                secondCardFaceUpId = cardFocusedId; // sinon c'est la deuxième carte qu'on stocke dans cardfocused
                StartCoroutine(CheckResult()); // on vérifie le tout
            }
            CardFocused.Unfocus();
            cardFocusedId = -1; // on défocus la carte en question
        }
    }

    private IEnumerator CheckResult()
    {
        MouseManager.enabled = false;

        yield return new WaitForSeconds(timeBeforeFaceDown); // retourne, puis attend une seconde, puis tu continue àpd d'ici. 

        if (FirstCardFaceUp.FaceId != SecondCardFaceUp.FaceId) // si ce ne sont pas les mêmes. 
        {
            FirstCardFaceUp.TurnFaceDOWN();
            SecondCardFaceUp.TurnFaceDOWN();
        }

        else if(FirstCardFaceUp.FaceId == SecondCardFaceUp.FaceId)
        {
            successCount++;
        }

        Debug.Log($"nombre de paires assorties : {successCount}");

        firstCardFaceUpId = -1;
        secondCardFaceUpId = -1;

        MouseManager.enabled = true;
    }
}
