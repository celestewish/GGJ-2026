using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndPosition : MonoBehaviour
{
    [SerializeField] Image maskSprite;
    [SerializeField] TMP_Text characterName;

    public void SetCharacter(CharacterData data)
    {
        maskSprite.sprite = data.maskSprite;
        characterName.text = data.characterName;
    }
}
