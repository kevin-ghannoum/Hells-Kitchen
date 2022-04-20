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
using UnityEngine.InputSystem;
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

    private void LeaveRestaurant(InputAction.CallbackContext obj)
    {
        ImposeFine();
        _input.reference.actions["Interact"].performed -= LeaveRestaurant;
        SceneManager.Instance.LoadDungeonScene();
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

        if (other.gameObject.CompareTag(Tags.Player))
        {
            var pv = other.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                _input.reference.actions["Interact"].performed += LeaveRestaurant;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Customer))
        {
            _numCustomers--;
        }
        
        if (other.gameObject.CompareTag(Tags.Player))
        {
            var pv = other.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                _input.reference.actions["Interact"].performed -= LeaveRestaurant;
            }
        }
    }
}
