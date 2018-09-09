using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Start()
    {
        //items.Add(new Item(아이콘명, 이름, 아이템아이디, 설명, 공격력, 공속, 방어력, 회피력, 회복량, 아이템 속성여부));
        items.Add(new Item("3", "맛 좋은 사과", 1002, "먹으면 체력 1칸을 회복하는 사과", 0, 0, 0, 0, 1, Item.ItemType.Use));
        items.Add(new Item("28", "붉은 물약", 1001, "마시면 체력 가득 채워주는 마법의 물약", 0, 0, 0, 0, 2, Item.ItemType.Use));
        items.Add(new Item("25", "상한 고기", 1003, "먹으면 체력 1칸이 깎이는 고기", 0, 0, 0, 0, -1, Item.ItemType.Use));

    }

}
