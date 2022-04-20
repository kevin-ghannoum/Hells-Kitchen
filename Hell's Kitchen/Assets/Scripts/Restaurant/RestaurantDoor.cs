using System;
using System.Linq;
using Common;
using Common.Enums;
using Dungeon_Generation;
using Input;
using Photon.Pun;
using PlayerInventory.Cooking;
using Restaurant;
using Restaurant.Enums;
using UnityEngine;
using SceneManager = Common.SceneManager;

public class RestaurantDoor : Interactable
{
    [SerializeField] private float missedOrderPenalty = 10f;
    [SerializeField] private int debtCap = -10;
    
    [SerializeField] private Animator animator;

    private int _numCustomers = 0;

    protected override void Interact()
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

    public override void Update()
    {
        base.Update();
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

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag(Tags.Customer))
        {
            _numCustomers++;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag(Tags.Customer))
        {
            _numCustomers--;
        }
    }

}
