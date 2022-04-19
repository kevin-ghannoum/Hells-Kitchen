using System;
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
    private bool isInTrigger;

    private void Awake()
    {
        _availableRecipes = new IRecipe[] {new Recipes.Hamburger(), new Recipes.Salad(), new Recipes.Sushi()};
        if (!_input)
            return;

        _input.reference.actions["Interact"].performed += LeaveRestaurant;
    }

    private void OnDestroy()
    {
        if (!_input)
            return;
        
        _input.reference.actions["Interact"].performed -= LeaveRestaurant;
    }

    private void LeaveRestaurant(InputAction.CallbackContext obj)
    {
        if (!isInTrigger)
            return;
        
        ImposeFine();
        var player = NetworkHelper.GetLocalPlayerObject();
        SceneManager.Instance.LoadDungeonScene(player.GetComponent<PhotonView>());
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
            isInTrigger = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Customer))
        {
            isInTrigger = false;
            _numCustomers--;
        }
        
        if (other.gameObject.CompareTag(Tags.Player))
        {
            isInTrigger = false;
        }
    }
}
