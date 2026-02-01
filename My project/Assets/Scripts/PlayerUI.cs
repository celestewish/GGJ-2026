using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] TMP_Text playerNumText;
    [SerializeField] Image maskSprite;
    [SerializeField] Slider specialBar;
    [SerializeField] Image knockedOutSprite;
    [SerializeField] Image readySpecial;
   
    

    public int playerID = -1;

    public void InitSpecialBar(int playerID, CharacterData data)
    {
        characterNameText.text = data.characterName;
        maskSprite.sprite = data.maskSprite;
        specialBar.value = 0;

        this.playerID = playerID;
        playerNumText.text = (playerID + 1).ToString();

        knockedOutSprite.enabled = false;
    }

    public void SetSpecialBar(float value)
    {
        specialBar.value = value;
    }

    public void Knockout()
    {
        knockedOutSprite.enabled = true;
      

        //can add more knockout visuals here
    }
   

    public void IsReady(float amount, float Max)
    {
        if (amount >= Max)
        {
           readySpecial.enabled = true;
        }
        else
        {
            readySpecial.enabled = false;
            
        }
    }
}
