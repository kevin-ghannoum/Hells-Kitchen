using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private CharacterController cc;

    [SerializeField]
    private float maxSpeed = 10;
    [SerializeField]
    private float rotationSpeed = 10;
    [SerializeField]
    private float acceleration = 1;

    private float speed = 0;
    
    /**
     * Singleton instance of player script.
     */
    public static Player Instance;
    private Inventory inventory;

    private void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (movement.magnitude != 0)
                transform.rotation = Quaternion.LookRotation(movement);
            animator.SetTrigger("Roll");
            speed = 1;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterArmature|Roll"))
        {
            cc.Move(transform.forward * Time.deltaTime * maxSpeed);

        }
        else
        {
            if (movement.magnitude != 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed * Time.deltaTime);
                if (Input.GetKey(KeyCode.LeftShift))
                    if (speed < 1)
                    {
                        speed += Time.deltaTime * acceleration;
                    }
                    else
                    {
                        speed = 1;
                    }
                else
                {
                    if (speed < 0.25)
                    {
                        speed += Time.deltaTime * acceleration;
                    }
                    else if (speed < 0.26)
                    {
                        speed = 0.25f;
                    }
                    else
                    {
                        speed -= Time.deltaTime * acceleration;
                    }
                }
            }
            else
            {
                if (speed > 0.01)
                {
                    speed -= Time.deltaTime * acceleration;
                    cc.Move(transform.forward * Time.deltaTime * speed * maxSpeed);
                }
                else
                {
                    speed = 0;
                }
            }

            animator.SetFloat("Speed", speed);
            cc.Move(movement * Time.deltaTime * speed * maxSpeed);
        }
    }
    
    public Dictionary<Item, int> GetPlayerInventory()
    {
        return inventory.GetInventory();
    }

    public void AddItemToInventory(Item item, int quantity)
    {
        inventory.AddItemToInventory(item, quantity);
    }

    public void RemoveItemFromInventory(Item item, int quantity)
    {
        inventory.RemoveItemFromInventory(item,quantity);
    }

    #region Unity Events
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    #endregion
}
