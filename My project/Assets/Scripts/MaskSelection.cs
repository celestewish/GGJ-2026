using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaskSelection : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
    [SerializeField] Image maskImage;
    [SerializeField] Image selectionImage;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text playerNumberText;
    [SerializeField] GameObject[] playerNums;
    [SerializeField] GameObject[] characterNames;

    bool selected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectionImage.enabled = false;
        maskImage.sprite = characterData.maskSprite;
        descriptionText.text = characterData.description;

        for (int i = 0; i < playerNums.Length; i++)
        {
            playerNums[i].SetActive(false);
        }

        for (int i = 0; i < characterNames.Length; i++)
        {
            characterNames[i].SetActive(false);
        }
        characterNames[characterData.characterID].SetActive(true);
    }

    public void MaskSelected(int playerNum)
    {
        selected = true;
        selectionImage.enabled = true;
        //playerNumberText.text = (playerNum + 1).ToString();

        for (int i = 0; i < playerNums.Length; i++)
        {
            playerNums[i].SetActive(false);
        }
        playerNums[playerNum].SetActive(true);
    }
    public void MaskUnselected()
    {
        selected = false;
        selectionImage.enabled = false;
        //playerNumberText.text = "";

        for (int i = 0; i < playerNums.Length; i++)
        {
            playerNums[i].SetActive(false);
        }
    }

    public CharacterData GetCharacterData()
    {
        return characterData;
    }
}
