using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BuffUI : MonoBehaviour
{
    public Image Image;
    public TMP_Text DurationText;
    public TMP_Text StrengthText;

    [HideInInspector] public int Strength;
    [HideInInspector] public int Duration;
    [HideInInspector] public int ID;
    [HideInInspector] public Sprite Sprite;
    [HideInInspector] public BuffManager BuffManager;

    void Start()
    {
        DurationText.text = Duration.ToString();
        StrengthText.text = Strength.ToString();
        Image.sprite = Sprite;

        InvokeRepeating("TickSelf", 0f, 1f);
    }

    void TickSelf()
    {
        if (Duration > 1)
        {
            Duration--;
            UpdateText();
        }
        else
        {
            BuffManager.RemoveBuff(ID);
            Destroy(gameObject);
        }
    }

    public void UpdateText()
    {
        DurationText.text = Duration.ToString();
        StrengthText.text = ToRoman(Strength);
    }

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException(nameof(number), "insert value between 1 and 3999");
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new Exception("Impossible state reached");
    }
}
