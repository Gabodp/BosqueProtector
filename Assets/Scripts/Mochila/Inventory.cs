using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Database database; //Referencia a la base de datos
    [SerializeField]
    private GameObject slotPrefab; //Referencia al prefab del slot
    [SerializeField]
    private Transform InventoryPanel; //Referencia al Panel de Inventario
    [SerializeField]
    private List<SlotInfo> slotInfoList; // Lista con la informacion de todos los slots (inventario)
    [SerializeField]
    private int capacity; //capacidad de mi inventario

    private string jsonString; // Texto en formato json que usaremos para guardar el inventario.

    private void Start()
    {
        slotInfoList = new List<SlotInfo>();
        if (PlayerPrefs.HasKey("inventory"))
        {
            LoadSavedInventory();
        }
        else
        {
            LoadEmptyInventory();
        }
    }

    private void LoadEmptyInventory()
    {
        for (int i = 0; i < capacity; i++)
        {
            GameObject slot = Instantiate<GameObject>(slotPrefab, InventoryPanel);
            Slot newSlot = slot.GetComponent<Slot>();
            newSlot.SetUp(i);
            newSlot.database = database;
            SlotInfo newSlotInfo = newSlot.slotInfo;
            slotInfoList.Add(newSlotInfo);
        }
    }

    private void LoadSavedInventory()
    {
        jsonString = PlayerPrefs.GetString("inventory");
        InventoryWrapper inventoryWrapper = JsonUtility.FromJson<InventoryWrapper>(jsonString);
        this.slotInfoList = inventoryWrapper.slotInfoList;
        for (int i = 0; i < capacity; i++)
        {
            GameObject slot = Instantiate<GameObject>(slotPrefab, InventoryPanel);
            Slot newSlot = slot.GetComponent<Slot>();
            newSlot.SetUp(i);
            newSlot.database = database;
            newSlot.slotInfo = slotInfoList[i];
            newSlot.UpdateUI();
        }


    }

    public SlotInfo FindItemInInventory(int itemId)
    {
        foreach (SlotInfo slotInfo in slotInfoList)
        {
            if (slotInfo.itemId == itemId && !slotInfo.isEmpty)
            {
                return slotInfo;
            }
        }
        return null;
    }

    private SlotInfo FindSuitableSlot(int itemId)
    {
        foreach (SlotInfo slotInfo in slotInfoList)
        {
            if (slotInfo.itemId == itemId && slotInfo.amount < slotInfo.maxAmount && !slotInfo.isEmpty && database.FindItemInDatabase(itemId).isStackable)
            {
                return slotInfo;
            }
        }
        foreach (SlotInfo slotInfo in slotInfoList)
        {
            if (slotInfo.isEmpty)
            {
                //slotInfo.EmptySlot();
                return slotInfo;
            }
        }
        return null;
    }

    private Slot FindSlot(int id)
    {
        return InventoryPanel.GetChild(id).GetComponent<Slot>();
    }

    public void AddItem(int itemId)
    {
        Item item = database.FindItemInDatabase(itemId); //busco en la base de datos
        if (item != null)
        {
            SlotInfo slotInfo = FindSuitableSlot(itemId);
            if (slotInfo != null)
            {
                slotInfo.amount++;
                slotInfo.itemId = itemId;
                slotInfo.isEmpty = false;

                FindSlot(slotInfo.id).UpdateUI();
            }
        }
    }

    public void RemoveItem(int itemId)
    {
        SlotInfo slotInfo = FindItemInInventory(itemId);
        if (slotInfo != null)
        {
            if (slotInfo.amount == 1)
            {
                slotInfo.EmptySlot();
            }
            else
            {
                slotInfo.amount--;
            }
            FindSlot(slotInfo.id).UpdateUI();
        }
    }
    public void RemoveItem(int itemId, SlotInfo slotInfo)
    {
        if (slotInfo != null)
        {
            if (slotInfo.amount == 1)
            {
                slotInfo.EmptySlot();
            }
            else
            {
                slotInfo.amount--;
            }
            FindSlot(slotInfo.id).UpdateUI();
        }
    }

    public void SaveInventory()
    {
        InventoryWrapper inventoryWrapper = new InventoryWrapper();
        inventoryWrapper.slotInfoList = this.slotInfoList;
        jsonString = JsonUtility.ToJson(inventoryWrapper);
        PlayerPrefs.SetString("inventory", jsonString);
    }


    public void SwapSlots(int id_o, int id_d, Transform image_o, Transform image_d)
    {
        //intercambio las imagenes
        image_o.SetParent(InventoryPanel.GetChild(id_d));
        image_d.SetParent(InventoryPanel.GetChild(id_o));
        image_o.localPosition = Vector3.zero;
        image_d.localPosition = Vector3.zero;

        if (id_o != id_d)
        {
            SlotInfo origin = slotInfoList[id_o];
            SlotInfo destination = slotInfoList[id_d];

            //intercambio en el inventario
            slotInfoList[id_o] = destination;
            slotInfoList[id_o].id = id_o;
            slotInfoList[id_d] = origin;
            slotInfoList[id_d].id = id_d;

            //intercambio en los slots(items) basado en los cambios en el inventario

            Slot originSlot = InventoryPanel.GetChild(id_o).GetComponent<Slot>();
            originSlot.slotInfo = slotInfoList[id_o];
            Slot destinationSlot = InventoryPanel.GetChild(id_d).GetComponent<Slot>();
            destinationSlot.slotInfo = slotInfoList[id_d];

            originSlot.itemImage = image_d.GetComponent<Image>();
            destinationSlot.itemImage = image_o.GetComponent<Image>();

            originSlot.amountText = originSlot.itemImage.transform.GetChild(0).GetComponent<Text>();
            destinationSlot.amountText = destinationSlot.itemImage.transform.GetChild(0).GetComponent<Text>();


        }

    }


    private struct InventoryWrapper
    {
        public List<SlotInfo> slotInfoList;
    }
    //funciones para hacer pruebas
    [ContextMenu("Test Add - itemId = 1")]
    public void TestAdd()
    {
        AddItem(1);
    }
    [ContextMenu("Test Add F - itemId = ?")]
    public void TestAddF(int id)
    {
        AddItem(id);
    }
    [ContextMenu("Test Add - itemId = 2")]
    public void TestAdd2()
    {
        AddItem(2);
    }
    [ContextMenu("Test Remove - itemId = 1")]
    public void TestRemove()
    {
        RemoveItem(1);
    }
    [ContextMenu("Test Remove - itemId = 2")]
    public void TestRemove2()
    {
        RemoveItem(2);
    }
    [ContextMenu("Test Save")]
    public void TestSave()
    {
        SaveInventory();
    }
}

