using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class Menu : MonoBehaviour
{
    #if UNITY_ANDROID
    AndroidJavaObject currentActivity;
    #endif
    [SerializeField] GameObject light;
    [SerializeField] TMP_Text versionPlaceholder;
    private string title;

    #if UNITY_STANDALONE
    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern System.IntPtr FindWindow(System.String className, System.String windowName);
    #endif
    void Awake()
    {
        #if UNITY_EDITOR
        string version = PlayerSettings.bundleVersion;
        versionPlaceholder.text = "v" + version;
        #endif
        setSplash();

        #if UNITY_STANDALONE
        var windowPtr = FindWindow(null, "Madglyph");
        SetWindowText(windowPtr, title);
        #endif

        #if UNITY_ANDROID
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        #endif
    }

    public void playTimeline()
    {
        gameObject.GetComponent<PlayableDirector>().Play();
        light.GetComponent<Animator>().SetBool("shouldPulse", false);
    }

    public void Play()
    {
        SceneManager.LoadScene("Character Creator");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Telegram()
    {
        Application.OpenURL("https://t.me/+uwxjxx9Qg_EwOWJi");
    }

    void setSplash()
    {
        LocalizedString localizedString = new LocalizedString
        {
            TableReference = "Splashes",
            TableEntryReference = "splash_" + UnityEngine.Random.Range(0, 6).ToString()
        };

        string splash = localizedString.GetLocalizedString();
        title = "Madglyph – " + splash;
    }

    public void sendToast()
    {
        #if UNITY_ANDROID
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", title);
        AndroidJavaObject toast = toastClass.CallStatic<AndroidJavaObject>("makeText", context, javaString, toastClass.GetStatic<int>("LENGTH_LONG"));
        toast.Call("show");
        #endif
    }
}


