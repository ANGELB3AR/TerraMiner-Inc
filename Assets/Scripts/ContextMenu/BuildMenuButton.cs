using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buildingNameText = null;
    [SerializeField] Image buildingIconImage = null;

    public void SetBuildingNameText(string buildingName)
    {
        buildingNameText.text = buildingName;
    }

    public void SetBuildingIconImage(Sprite icon)
    {
        buildingIconImage.sprite = icon;
    }
}
