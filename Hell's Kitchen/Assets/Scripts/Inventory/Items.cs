﻿using Enums;

public static class Items
{
    // Ingredients
    public static readonly Item Honey = new Item(ItemType.Ingredient, "Honey", 1.0f, new ItemModel());
    public static readonly Item PorkChop = new Item(ItemType.Ingredient, "Pork Chop", 1.0f, new ItemModel());
    public static readonly Item Fish = new Item(ItemType.Ingredient, "Fish", 1.0f, new ItemModel());
    public static readonly Item Mushrooms = new Item(ItemType.Ingredient, "Mushroom", 1.0f, new ItemModel());

    // Recipe results
    public static readonly Item Hamburger = new Item(ItemType.RecipeResult, "Burger", 1.0f, new ItemModel());
    public static readonly Item Salad = new Item(ItemType.RecipeResult, "Salad", 1.0f, new ItemModel());
    public static readonly Item Sushi = new Item(ItemType.RecipeResult, "Sushi", 1.0f, new ItemModel());
}