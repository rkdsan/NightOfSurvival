using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecipe
{
    public static List<string[]> recipeList = new List<string[]>
    {
        new string[]{"¿³", "¹°º´", "²ö²öÀÌ"},
        new string[]{"Á¤À°¸éÃ¼", "±¸Ã¼", "Á¤À°±¸Ã¼" }
    };
    
    public static string GetCraftResult(string name1, string name2)
    {
        foreach (string[] recipe in recipeList)
        {
            if (name1.Equals(recipe[0]))
            {
                if (name2.Equals(recipe[1]))
                {
                    return recipe[2];
                }
            }
            else if (name2.Equals(recipe[0]))
            {
                if (name1.Equals(recipe[1]))
                {
                    return recipe[2];
                }
            }
        }

        return "";
    }


}
