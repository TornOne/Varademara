using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections;

class DeckAccess : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;


    Unit selectedUnit;
    GameObject temp;
    GameObject handCard;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/DeckAccess")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(DeckAccess));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.BeginHorizontal();
        selectedUnit = (Unit)EditorGUILayout.ObjectField("Unit ", selectedUnit, typeof(Unit), true);
        EditorGUILayout.EndHorizontal();

        if (selectedUnit != null)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hand");
            GUILayout.Label("Count: " + selectedUnit.cardManager.hand.Count);

            if (GUILayout.Button("Clear"))
            {
                Debug.Log("Hand cleared");
                selectedUnit.cardManager.hand = new List<Card>();
                selectedUnit.cardManager.UpdateHandUI();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            handCard = EditorGUILayout.ObjectField("Card ", handCard, typeof(GameObject), false) as GameObject;
            if (temp != null && handCard.GetComponent<Card>() != null)
            {
                handCard = temp
            }
            else
            {
                handCard = null;
            }
            //GUILayout.Label("Count: " + selectedUnit.cardManager.hand.Count);
            if (GUILayout.Button("Add"))
            {
                Debug.Log("Card added");
            }
            if (GUILayout.Button("Remove"))
            {
                Debug.Log("Card removed");
            }
            EditorGUILayout.EndHorizontal();
        }



    }
}