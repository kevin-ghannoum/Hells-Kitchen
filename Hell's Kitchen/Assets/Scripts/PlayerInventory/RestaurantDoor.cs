using System.Collections.Generic;
using Common;
using Common.Enums;
using Input;
using PlayerInventory.Cooking;
using UnityEngine;
using Random = UnityEngine.Random;
using SceneManager = Common.SceneManager;

public class RestaurantDoor : MonoBehaviour
{
    [SerializeField] private int numberOfOrders = 5;
    [SerializeField] private float missedOrderPenalty = 10f;
    private InputManager _input => InputManager.Instance;

    private static IRecipe[] _availableRecipes;
    private Dictionary<IRecipe, int> OrderList => GameStateManager.Instance.OrderList;

    private void Awake()
    {
        _availableRecipes = new IRecipe[] {new Recipes.Hamburger(), new Recipes.Salad(), new Recipes.Sushi()};
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (_input.interact)
            {
                ImposeFine();
                ClearOrderList();
                GetRestaurantOrders(numberOfOrders);
                SceneManager.Instance.LoadDungeonScene();
            }
        }
    }
    
    public void GetRestaurantOrders(int numOfOrders)
    {
        for (int i = 0; i < numOfOrders; i++)
        {
            IRecipe order = GetRandomOrder();
            if (OrderList.ContainsKey(order))
                OrderList[order]++;
            else
                OrderList.Add(order, 1);
        }
    }
    
    public void ClearOrderList()
    {
        OrderList.Clear();
    }

    private IRecipe GetRandomOrder()
    {
        int index = Random.Range(0, _availableRecipes.Length);
        return _availableRecipes[index];
    }

    private void ImposeFine()
    {
        foreach (var order in OrderList)
        {
            GameStateManager.Instance.cashMoney -= order.Value * missedOrderPenalty;
        }
    }
    
}
