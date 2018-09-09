using UnityEngine;
using System.Collections;

// 본 스크립트는 아이템의 속성을 정해주는 스크립트입니다.
// 본 스크립트는 ItemDataBase 스크립트와 연계됩니다.
[System.Serializable]
// 위 스크립트 한줄은 스크립틔의 직렬화를 위한 소스코드입니다.
// 위 줄을 사용하면 유니티3D에서 직접 모든 변수에 대해 접근할 수 있습니다.
// 만약 위 코드를 사용하지 않는다면,
// 아래 코드 public class Item 코드를
// public class Item : MonoBehavior(?)로 작성해주어야 합니다.
// 그냥 쓰셔요 ' ㅅ' 
public class Item
{
    public string itemName;         // 아이템의 이름
    public int itemID;              // 아이템의 고유번호
    public string itemDes;          // 아이템의 설명
    public Texture2D itemIcon;      // 아이템의 아이콘(2D)
    public int itemPower;           // 아이템의 공격력
    public int itemSpeed;           // 아이템의 공격속도
    public int itemDefense;         // 아이템의 방어력
    public int itemEvasion;         // 아이템의 회피력
    public int itemRecoveryAmount;  // 아이템의 회복량
    public ItemType itemType;       // 아이템의 속성 설정

    public enum ItemType            // 아이템의 속성 설정에 대한 갯수
    {
        Weapon,                     // 무기류 (검, 방패, 창 등 해당)
        Costume,                    // 옷류   (상의, 하의, 모자 등 해당)
        Quest,                      // 퀘스트 아이템류
        Use         // 소모품류
    }

    public Item()
    {

    }

    public Item(string img, string name, int id, string desc, int power, int speed, int defense, int evasion, int hprecover, ItemType type)
    // 아이템의 필요한 속성을 모두 위에 적어줍니다.(다른곳에서 받아올 예정)
    {
        itemName = name;
        // 윗 줄과 같이 모두 연결해줍니다.
        itemID = id;
        itemDes = desc;
        itemPower = power;
        // itemIcon 속성은 별도의 방법을 이용합니다.
        itemIcon = Resources.Load<Texture2D>("ItemIcons/34x34icons180709_" + img);
        // itemIcon 속성은 별도의 방법을 이용합니다.

        itemSpeed = speed;
        itemDefense = defense;
        itemEvasion = evasion;
        itemRecoveryAmount = hprecover;
        itemType = type;
        // 으하하하하
    }
}
