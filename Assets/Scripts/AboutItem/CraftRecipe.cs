using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecipe
{
    public static List<string[]> recipeList = new List<string[]>
    {
        new string[]{"엿", "물이 담긴 호리병", "물엿이 담긴 호리병"},
        new string[]{"정육면체", "구체", "정육구체" }
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
