using UnityEngine;
using UnityEngine.UI;

public class SpecialBarManager : MonoBehaviour
{
    public Slider Bar;
    public void SetSpecialBar(float amount)
    {
        Bar.value = amount;
    }
}
