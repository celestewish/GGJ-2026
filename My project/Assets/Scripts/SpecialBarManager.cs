using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialBarManager : MonoBehaviour
{
    /// <summary>
    /// Need to have an id that sees what the player has selected. which will tel lmy boolean values what they need to be
    /// they i need my method to through an array four times for assign each player.
    /// </summary>

    //just needs to be toggled when the special reaches max
    public Image P1_GO;
    public Image P2_GO;
    public Image P3_GO;
    public Image P4_GO;

    //Needs to be enabled
    public Image P1_MainImage;
    public Image P2_MainImage;
    public Image P3_MainImage;
    public Image P4_MainImage;

    //Needs to be enabled
    public TextMeshProUGUI P1_Text;
    public TextMeshProUGUI P2_Text;
    public TextMeshProUGUI P3_Text;
    public TextMeshProUGUI P4_Text;

    //needs to be enabled
    public Slider P1_Slider;
    public Slider P2_Slider;
    public Slider P3_Slider;
    public Slider P4_Slider;

    //needs to equal players selection
    public Image musicManMask;
    public Image DrunkMask;
    public Image TricksterMask;
    public Image LawManMask;

    //needs to equal players selection
    public Slider MusicManBar;
    public Slider DrunkBar;
    public Slider TricksterBar;
    public Slider LawManBar;

    bool isMusicMan;
    bool isDrunk;
    bool isTrickster;
    bool isLawMan;

    private void Awake()
    {
        //Text
        P1_Text.name = "P1";
        P2_Text.name = "P2";
        P3_Text.name = "P3";
        P4_Text.name = "P4";
        P1_Text.enabled = true;
        P2_Text.enabled = true;
        P3_Text.enabled = true;
        P4_Text.enabled = true;
        //Image
        P1_MainImage.enabled = true;
        P2_MainImage.enabled = true;
        P3_MainImage.enabled = true;
        P4_MainImage.enabled = true;
        //Slider
        P1_Slider.enabled = true;
        P2_Slider.enabled = true;
        P3_Slider.enabled = true;
        P4_Slider.enabled = true;
        //Go
        P1_GO.enabled = false;
        P2_GO.enabled = false;
        P3_GO.enabled = false;
        P4_GO.enabled = false;
    }

    public void WhatisWhat(int playerID, float amount, float Max)
    {
        switch (playerID)
        {
            case 0:
                //Player one
                if (isMusicMan)
                {
                    P1_Text.name = "MusicMan";
                    P1_MainImage = musicManMask;
                    P1_Slider = MusicManBar;
                    if (amount >= Max)
                    {
                        P1_GO.enabled = true;
                    }
                    else
                    {
                        P1_GO.enabled = false;
                    }

                }
                else if (isDrunk)
                {
                    P1_Text.name = "Drunk";
                    P1_MainImage = DrunkMask;
                    P1_Slider = DrunkBar;
                    if (amount >= Max)
                    {
                        P1_GO.enabled = true;
                    }
                    else
                    {
                        P1_GO.enabled = false;
                    }
                }
                else if (isTrickster)
                {
                    P1_Text.name = "Trickster";
                    P1_MainImage = TricksterMask;
                    P1_Slider = TricksterBar;
                    if (amount >= Max)
                    {
                        P1_GO.enabled = true;
                    }
                    else
                    {
                        P1_GO.enabled = false;
                    }
                }
                else if (isLawMan)
                {
                    P1_Text.name = "LawMan";
                    P1_MainImage = LawManMask;
                    P1_Slider = LawManBar;
                    if (amount >= Max)
                    {
                        P1_GO.enabled = true;
                    }
                    else
                    {
                        P1_GO.enabled = false;
                    }
                }
                break;
            case 1:
                //Player Two
                if (isMusicMan)
                {
                    P2_Text.name = "MusicMan";
                    P2_MainImage = musicManMask;
                    P2_Slider = MusicManBar;
                    if (amount >= Max)
                    {
                        P2_GO.enabled = true;
                    }
                    else
                    {
                        P2_GO.enabled = false;
                    }

                }
                else if (isDrunk)
                {
                    P2_Text.name = "Drunk";
                    P2_MainImage = DrunkMask;
                    P2_Slider = DrunkBar;
                    if (amount >= Max)
                    {
                        P2_GO.enabled = true;
                    }
                    else
                    {
                        P2_GO.enabled = false;
                    }
                }
                else if (isTrickster)
                {
                    P2_Text.name = "Trickster";
                    P2_MainImage = TricksterMask;
                    P2_Slider = TricksterBar;
                    if (amount >= Max)
                    {
                        P2_GO.enabled = true;
                    }
                    else
                    {
                        P2_GO.enabled = false;
                    }
                }
                else if (isLawMan)
                {
                    P2_Text.name = "LawMan";
                    P2_MainImage = LawManMask;
                    P2_Slider = LawManBar;
                    if (amount >= Max)
                    {
                        P2_GO.enabled = true;
                    }
                    else
                    {
                        P2_GO.enabled = false;
                    }
                }
                break;
            case 2:
                //Player Three
                if (isMusicMan)
                {
                    P3_Text.name = "MusicMan";
                    P3_MainImage = musicManMask;
                    P3_Slider = MusicManBar;
                    if (amount >= Max)
                    {
                        P3_GO.enabled = true;
                    }
                    else
                    {
                        P3_GO.enabled = false;
                    }

                }
                else if (isDrunk)
                {
                    P3_Text.name = "Drunk";
                    P3_MainImage = DrunkMask;
                    P3_Slider = DrunkBar;
                    if (amount >= Max)
                    {
                        P3_GO.enabled = true;
                    }
                    else
                    {
                        P3_GO.enabled = false;
                    }
                }
                else if (isTrickster)
                {
                    P3_Text.name = "Trickster";
                    P3_MainImage = TricksterMask;
                    P3_Slider = TricksterBar;
                    if (amount >= Max)
                    {
                        P3_GO.enabled = true;
                    }
                    else
                    {
                        P3_GO.enabled = false;
                    }
                }
                else if (isLawMan)
                {
                    P3_Text.name = "LawMan";
                    P3_MainImage = LawManMask;
                    P3_Slider = LawManBar;
                    if (amount >= Max)
                    {
                        P3_GO.enabled = true;
                    }
                    else
                    {
                        P3_GO.enabled = false;
                    }
                }

                break;
            case 3:
                //Player Four
                if (isMusicMan)
                {
                    P4_Text.name = "MusicMan";
                    P4_MainImage = musicManMask;
                    P4_Slider = MusicManBar;
                    if (amount >= Max)
                    {
                        P4_GO.enabled = true;
                    }
                    else
                    {
                        P4_GO.enabled = false;
                    }

                }
                else if (isDrunk)
                {
                    P4_Text.name = "Drunk";
                    P4_MainImage = DrunkMask;
                    P4_Slider = DrunkBar;
                    if (amount >= Max)
                    {
                        P4_GO.enabled = true;
                    }
                    else
                    {
                        P4_GO.enabled = false;
                    }
                }
                else if (isTrickster)
                {
                    P4_Text.name = "Trickster";
                    P4_MainImage = TricksterMask;
                    P4_Slider = TricksterBar;
                    if (amount >= Max)
                    {
                        P4_GO.enabled = true;
                    }
                    else
                    {
                        P4_GO.enabled = false;
                    }
                }
                else if (isLawMan)
                {
                    P4_Text.name = "LawMan";
                    P4_MainImage = LawManMask;
                    P4_Slider = LawManBar;
                    if (amount >= Max)
                    {
                        P4_GO.enabled = true;
                    }
                    else
                    {
                        P4_GO.enabled = false;
                    }
                }
                break;
            default:
                //Text
                P1_Text.name = "P1";
                P2_Text.name = "P2";
                P3_Text.name = "P3";
                P4_Text.name = "P4";
                P1_Text.enabled = true;
                P2_Text.enabled = true;
                P3_Text.enabled = true;
                P4_Text.enabled = true;
                //Image
                P1_MainImage.enabled = true;
                P2_MainImage.enabled = true;
                P3_MainImage.enabled = true;
                P4_MainImage.enabled = true;
                //Slider
                P1_Slider.enabled = true;
                P2_Slider.enabled = true;
                P3_Slider.enabled = true;
                P4_Slider.enabled = true;
                //Go
                P1_GO.enabled = false;
                P2_GO.enabled = false;
                P3_GO.enabled = false;
                P4_GO.enabled = false;
                break;
        
        } 

           

           

           
           
    }
}
    
    


