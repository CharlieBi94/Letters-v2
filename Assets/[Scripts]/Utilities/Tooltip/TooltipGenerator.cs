using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TooltipGenerator { 

    public static string GenerateTooltip(char c)
    {
        if(c == '-')
        {
            return $"Upgrade a inventory slot / letter so that they can be placed between tiles.";
        }
        if(c == '*')
        {
            return $"Gain the ability to move all player added tiles on the field for 15 seconds.";
        }
        string tooltip = $"Swap a ";
        if (LetterUtility.IsVowel(c))
        {
            tooltip += "<b>vowel</b>";
        }
        else
        {
            tooltip += "<b>consonant</b>";
        }
        tooltip += $" in your inventory for the letter {c}.";
        return tooltip;
    }

}
