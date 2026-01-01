using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DialogueObject : MonoBehaviour
{

    [Header("General")]
    [HideInInspector] public string localizationKey;
    [HideInInspector] public string text;
    [HideInInspector] public string characterName;
    [HideInInspector] public Sprite sprite;

    [Header("Customization")]
    [HideInInspector] public Color textColor = new Color(1, 1, 1, 1);
    [HideInInspector] public float textDelay = 0.1f;
    [HideInInspector] public float specialDelay = 0.4f;
    [HideInInspector] public float continueDelay = 0;
    
    [Header("Events")]
    [HideInInspector] public DialogueObject nextDialogue;
    [HideInInspector] public UnityEvent nextEvent;
    [HideInInspector] public bool hasAfterTextEvent;
    [HideInInspector] public UnityEvent afterTextEvent;

    [HideInInspector] public bool hasAfterTextTimeline;
    [HideInInspector] public TimelineAsset afterTextTimeline;
    [HideInInspector] public TimelineAsset nextTimeline;
    [HideInInspector] public EndEvent endEvent;
    public enum EndEvent
    {
        continueDialogue,
        scriptEvent,
        startTimeline
    }

    public void OnValidate()
    {
        UpdateLocalizedString();
    }

    private void UpdateLocalizedString()
    {
        if (!string.IsNullOrEmpty(localizationKey))
        {
            try
            {
                LocalizedString localizedString = new LocalizedString
                {
                    TableReference = localizationKey.Split(", ")[0],
                    TableEntryReference = localizationKey.Split(", ")[1]
                };

                text = localizedString.GetLocalizedString();
            }
            catch (IndexOutOfRangeException)
            {

            }
        }
    }
}



#if UNITY_EDITOR

[CustomEditor(typeof(DialogueObject))]
public class EndEventEditor : Editor
{
    private SerializedProperty nextEventProperty;
    private SerializedProperty afterTextEventProperty;

    public override void OnInspectorGUI()
    {
        DialogueObject script = (DialogueObject)target;

        SerializedObject serializedObject = new SerializedObject(script);
        nextEventProperty = serializedObject.FindProperty("nextEvent");
        afterTextEventProperty = serializedObject.FindProperty("afterTextEvent");

        base.OnInspectorGUI();


        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Text");
        script.text = EditorGUILayout.TextArea(script.text, GUILayout.Height(50));

        EditorGUILayout.LabelField("Localization Key");
        script.localizationKey = EditorGUILayout.TextArea(script.localizationKey, GUILayout.Height(20));

        script.characterName = EditorGUILayout.TextField("Character Name", script.characterName);
        script.sprite = (Sprite)EditorGUILayout.ObjectField("Dialogue Sprite", script.sprite, typeof(Sprite), true);


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Customization", EditorStyles.boldLabel);

        script.textColor = EditorGUILayout.ColorField("Text Color", script.textColor);
        if(script.textColor.a == 0) {
            EditorGUILayout.HelpBox("The color is transparent. Change the alpha.", MessageType.Warning);
        }
        script.textDelay = EditorGUILayout.FloatField("Text Delay", script.textDelay, GUILayout.Width(170));
        script.specialDelay = EditorGUILayout.FloatField("Special Delay", script.specialDelay, GUILayout.Width(170));
        script.continueDelay = EditorGUILayout.FloatField("Continue Delay", script.continueDelay, GUILayout.Width(170));


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);

        script.endEvent = (DialogueObject.EndEvent)EditorGUILayout.EnumPopup("After Dialogue", script.endEvent);

        switch (script.endEvent)
        {
            case DialogueObject.EndEvent.continueDialogue:
                script.nextDialogue = (DialogueObject)EditorGUILayout.ObjectField("Next Dialogue", script.nextDialogue, typeof(DialogueObject), true);
                break;

            case DialogueObject.EndEvent.scriptEvent:
                EditorGUILayout.PropertyField(nextEventProperty, new GUIContent("Next Event"), true);
                break;

            case DialogueObject.EndEvent.startTimeline:
                script.nextTimeline = (TimelineAsset)EditorGUILayout.ObjectField("Next Timeline", script.nextTimeline, typeof(TimelineAsset), true);
                break;
        }

        script.hasAfterTextEvent = EditorGUILayout.Toggle("After Text Event", script.hasAfterTextEvent);
        if (script.hasAfterTextEvent)
        {
            EditorGUILayout.PropertyField(nextEventProperty, new GUIContent("Event"), true);
        }
        script.hasAfterTextTimeline = EditorGUILayout.Toggle("After Text Timeline", script.hasAfterTextTimeline);
        if (script.hasAfterTextTimeline)
        {
            script.afterTextTimeline = (TimelineAsset)EditorGUILayout.ObjectField("Timeline", script.afterTextTimeline, typeof(TimelineAsset), true);
        }

        serializedObject.ApplyModifiedProperties();
        script.OnValidate();
    }
}
#endif