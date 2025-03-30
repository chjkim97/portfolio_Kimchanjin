using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string effect;
    public string type;
    public string type2;
    public int cost;
    public Sprite sprite;
    public int usetype;
    public int number;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO" )]
public class ItemSO : ScriptableObject
{
    public Item[] items;
    
    public Item MakeItem(string name, string effect, string type, string type2, int cost, int usetype, int number)
    {
        Item item = new Item();
        item.name = name;
        item.effect = effect;
        item.type = type;    
        item.type2 = type2;
        item.cost = cost;
        item.usetype = usetype;
        item.number = number;
        string spritePath = "card" + number;
        Sprite loadedSprite = Resources.Load<Sprite>(spritePath);
        if (loadedSprite != null)
        {
           item.sprite = loadedSprite;

        }
        else
        {
            Debug.LogWarning($"Sprite not found at path: {spritePath}");
        }
        return item;
    }
    // 새로운 아이템을 추가하는 함수
   public void AddNewItem(string name, string effect, string type,string type2, int cost,  int usetype, int number)
    {
        // 현재 아이템 배열의 길이를 확장하여 새로운 아이템을 추가합니다.
        int currentLength = items.Length;
        Item[] newItems = new Item[currentLength + 1];

        // 기존의 아이템을 새 배열에 복사합니다.
        for (int i = 0; i < currentLength; i++)
        {
            newItems[i] = items[i];
        }

        // 새 아이템을 추가합니다.
        newItems[currentLength] = new Item();
        newItems[currentLength].name = name;
        newItems[currentLength].effect = effect;
        newItems[currentLength].type = type;
        newItems[currentLength].type2 = type2;
        newItems[currentLength].cost = cost;

        
        newItems[currentLength].usetype = usetype;
        newItems[currentLength].number = number;
        string spritePath = "card" + number;
        Sprite loadedSprite = Resources.Load<Sprite>(spritePath);
         if (loadedSprite != null)
        {
            newItems[currentLength].sprite = loadedSprite;
            
        }
        else
        {
            Debug.LogWarning($"Sprite not found at path: {spritePath}");
        }
        // 새 배열을 현재 배열로 교체합니다.
        items = newItems;
    }
    
    public void RemoveItem(Item itemToRemove)
    {
        int currentLength = items.Length;
        Item[] newItems = new Item[currentLength - 1];
        bool itemRemoved = false;
        int newIndex = 0;

        // 새 배열에 아이템 복사하면서, 제거할 아이템(카드)는 제외시킨다
        for (int i = 0; i < currentLength; i++)
        {
            if (items[i] != itemToRemove)
            {
                newItems[newIndex] = items[i];
                newIndex++;
            }
            else
            {
                itemRemoved = true;
            }
        }

        if (itemRemoved)
        {
            // 새 배열을 현재 배열로 교체한다.
            items = newItems;
            Debug.Log($"removed: {itemToRemove.name}");
        }
        else
        {
            Debug.LogWarning("Item not found.");
        }
    }
}
