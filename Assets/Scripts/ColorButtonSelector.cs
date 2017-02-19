using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorButtonSelector : MonoBehaviour {

    public Image ownImage = null;
    public void OnColorSelected()
    {
        if (ownImage == null)
            ownImage = GetComponent<Image>();

        TankChoosingWindowManager.Instance.ReturnColorValue(ownImage.color);
    }
}
