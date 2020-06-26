using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GUIGameConfigurationCanvasController : MonoBehaviour
{
    public Dropdown dropdownNumberOfPlayers;
    public Dropdown dropdownGridType;
    public Dropdown dropdownNumberOfSameFigures;

    public InputField uid1InputField;
    public InputField uid2InputField;

    List<string> numberOfPlayers;
    List<string> gridTypes;
    List<string> numberOfSameFigures;

    public Text gameNameText;
    public Text numberOfPlayersText;
    public Text gridTypeText;
    public Text numberOfSameFiguresText;
    public Text userId1;
    public Text userId2;

    public Button startGameButton;
    public Button closeGameButton;
    public Button soundButton;
    public Sprite soundEnabled, soundDisabled;

    public void SetNumberOfPlayers(int index)
    {
        var numberOfPlayersTemp = Convert.ToInt32(numberOfPlayers[index]);
        GameController.Instance.NumberOfPlayers = numberOfPlayersTemp;
    }

    public void SetGridSize(int index)
    {
        switch (index)
        {
            case 0:
                GameController.Instance.Rows = 4;
                GameController.Instance.Columns = 4;
                break;
            case 1:
                GameController.Instance.Rows = 6;
                GameController.Instance.Columns = 6;
                break;
            case 2:
                GameController.Instance.Rows = 8;
                GameController.Instance.Columns = 8;
                break;
            case 3:
                GameController.Instance.Rows = 10;
                GameController.Instance.Columns = 10;
                break;
        }
    }

    public void SetNumberOfSameFigures(int index)
    {
        var numberOfSameFiguresTemp = Convert.ToInt32(numberOfSameFigures[index]);
        GameController.Instance.NumberOfCopies = numberOfSameFiguresTemp;
    }

    private void Start()
    {
        PopulateDropDowns();
        DropdownValueChanged();
        //checks if the user is already contained in the DB
        var se = new InputField.SubmitEvent();
        se.AddListener(CheckExistingUID);
        uid1InputField.onEndEdit = se;
        uid2InputField.onEndEdit = se;
        soundButton.GetComponent<Image>().sprite = AudioController.SoundEnabled() ? soundDisabled : soundEnabled;
        dropdownNumberOfPlayers.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged();
        });
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged()
    {
        //0 means 1 player
        int players = dropdownNumberOfPlayers.value;
        switch (players)
        {
            case 0:
                uid2InputField.gameObject.SetActive(false);
                uid1InputField.transform.position = Camera.main.WorldToScreenPoint(new Vector3(0, 86, 0));
                userId2.gameObject.SetActive(false);
                break;
            case 1:
                uid2InputField.gameObject.SetActive(true);
                uid1InputField.transform.position = Camera.main.WorldToScreenPoint(new Vector3(-336, 86, 0));
                userId2.gameObject.SetActive(true);
                break;
        }
    }

    void CheckExistingUID(string uid)
    {
        var gameData = GameController.Instance.GameIOController.GameDataList;
    }

    void PopulateDropDowns()
    {
        numberOfPlayers = new List<string>()
        {
            "1","2"
        };
        dropdownNumberOfPlayers.AddOptions(numberOfPlayers);

        gridTypes = new List<string>()
        {
            "4x4","6x6","8x8","10x10"
        };
        dropdownGridType.AddOptions(gridTypes);

        numberOfSameFigures = new List<string>()
        {
            "2","4"
        };
        dropdownNumberOfSameFigures.AddOptions(numberOfSameFigures);
    }

    public void StartRasterScene(int sceneIndex)
    {
        SetGridSize(dropdownGridType.value);
        SetNumberOfPlayers(dropdownNumberOfPlayers.value);
        SetNumberOfSameFigures(dropdownNumberOfSameFigures.value);
        GameController.Instance.GameIOController.InitializeCurrentGameDataLists();
        SceneManager.LoadSceneAsync(sceneIndex);
        SetUserIds(uid1InputField, uid2InputField);
    }

    private void SetUserIds(InputField uid1InputField, InputField uid2InputField)
    {
        GameController.Instance.Uid1 = uid1InputField.text == "" ? "unbekannt" : uid1InputField.text;
        GameController.Instance.Uid2 = GameController.Instance.NumberOfPlayers == 1 ? "" : uid2InputField.text == "" ? "unbekannt" : uid2InputField.text;
    }

    public void SetGameSound()
    {
        var isMuted = AudioController.SetSound();
        if (isMuted)
        {
            soundButton.GetComponent<Image>().sprite = soundDisabled;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = soundEnabled;
        }

        // GameController.Instance.SoundIsActive = !GameController.Instance.SoundIsActive;
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
