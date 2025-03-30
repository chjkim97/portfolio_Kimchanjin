using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    public ItemSO itemSO;
    private List<CardInfo> allCards = new List<CardInfo>();

    // 카드 정보 클래스
    public class CardInfo
    {
        public string name;
        public string description;
        public string type;
        public string type2;
        public int cost;
        public int usageType;
        public int cardNumber;

        public CardInfo(string name, string description, string type, string type2, int cost, int usageType, int cardNumber)
        {
            this.name = name;
            this.description = description;
            this.type = type;
            this.type2 = type2;
            this.cost = cost;
            this.usageType = usageType;
            this.cardNumber = cardNumber;
        }
    }
    public CardInfo GetCardByNumber(int cardNumber)
    {
        return allCards.Find(card => card.cardNumber == cardNumber);
    }
    void Start()
    {
        InitializeCardList();
    }
    public void ClearItems()
    {
        itemSO.items = new Item[0];
    }
    public void InitializeCardList()
    {
        //추가 할 때 shop과 battlereward에 추가
        //공용카드
        allCards.Add(new CardInfo("단검", "피해를 5 줍니다.", "Cont", "공격", 1, 2, 1));
        allCards.Add(new CardInfo("창", "피해를 12 줍니다.", "Cont", "공격", 2, 2, 2));
        allCards.Add(new CardInfo("작은 방패", "실드를 5 얻습니다.", "Once", "주문", 1, 1, 3));
        allCards.Add(new CardInfo("큰 방패", "실드를 12 얻습니다.", "Cont", "주문", 2, 1, 4));
        allCards.Add(new CardInfo("도끼", "취약을 +2 부여합니다. 피해를 8 줍니다. ", "Cont", "공격", 2, 2, 5));
        allCards.Add(new CardInfo("망치", "피해를 22 줍니다", "Cont", "공격", 3, 2, 6));
        allCards.Add(new CardInfo("권총 난사", "모든 적에게 피해를 10 줍니다", "Cont", "공격", 1, 3, 7));
        allCards.Add(new CardInfo("붕대", "체력을 5 회복합니다. 일회용", "Once", "주문", 1, 1, 8));
        allCards.Add(new CardInfo("산탄총", "모든 적에게 피해를 10 줍니다.", "Cont", "공격", 2, 3, 9));
        allCards.Add(new CardInfo("가시방패", "실드를 6 얻습니다. 가시를 3 얻습니다.", "Cont", "주문", 2, 1, 10));
        allCards.Add(new CardInfo("흡혈 송곳니", "피해를 6 줍니다. 체력을 2 회복합니다.", "Cont", "공격", 1, 2, 11));
        allCards.Add(new CardInfo("재빠른 타격", "피해를 2 줍니다. 카드를 1장 뽑습니다.", "Cont", "공격", 0, 2, 12));
        allCards.Add(new CardInfo("신성한 비", "모든 적에게 피해를 6 줍니다. 일회용", "Once", "공격", 0, 3, 13));
        allCards.Add(new CardInfo("비밀 작전", "실드를 3 얻습니다. 카드를 뽑습니다.", "Cont", "주문", 0, 1, 14));
        allCards.Add(new CardInfo("약점 공략", "취약을 3 부여합니다.", "Cont", "주문", 0, 2, 15));
        allCards.Add(new CardInfo("끈끈이 볼", "약화를 3 부여합니다.", "Cont", "주문", 0, 2, 16));
        allCards.Add(new CardInfo("기습", "피해를 8 줍니다.", "Cont", "공격", 0, 2, 17));
        allCards.Add(new CardInfo("천 갑옷", "실드를 8 얻습니다.", "Cont", "주문", 0, 1, 18));
        allCards.Add(new CardInfo("작은 주머니", "골드를 30 얻습니다. 일회용", "Once", "주문", 1, 1, 19));
        allCards.Add(new CardInfo("팔방미인", "힘과 실드를 1 얻습니다. 취약을 1 부여합니다. 피해를 1 줍니다. 카드를 1장 뽑습니다.", "Cont", "주문", 1, 2, 20));
        allCards.Add(new CardInfo("전력질주", "카드를 6장 뽑습니다. 일회용", "Once", "주문", 1, 1, 21));
        allCards.Add(new CardInfo("카드 주머니", "카드를 3장 뽑습니다. 일회용", "Once", "주문", 0, 1, 22));

        // 전사 카드
        allCards.Add(new CardInfo("방패 방어술", "내 턴이 시작할 때 실드가 사라지지 않습니다.", "Once", "지속", 2, 1, 101));
        allCards.Add(new CardInfo("방패 타격", "내 실드만큼 적에게 피해를 줍니다.", "Cont", "공격", 1, 2, 102));
        allCards.Add(new CardInfo("치고 빠지기", "피해를 6 주고 실드를 6 얻습니다.", "Cont", "공격", 1, 2, 103));
        allCards.Add(new CardInfo("유물 방패", "내 유물 1개당 실드를 2 얻습니다. 일회용", "Once", "주문", 1, 1, 104));
        allCards.Add(new CardInfo("방어 태세", "이번 턴에 카드를 사용할 때마다 실드를 4 얻습니다.", "Cont", "주문", 0, 1, 105));
        allCards.Add(new CardInfo("혈기 전환", "체력을 2 잃습니다. 실드를 15 얻습니다.", "Cont", "주문", 1, 1, 106));
        allCards.Add(new CardInfo("어둠의 거래", "내 턴이 끝날 때마다 힘을 3 얻습니다. ", "Once", "지속", 3, 1, 107));
        allCards.Add(new CardInfo("피의 거래", "체력을 1 잃고 힘을 3 얻습니다.", "Cont", "주문", 1, 1, 108));
        allCards.Add(new CardInfo("쾌검", "피해를 2씩 6번 줍니다.", "Cont", "공격", 2, 2, 109));
        allCards.Add(new CardInfo("광전사의 칼", "피해를 2씩 2번 줍니다. 흡수, 일회용", "Once", "공격", 2, 2, 110));

        // 도적 카드
        allCards.Add(new CardInfo("민첩한 찌르기", "이번턴에 사용한 카드만큼 피해를 줍니다. ", "Cont", "공격", 0, 2, 201));
        allCards.Add(new CardInfo("능숙한 찌르기", "힘을 +1 얻습니다. 피해를 3 줍니다.", "Cont", "공격", 0, 2, 202));
        allCards.Add(new CardInfo("민첩한 막기", "이번턴에 사용한 카드 한 장당 실드를 4 얻습니다.", "Cont", "주문", 0, 1, 203));
        allCards.Add(new CardInfo("보물지도", "이 카드를 내거나 버리면 카드를 2장 뽑습니다.", "Cont", "주문", 1, 1, 204));
        allCards.Add(new CardInfo("구멍난 지갑", "카드를 5장 뽑고 가장 왼쪽 카드를 2장 버립니다.", "Cont", "주문", 1, 1, 205));
        allCards.Add(new CardInfo("단검 주머니", "피해를 2씩 3번 줍니다.", "Cont", "공격", 1, 2, 206));
        allCards.Add(new CardInfo("마법 구슬", "이 카드를 내거나 버리면 마나를 2 얻습니다.", "Cont", "주문", 1, 1, 207));
        allCards.Add(new CardInfo("스택", "카드를 1장 뽑습니다. ", "Cont", "주문", 0, 1, 208));
        allCards.Add(new CardInfo("천 덧대기", "내가 카드를 낼 때마다 실드를 1 얻습니다.", "Once", "지속", 1, 1, 209));
        allCards.Add(new CardInfo("미래 예지", "현재 마나를 0으로 만듭니다. 다음턴에 그 2배의 마나를 얻습니다.", "Cont", "주문", 0, 1, 210));

        allCards.Add(new CardInfo("주사위 던지기", "피해를 5~10줍니다.", "Cont", "공격", 1, 2, 211));
        allCards.Add(new CardInfo("유리한 도박", "골드를 50 잃거나 200 얻습니다. 일회용", "Once", "주문", 1, 1, 212));
        allCards.Add(new CardInfo("체력 영약", "골드를 100 소모합니다. 최대 체력이 영구적으로 5 증가합니다. 일회용", "Once", "주문", 0, 1, 213));
        allCards.Add(new CardInfo("마나 영약", "골드를 50 소모합니다. 마나를 3 얻습니다. 일회용", "Once", "주문", 0, 1, 214));
        allCards.Add(new CardInfo("? 방패", "실드를 5~10 얻습니다.", "Cont", "주문", 1, 1, 215));
        allCards.Add(new CardInfo("이상한 포션", "적에게 약화 2, 취약 2, 힘 -1, 화상 3 중에 하나를 무작위로 적용합니다.", "Cont", "주문", 0, 2, 216));
        allCards.Add(new CardInfo("이상한 영약", " 나에게 실드 5, 이번 턴에 마나 2, 최대 마나 1, 힘 2 중에 하나를 무작위로 적용합니다.", "Cont", "주문", 0, 1, 217));
        allCards.Add(new CardInfo("전투 훈련", "내 턴이 끝날 때 힘을 4 얻습니다.", "Cont", "주문", 1, 1, 218));
        allCards.Add(new CardInfo("소매치기", "골드를 20 얻습니다. 피해를 6 줍니다.", "Cont", "공격", 1, 2, 219));
        allCards.Add(new CardInfo("동전 던지기", "보유 골드의 10%를 소모하여 그만큼 피해를 줍니다.", "Cont", "공격", 2, 2, 220));


        // 마법사 카드
        allCards.Add(new CardInfo("화염구", "피해를 6 줍니다. 화상을 +4 부여합니다.", "Cont", "공격", 1, 2, 301));
        allCards.Add(new CardInfo("마나 연구", "최대 마나가 1 증가합니다.", "Cont", "주문", 2, 1, 302));
        allCards.Add(new CardInfo("고급 마나 연구", "최대 마나가 2 증가합니다.", "Cont", "공격", 3, 1, 303));
        allCards.Add(new CardInfo("마나 엘릭서", "이번 턴에 마나를 2 얻습니다.", "Cont", "주문", 0, 1, 304));
        allCards.Add(new CardInfo("헬파이어", "피해를 20 줍니다. 화상을 +8 부여합니다.", "Cont", "공격", 3, 2, 305));
        allCards.Add(new CardInfo("매직 실드", "실드를 최대 마나 * 4 만큼 얻습니다.", "Cont", "주문", 2, 1, 306));
        allCards.Add(new CardInfo("신비한 마법서", "카드를 3장 뽑습니다.", "Cont", "주문", 1, 1, 307));
        allCards.Add(new CardInfo("약화 마법", "적에게 힘을 -3 부여합니다.", "Cont", "주문", 1, 2, 308));
        allCards.Add(new CardInfo("윈드 커터", "피해를 4씩 2번 줍니다.", "Cont", "공격", 1, 2, 309));
        allCards.Add(new CardInfo("불길", "모든 적에게 화상을 +4 부여합니다.", "Cont", "주문", 0, 3, 310));
        allCards.Add(new CardInfo("불의 바다", "내 턴이 끝날 때 모든 적에게 화상을 +3 부여합니다.", "Once", "지속", 1, 3, 311));
        allCards.Add(new CardInfo("얼음 방패", "카드를 1장 뽑습니다. 실드를 +8 얻습니다.", "Cont", "주문", 1, 1, 312));
        allCards.Add(new CardInfo("화염 확산", "적의 화상을 2배로 늘립니다.", "Cont", "주문", 2, 2, 313));
        allCards.Add(new CardInfo("불씨", "카드를 1장 뽑습니다. 화상을 4 부여합니다.", "Cont", "주문", 1, 2, 314));
        allCards.Add(new CardInfo("황금 재련", "적의 화상 1당 10 골드를 얻습니다. 일회용", "Once", "주문", 0, 2, 315));
        allCards.Add(new CardInfo("점화", "최대마나 *3 만큼 화상을 부여합니다.", "Cont", "주문", 1, 2, 316));
        allCards.Add(new CardInfo("화염 방패", "적의 화상 만큼 실드를 얻습니다.", "Cont", "주문", 1, 2, 317));
        allCards.Add(new CardInfo("방어 마법진", "내 턴이 끝날 때 실드를 3 얻습니다.", "Cont", "주문", 1, 1, 318));
        allCards.Add(new CardInfo("완전 방어", "실드를 30 얻습니다.", "Cont", "주문", 3, 1, 319));
        allCards.Add(new CardInfo("기적", "카드를 1장 뽑습니다. 마나를 2 회복합니다.", "Cont", "주문", 1, 1, 320));

        //방해카드
        allCards.Add(new CardInfo("무거운 돌", "이 돌은 너무 무겁습니다.", "Once", "방해", 10, 1, 401));
        allCards.Add(new CardInfo("차가운 냉기", "카드를 한장 뽑습니다.", "Once", "방해", 1, 1, 402));
        allCards.Add(new CardInfo("휩쓰는 화염", "모든 캐릭터에게 화상을 3 부여합니다.", "Once", "방해", 1, 3, 403));
        allCards.Add(new CardInfo("쓸모 없는 돌맹이", "이 돌은 쓸모 없어 보입니다.", "Once", "방해", 1, 2, 404));
        allCards.Add(new CardInfo("무딘 칼", "피해를 1 줍니다.", "Once", "방해", 1, 2, 405));
        allCards.Add(new CardInfo("낡은 방패", "실드를 1 얻습니다.", "Once", "방해", 1, 1, 406));
        


    }
    public void AddCardToPlayer(int cardNumber)
    {
        // 입력된 카드 번호로부터 해당하는 카드 정보 찾기
        CardInfo selectedCard = allCards.Find(card => card.cardNumber == cardNumber);

        // 카드가 존재하는지 확인하고 추가
        if (selectedCard != null)
        {      
            itemSO.AddNewItem(selectedCard.name, selectedCard.description, selectedCard.type, selectedCard.type2, selectedCard.cost, selectedCard.usageType, selectedCard.cardNumber);
            if(itemSO.items.Length >= 20)
            {
                GameData.Instance.ItemCount();
            }
        }
        else
        {
            Debug.LogError("Invalid card number: " + cardNumber);
        }
    }

    public Item MakeCardToItem(int cardNumber) 
    {
        CardInfo selectedCard = allCards.Find(card => card.cardNumber == cardNumber);
        if (selectedCard != null)
        {
            ItemSO itemSOInstance = ScriptableObject.CreateInstance<ItemSO>();
            // MakeItem 함수를 사용하여 Item 생성
            Item item = itemSOInstance.MakeItem(
                name: selectedCard.name,
                effect: selectedCard.description, // 선택된 카드의 effect를 사용
                type: selectedCard.type,
                type2: selectedCard.type2,
                cost: selectedCard.cost,
                usetype: selectedCard.usageType, // 사용 타입
                number: selectedCard.cardNumber // 카드 번호
            );

            return item; // 생성한 Item 객체 반환
        }
        else
        {
            Debug.LogError("유효하지 않은 카드 번호: " + cardNumber);
            return null; // 카드가 없을 경우 null 반환
        }
    }

}