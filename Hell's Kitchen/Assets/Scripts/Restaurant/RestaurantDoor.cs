using System;
using System.Linq;
using Common;
using Common.Enums;
using Input;
using Photon.Pun;
using PlayerInventory.Cooking;
using Restaurant;
using Restaurant.Enums;
using UnityEngine;
using SceneManager = Common.SceneManager;

public class RestaurantDoor : MonoBehaviour
{
    [SerializeField] private float missedOrderPenalty = 10f;
    [SerializeField] private int debtCap = -10;
    
    [SerializeField] private Animator animator;
    
    private InputManager _input => InputManager.Instance;
    private static IRecipe[] _availableRecipes;

    private int _numCustomers = 0;

    private void Awake()
    {
        _availableRecipes = new IRecipe[] {new Recipes.Hamburger(), new Recipes.Salad(), new Recipes.Sushi()};
    }

    private void LeaveRestaurant()
    {
        ImposeFine();
        if (GameStateData.cashMoney < debtCap)
        {
            SceneManager.Instance.LoadGameOverScene();
        }
        else
        {
            SceneManager.Instance.LoadDungeonScene(true);
        }
    }

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        animator.SetBool(RestaurantDoorAnimator.Open, _numCustomers > 0);
    }

    private void ImposeFine()
    {
        var missedOrders = RestaurantManager.Instance.OrderList.Where(o => !o.Served);
        foreach (var order in missedOrders)
        {
            GameStateManager.SetCashMoney(GameStateData.cashMoney - order.Quantity * missedOrderPenalty);
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

    private void OnTriggerStay(Collider other)
    {
        if (InputManager.Actions.Interact.triggered && other.gameObject.CompareTag(Tags.Player))
        {
            var pv = other.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                LeaveRestaurant();
            }
        }
    }
}
