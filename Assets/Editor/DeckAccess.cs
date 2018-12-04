using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections;

class DeckAccess : EditorWindow
{


    Unit selectedUnit;
    GameObject deckCard;
    GameObject handCard;
    GameObject discardCard;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/DeckAccess")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(DeckAccess));
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        selectedUnit = (Unit)EditorGUILayout.ObjectField("Unit ", selectedUnit, typeof(Unit), true);
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginDisabledGroup(TurnManager.instance.activeUnit == selectedUnit);
        if (GUILayout.Button("Select active Unit"))
        {
            selectedUnit = TurnManager.instance.activeUnit;
        }
        EditorGUI.EndDisabledGroup();


        if (selectedUnit != null)
        {
            //Hand
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hand");
            GUILayout.Label("Hand size: " + selectedUnit.cardManager.hand.Count);
            EditorGUILayout.Space();
            if (GUILayout.Button("Clear"))
            {
                Debug.Log("Hand cleared");
                selectedUnit.cardManager.hand = new List<Card>();
                UIupdate();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            handCard = EditorGUILayout.ObjectField("Card ", handCard, typeof(GameObject), false) as GameObject;
            if (handCard != null)
            {
                if (handCard.tag != "Card") handCard = null;
            }
            if (GUILayout.Button("Add"))
            {
                Debug.Log(handCard);
                selectedUnit.cardManager.hand.Add(handCard.GetComponent<Card>());
                UIupdate();
                Debug.Log("Card added");
            }
            if (GUILayout.Button("Remove"))
            {
                selectedUnit.cardManager.hand.Remove(handCard.GetComponent<Card>());
                UIupdate();
                Debug.Log("Card removed");
            }
            EditorGUILayout.EndHorizontal();

            //Deck
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Deck");
            GUILayout.Label("Deck size: " + selectedUnit.cardManager.deck.Count);
            EditorGUILayout.Space();
            if (GUILayout.Button("Clear"))
            {
                Debug.Log("Deck cleared");
                selectedUnit.cardManager.deck = new List<Card>();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            deckCard = EditorGUILayout.ObjectField("Card ", deckCard, typeof(GameObject), false) as GameObject;
            if (deckCard != null)
            {
                if (deckCard.tag != "Card") deckCard = null;
            }
            if (GUILayout.Button("Add"))
            {
                Debug.Log(deckCard);
                selectedUnit.cardManager.deck.Add(deckCard.GetComponent<Card>());
                Debug.Log("Card added");
            }
            if (GUILayout.Button("Remove"))
            {
                selectedUnit.cardManager.deck.Remove(deckCard.GetComponent<Card>());
                Debug.Log("Card removed");
            }
            EditorGUILayout.EndHorizontal();

            //Discard
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Discard");
            GUILayout.Label("Discard size: " + selectedUnit.cardManager.discard.Count);
            EditorGUILayout.Space();
            if (GUILayout.Button("Clear"))
            {
                Debug.Log("Discard cleared");
                selectedUnit.cardManager.discard = new List<Card>();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            discardCard = EditorGUILayout.ObjectField("Card ", discardCard, typeof(GameObject), false) as GameObject;
            if (discardCard != null)
            {
                if (discardCard.tag != "Card") discardCard = null;
            }
            if (GUILayout.Button("Add"))
            {
                Debug.Log(discardCard);
                selectedUnit.cardManager.discard.Add(discardCard.GetComponent<Card>());
                Debug.Log("Card added");
            }
            if (GUILayout.Button("Remove"))
            {
                selectedUnit.cardManager.discard.Remove(discardCard.GetComponent<Card>());
                Debug.Log("Card removed");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            //Stats
            EditorGUILayout.Space();
            GUILayout.Label("Unit Stats");

            //ap
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            selectedUnit.ap = EditorGUILayout.IntField("Action points", selectedUnit.ap);
            if (GUILayout.Button("+5"))
            {
                selectedUnit.ap+=5;
            }
            if (GUILayout.Button("+1"))
            {
                selectedUnit.ap++;
            }
            if (GUILayout.Button("-1"))
            {
                selectedUnit.ap--;
            }
            if (GUILayout.Button("-5"))
            {
                selectedUnit.ap-=5;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            selectedUnit.maxAP = EditorGUILayout.IntField("Max", selectedUnit.maxAP);
            if (GUILayout.Button("Set max action points to current"))
            {
                selectedUnit.maxAP = selectedUnit.ap;
            }
            EditorGUILayout.EndHorizontal();

            //hp
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            selectedUnit.HP = EditorGUILayout.IntField("Health points", selectedUnit.HP);
            if (GUILayout.Button("+5"))
            {
                if (selectedUnit.HP + 5 > selectedUnit.maxHP) selectedUnit.maxHP = selectedUnit.HP + 5;
                selectedUnit.HP += 5;
            }
            if (GUILayout.Button("+1"))
            {
                if (selectedUnit.HP + 1 > selectedUnit.maxHP) selectedUnit.maxHP = selectedUnit.HP + 1;
                selectedUnit.HP++;
            }
            if (GUILayout.Button("-1"))
            {
                selectedUnit.HP++;
            }
            if (GUILayout.Button("-5"))
            {
                selectedUnit.HP -= 5;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            selectedUnit.maxHP = EditorGUILayout.IntField("Max", selectedUnit.maxHP);
            if (GUILayout.Button("Set max health points to current"))
            {
                selectedUnit.maxHP = selectedUnit.HP;
            }
            EditorGUILayout.EndHorizontal();

            //initiative
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            selectedUnit.initiative = EditorGUILayout.IntField("Initiative", selectedUnit.initiative);

            if (GUILayout.Button("+10"))
            {
                selectedUnit.initiative += 10;
            }
            if (GUILayout.Button("+1"))
            {
                selectedUnit.initiative++;
            }
            if (GUILayout.Button("-1"))
            {
                selectedUnit.initiative--;
            }
            if (GUILayout.Button("-10"))
            {
                selectedUnit.initiative -= 10;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Godmode"))
            {
                selectedUnit.maxAP = 999999;
                selectedUnit.maxHP = 999999;
                selectedUnit.HP = 999999;
                selectedUnit.ap = 999999;
            }
            if (GUILayout.Button("Kill"))
            {
                selectedUnit.HP = 0;
            }
        }


        
    }

    void UIupdate()
    {
        if (TurnManager.instance.activeUnit == selectedUnit)
        {
            selectedUnit.cardManager.UpdateHandUI();
        }
    }
}