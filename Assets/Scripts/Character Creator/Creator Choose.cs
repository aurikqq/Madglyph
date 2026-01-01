using UnityEngine;
using UnityEngine.UI;

public class CreatorChoose : MonoBehaviour
{
    [SerializeField] CreatorChoose[] otherClothes;
    [SerializeField] Material outlineMaterial;
    [SerializeField] GameObject clothesToChange;
    [SerializeField] Color defaultColor, disabledColor;

    public void choose()
    {
        foreach (var cloth in otherClothes)
        {
            if (cloth.gameObject != this.gameObject)
            {
                cloth.gameObject.GetComponent<Image>().material = null;
                cloth.gameObject.GetComponent<Image>().color = disabledColor;
            }
            else
            {
                cloth.gameObject.GetComponent<Image>().material = outlineMaterial;
                cloth.gameObject.GetComponent<Image>().color = defaultColor;
            }
        }

        clothesToChange.GetComponent<Image>().sprite = this.gameObject.GetComponent<Image>().sprite;
    }
}
