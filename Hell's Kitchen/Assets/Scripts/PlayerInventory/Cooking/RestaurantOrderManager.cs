// using System;
// using System.Collections.Generic;
// using Common;
// using Enums;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// namespace PlayerInventory.Cooking
// {
//     public  class RestaurantOrderManager : MonoBehaviour
//     {
//         public static RestaurantOrderManager Instance;
//         private static readonly Item[] AvailableRecipes = new[] {Items.Hamburger, Items.Salad, Items.Salad};
//         private Dictionary<Item, int> OrderList => GameStateManager.Instance.OrderList;
//         
//         private void Awake()
//         {
//             if (Instance == null)
//                 Instance = this;
//
//             DontDestroyOnLoad(Instance.gameObject);
//         }
//         public void GetRestaurantOrders(int numOfOrders)
//         {
//             for (int i = 0; i < numOfOrders; i++)
//             {
//                 Item order = GetRandomOrder();
//                 Debug.Log(order);
//                 if (OrderList.ContainsKey(order))
//                     OrderList[order]++;
//                 else
//                     OrderList.Add(order, 1);
//             }
//             
//             DebugPrintList();
//         }
//
//         public void ClearOrderList()
//         {
//             OrderList.Clear();
//         }
//
//         private static Item GetRandomOrder()
//         {
//             int index = Random.Range(0, AvailableRecipes.Length);
//             return AvailableRecipes[index];
//         }
//
//         public void DebugPrintList()
//         {
//             foreach (var item in OrderList)
//             {
//                 UnityEngine.Debug.Log("New item");
//                 UnityEngine.Debug.Log(item.Key);
//                 UnityEngine.Debug.Log(item.Value);
//             }
//         }
//     }
// }