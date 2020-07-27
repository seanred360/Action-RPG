using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singleton

    public static InputManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public bool inputEnabled = true;

    public void TogglePlayerInput(byte toggle)
    {
        inputEnabled = Convert.ToBoolean(toggle);
    }
}
