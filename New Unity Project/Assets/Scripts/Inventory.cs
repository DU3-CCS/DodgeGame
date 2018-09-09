using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    // 인벤토리를 리스트로 만듭니다.
    private itemDatabase db;
    // 아이템 데이터베이스는 db로 축약해서 사용합니다.

    public int slotX, slotY;    // 인벤토리 가로 세로 속성 설정 위한 변수
    public List<Item> slots = new List<Item>(); // 인벤토리 속성 변수

    private bool showInventory = true;
    public GUISkin skin;

    void Start()
    {
        for (int i = 0; i < slotX * slotY; i++)
        {
            slots.Add(new Item());
            // 아이템 슬롯칸에 빈 오브젝트 추가하기
            inventory.Add(new Item());
            // 인벤토리에 추가
        }
        db = GameObject.FindGameObjectWithTag("Item Database").GetComponent<itemDatabase>();
        // 디비 변수에 "Item Database" 태그를 가진 오브젝트를 연결합니다.
        // 그리고 그 중 가져오는 컴포넌트는 "itemDatabse"라는 속성입니다.
            //inventory[0] = db.items[0];
           // inventory[1] = db.items[1];
        for (int i = 0;/*db.items[i]!=null*/i < slotX * slotY; i++)
        // 반복문을 이용하여 전체 인벤토리에 저장토록 합니다.
        {
            if (db.items[i] != null)
            {
                inventory[i] = db.items[i];
                // 디비의 아이템칸에 비어있지 않다면, 저장
            }
            else
            {
                // 디비의 아이템칸이 비어있다면 다른 행동을 하도록 유도합니다.

            }
        }
    }

    void Update()
    {

    }
    void OnGUI()
    {
        GUI.skin = skin;
        if (showInventory)
        {
            DrawInventory();
        }
    }

    void DrawInventory()
    {
        int k = 0;
        for (int j = 0; j < slotY; j++)
        {

            for (int i = 0; i < slotX; i++)
            {
                Rect slotRect = new Rect(i * 52 + 0, j * 52 + 0, 50, 50);
                // 박스 분할하기
                GUI.Box(slotRect, "", skin.GetStyle("slot background"));
                // 각 박스의 생성 위치를 설정해주는 곳입니다. skin.GetStyle은 이전에 만들었던 skin을 불러오는 것임

                // 기능 추가하기
                slots[k] = inventory[k];
                if (slots[k].itemName != null)
                {
                    GUI.DrawTexture(slotRect, slots[k].itemIcon);
                    Debug.Log(slots[k].itemName);
                }

                k++;
                // 갯수 증가
            }
        }
    }
}
