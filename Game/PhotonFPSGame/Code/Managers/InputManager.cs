using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool _isCursorLocked;

    [SerializeField] private InGameUI _ui;

    private void Start()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
            _isCursorLocked = true;
        else
            _isCursorLocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Show Pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isCursorLocked = !_isCursorLocked;
            if (_isCursorLocked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
            _ui.ShowEscPanel(!_isCursorLocked);
        }

    }
}
