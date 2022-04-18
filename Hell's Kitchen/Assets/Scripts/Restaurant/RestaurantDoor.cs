using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Enums;
using Input;
using PlayerInventory.Cooking;
using Restaurant;
using Restaurant.Enums;
using UnityEngine;
using Random = UnityEngine.Random;
using SceneManager = Common.SceneManager;

public class RestaurantDoor : MonoBehaviour
{
    [SerializeField] private int numberOfOrders = 5;
    [SerializeField] private float missedOrderPenalty = 10f;
    
    [SerializeField] private Animator animator;
    
    private InputManager _input => InputManager.Instance;
    private static IRecipe[] _availableRecipes;

    private int _numCustomers = 0;
    
    private void Awake()
    {
        _availableRecipes = new IRecipe[] {new Recipes.Hamburger(), new Recipes.Salad(), new Recipes.Sushi()};
    }

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        animator.SetBool(RestaurantDoorAnimator.Open, _numCustomers > 0);
    }

    private IRecipe GetRandomOrder()
    {
        int index = Random.Range(0, _availableRecipes.Length);
        return _availableRecipes[index];
    }

    private void ImposeFine()
    {
        foreach (var order in RestaurantManager.Instance.OrderList)
        {
            GameStateManager.Instance.cashMoney -= order.Quantity * missedOrderPenalty;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Customer))
        {
            _numCustomers++;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Customer))
        {
            _numCustomers--;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && _input.interact)
        {
            ImposeFine();
            SceneManager.Instance.LoadDungeonScene();
        }
    }
    
}
