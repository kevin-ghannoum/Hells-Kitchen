using System;
using UnityEngine;

public class OnTriggerEnterCookMeal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("cooking...");
            Cooking script = GetComponent<Cooking>();
            if (script.CookRecipe(new Recipes.Hamburger()))
            {
                print("meal successfully added to inventory!");
            }
            else
            {
                print("insufficient items in inventory");
            }
            Destroy(gameObject);
        }
    }
}