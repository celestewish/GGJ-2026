using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaskSelection : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
    [SerializeField] Image maskImage;
    [SerializeField] Image selectionImage;
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] TMP_Text playerNumberText;

    bool selected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectionImage.enabled = false;
        maskImage.sprite = characterData.maskSprite;
        characterNameText.text = characterData.characterName;
    }

    public void MaskSelected(int playerNum)
    {
        selected = true;
        selectionImage.enabled = true;
        playerNumberText.text = (playerNum + 1).ToString();
    }
    public void MaskUnselected()
    {
        selected = false;
        selectionImage.enabled = false;
        playerNumberText.text = "";
    }

    public CharacterData GetCharacterData()
    {
        return characterData;
    }
}
