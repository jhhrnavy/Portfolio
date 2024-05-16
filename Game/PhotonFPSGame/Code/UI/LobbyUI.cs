using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PanelType
{
    None = -1,

    Login,
    Lobby,
    Room,

    Count
}

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance;

    [SerializeField] private GameObject[] _panels;
    [SerializeField] private GameObject _pauseMenuPanel;

    [SerializeField] TMP_InputField _nickInput;

    [SerializeField] TextMeshProUGUI _stateText;
    [SerializeField] TextMeshProUGUI _lobbyPlayerCount;
    [SerializeField] TextMeshProUGUI _nickText;

    [SerializeField] TMP_InputField _roomNameInput;
    [SerializeField] Dropdown _playerCountDropdown;
    [SerializeField] GameObject _roomBttnPref;
    List<Button> _roomList = new List<Button>();
    [SerializeField] Transform _roomBttnParent;
    public string NickInput => _nickInput.text;
    public string RoomNameInput => _roomNameInput.text;
    public string StateText { get { return _stateText.text; } set { _stateText.text = value; } }
    public string NickText { get { return _nickText.text; } set { _nickText.text = value; } }

    public Dropdown PlayerCountDropdown { get => _playerCountDropdown;}

    private void Awake()
    {
        if(Instance != null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetPlayerCountDropdown();
        _pauseMenuPanel.SetActive(false);
    }

    public void ShowPanel(PanelType panelType)
    {
        CloseAllPanel();
        _panels[(int)panelType].SetActive(true);
    }

    public void ShowPauseMenu()
    {
        _pauseMenuPanel.SetActive(!_pauseMenuPanel.activeSelf);
    }

    public void CloseAllPanel()
    {
        foreach(GameObject panel in _panels)
            panel.SetActive(false);
    }

    public void CreateRoomButton(string roomName, int maxPlayerCount)
    {
        var roomButton = Instantiate(_roomBttnPref);

        Button button = roomButton.GetComponent<Button>();
        RoomButton buttonData = roomButton.GetComponent<RoomButton>();

        buttonData.SetRoomData(roomName, maxPlayerCount);

        roomButton.transform.SetParent(_roomBttnParent);

        roomButton.transform.localScale = new Vector3(1, 1, 1);

        _roomList.Add(button);
    }

    public void ClearAllRoomButton()
    {
        foreach (var item in _roomList)
        {
            Destroy(item.gameObject);
        }

        _roomList.Clear();
    }

    public void UpdateUIPlayerCountInLobby(int curCountInRooms, int playerCount)
    {
        _lobbyPlayerCount.text = (playerCount - curCountInRooms) + "/" + playerCount;
    }

    public void SetPlayerCountDropdown()
    {
        _playerCountDropdown.options.Clear();

        for (int i = 2; i < 9; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = i.ToString();
            _playerCountDropdown.options.Add(option);
        }

        _playerCountDropdown.value = 0;

        _playerCountDropdown.RefreshShownValue();
    }
}
