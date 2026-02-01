using UnityEngine;

[CreateAssetMenu(fileName="New Character", menuName ="Character")]
public class CharacterData : ScriptableObject
{
    public Sprite maskSprite;
    public string characterName;
    public int characterID;
}
