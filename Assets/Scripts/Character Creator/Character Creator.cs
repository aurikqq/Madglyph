using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField] TMP_Text outputField;
    string currentDigit;
    [SerializeField] float textDelay = 0.1f, specialDelay = 0.4f;
    [SerializeField] string text;
    [SerializeField] bool isTextSkipped;

    public GameObject dialoguePrefab;
    public PlayableDirector playableDirector;
    public int stringIndex;

    GameObject dialogue;
    Transform phrase;
    DialogueObject dialogueSettings;
    IEnumerator TextAnimating;

    void OnEnable()
    {
        dialogue = Instantiate(dialoguePrefab);
        startDialogue();
    }

    void OnDisable() 
    {
        Destroy(dialogue);
    }

    public void startDialogue()
    {
        phrase = dialogue.transform.GetChild(stringIndex);
        dialogueSettings = phrase.gameObject.GetComponent<DialogueObject>();
        textDelay = dialogueSettings.textDelay;
        specialDelay = dialogueSettings.specialDelay;
        text = dialogueSettings.text;
        outputField.GetComponent<TMP_Text>().text = null;
        TextAnimating = textAnimating();
        StartCoroutine(TextAnimating);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log(outputField.GetComponent<TMP_Text>().text);
            Debug.Log(text);

            if (outputField.GetComponent<TMP_Text>().text != "" && outputField.GetComponent<TMP_Text>().text == text)
            {
                if (isTextSkipped)
                {
                    TextAnimating = textAnimating();
                    StartCoroutine(TextAnimating);
                }
                isTextSkipped = false;
                if (dialogueSettings.continueDelay != 0)
                {
                    outputField.GetComponent<TMP_Text>().text = null;
                    StartCoroutine(waitToContinue(dialogueSettings.continueDelay));
                }
                else
                {
                    checkInput();
                }
            }

            else if (outputField.GetComponent<TMP_Text>().text != "" && outputField.GetComponent<TMP_Text>().text != text)
            {
                StopCoroutine(TextAnimating);
                outputField.GetComponent<TMP_Text>().text = text;
                isTextSkipped = true;

                if (dialogueSettings.hasAfterTextEvent)
                {
                    dialogueSettings.afterTextEvent.Invoke();
                }

                if (dialogueSettings.hasAfterTextTimeline)
                {
                    playableDirector.playableAsset = dialogueSettings.afterTextTimeline;
                    playableDirector.Play();
                }
            }
        } 
    }

    void checkInput()
    {
        switch (dialogueSettings.endEvent)
        {
            case DialogueObject.EndEvent.continueDialogue:
                stringIndex += 1;
                startDialogue();
                break;

            case DialogueObject.EndEvent.scriptEvent:
                 dialogueSettings.nextEvent.Invoke();
                break;

            case DialogueObject.EndEvent.startTimeline:
                playableDirector.playableAsset = dialogueSettings.nextTimeline;
                playableDirector.Play();
                break;
         }
    }

    IEnumerator textAnimating()
    {
        foreach (var ABC in text)
        {
            outputField.GetComponent<TMP_Text>().text += ABC;
            currentDigit = ABC.ToString();

            if (!isTextSkipped)
            {
                if (currentDigit == "." || currentDigit == "," || currentDigit == "!" || currentDigit == "?" || currentDigit == "-" || currentDigit == "–")
                {
                    yield return new WaitForSeconds(specialDelay);
                }
                else if (currentDigit == (" "))
                {
                    yield return null;
                }
                else
                {
                    yield return new WaitForSeconds(textDelay);
                }
            }

            //if (ABC == text[text.Length - 1])
            //{
                //isTextAnimatingOver = true;
            //}
        }

        if (dialogueSettings.hasAfterTextEvent)
        {
            dialogueSettings.afterTextEvent.Invoke();
        }

        if (dialogueSettings.hasAfterTextTimeline)
        {
            playableDirector.playableAsset = dialogueSettings.afterTextTimeline;
            playableDirector.Play();
        }
    }

    IEnumerator waitToContinue(float seconds)
    {
        float timer = seconds;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        checkInput();
        yield break;
    }
}