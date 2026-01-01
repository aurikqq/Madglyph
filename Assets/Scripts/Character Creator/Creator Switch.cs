using UnityEngine;

public class CreatorSwitch : MonoBehaviour //TODO Make chosen character treats be saved in a file, do a transition to next creation part by pressing Enter or smth on mobile.
{
    [SerializeField] GameObject[] switchingPanels;
    [SerializeField] GameObject[] switchers;
    [SerializeField] GameObject linkedPanel;

    public bool isChosen;

    public void switchPanel()
    {
        if (!isChosen)
        {
            this.transform.parent.gameObject.GetComponent<Animation>().Play("Switcher On");
        }

        foreach (var panel in switchingPanels)
        {
            if (panel != linkedPanel)
            {
                panel.SetActive(false);
            }
            else
            {
                panel.SetActive(true);
                isChosen = true;
            }
        }

        foreach (var switcher in switchers)
        {
            if (switcher != gameObject && switcher.GetComponent<CreatorSwitch>().isChosen)
            {
                switcher.transform.parent.gameObject.GetComponent<Animation>().Play("Switcher Off");
                switcher.GetComponent<CreatorSwitch>().isChosen = false;
            }
        }
    }
}

