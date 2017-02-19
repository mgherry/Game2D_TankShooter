using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TankConfigurationSetupWindow : MonoBehaviour
{
    public TankConfiguration configurationReference = null;

    public Text MoveForwardKey;
    public Text MoveBackwardsKey;
    public Text TurnLeftKey;
    public Text TurnRightKey;
    public Text ShootKey;

    public Image colorCircle;

    public InputField InputFieldReference;

    public string tankName = "PlayerTank";
    
    public enum ButtonKeyName
    {
        NULL,
        Forward,
        Backwards,
        Right,
        Left,
        Shoot,
    }

    public void Update()
    {
        if (configurationReference == null)
        {
            configurationReference = GetComponentInChildren<TankConfiguration>();
            if (configurationReference == null)
            {
                GameObject newGO = new GameObject("TankConfig " + tankName);
            }
        }

        if (MoveForwardKey != null)
            MoveForwardKey.text = GetDisplayForKey(configurationReference.MoveForward);
        if (MoveBackwardsKey != null)
            MoveBackwardsKey.text = GetDisplayForKey(configurationReference.MoveBackwards);
        if (TurnLeftKey != null)
            TurnLeftKey.text = GetDisplayForKey(configurationReference.TurnLeft);
        if (TurnRightKey != null)
            TurnRightKey.text = GetDisplayForKey(configurationReference.TurnRight);

        if (ShootKey != null)
            ShootKey.text = GetDisplayForKey(configurationReference.Shoot);

        if (colorCircle != null)
            colorCircle.color = configurationReference.TankColor;

    }


    #region On GUI Event Receivers

    public void OnButtonClick()
    {
        EventSystem.current.SetSelectedGameObject(null);

    }

    public void OnDeletePressed()
    {
        EventSystem.current.SetSelectedGameObject(null);

    }

    public void OnNameChange()
    {
        tankName = InputFieldReference.text;
    }

    public void OnColorChange()
    {
        TankChoosingWindowManager.Instance.OnSetColorRequest(this);
    }

    public void OnForwardClick()
    {
        TankChoosingWindowManager.Instance.OnSetKeyRequest(this, ButtonKeyName.Forward);
    }
    public void OnBackwardsClick()
    {
        TankChoosingWindowManager.Instance.OnSetKeyRequest(this, ButtonKeyName.Backwards);
    }
    public void OnRightClick()
    {
        TankChoosingWindowManager.Instance.OnSetKeyRequest(this, ButtonKeyName.Right);
    }
    public void OnLeftClick()
    {
        TankChoosingWindowManager.Instance.OnSetKeyRequest(this, ButtonKeyName.Left);
    }

    #endregion

    public void SetNewKey(KeyCode newKeyCode, ButtonKeyName keyType)
    {
        switch (keyType)
        {
            case ButtonKeyName.Forward:
                configurationReference.MoveForward = newKeyCode;
                break;
            case ButtonKeyName.Backwards:
                configurationReference.MoveBackwards = newKeyCode;
                break;
            case ButtonKeyName.Right:
                configurationReference.TurnRight = newKeyCode;
                break;
            case ButtonKeyName.Left:
                configurationReference.TurnLeft = newKeyCode;
                break;
            case ButtonKeyName.Shoot:
                configurationReference.Shoot = newKeyCode;
                break;
        }
    }

    public void SetNewColor(Color newColor)
    {
        configurationReference.TankColor = newColor;
    }

    public string GetDisplayForKey(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Space:
                return "SP";
            case KeyCode.KeypadEnter:
                return "ENT";

            case KeyCode.UpArrow:
                return "UpA";
            case KeyCode.DownArrow:
                return "DA";
            case KeyCode.RightArrow:
                return "RA";
            case KeyCode.LeftArrow:
                return "LA";

            default:
                return keyCode.ToString();
        }
    }

}
