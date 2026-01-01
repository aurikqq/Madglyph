using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class setLanguage : MonoBehaviour
{
    public void changeLanguage(int localeID)
    {
        StartCoroutine(setLocale(localeID));
    }

    IEnumerator setLocale(int localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        StopCoroutine("setLocale");
    }
}
