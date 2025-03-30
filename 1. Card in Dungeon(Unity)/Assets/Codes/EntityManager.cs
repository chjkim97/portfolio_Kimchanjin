// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EntityManager : MonoBehaviour
// {
//     public static EntityManager instance { get; private set; }
//     void Awake() => instance = this;

//     [SerializeField] GameObject entityPrefab;
//     [SerializeField] List<Entity> myEntities;
//     [SerializeField] Entity myEmptyEntity;


//     const int MAX_ENTITY_COUNT = 6;
//     public bool IsFullMyEntities => myEntities.Count >= MAX_ENTITY_COUNT && !ExistMyEmptyEntity;
//     bool ExistMyEmptyEntity => myEntities.Exists(x => x == myEmptyEntity);
//     int MyEmptyEntityIndex => myEntities.FindIndex(x => x == myEmptyEntity);




//     void EntityAlignment()
//     {
//         float targetY = -4.35f;
//         var targetEntities = myEntities;

//         for (int i = 0; i < targetEntities.Count; i++)
//         {
//             float targetX = (targetEntities.Count - 1) * -3.4f + i * 6.8f;

//             var targetEntity = targetEntities[i];
//             targetEntity.originPos = new Vector3(targetX, targetY, 0);
//             targetEntity.MoveTransform(targetEntity.originPos, true, 0.5f);
//             targetEntity.GetComponent<Order>()?.SetOriginOrder(i);
//         }
//     }

//     public bool SpawnEntity( Item item, Vector3 spawnPos)
//     {

//         if (IsFullMyEntities || !ExistMyEmptyEntity)
//         { return false; }
       
     
//         var entityObject = Instantiate(entityPrefab, spawnPos, Utils.QI);
//         var entity = entityObject.GetComponent<Entity>();

        
//         myEntities[MyEmptyEntityIndex] = entity;
        
//          //entity.isMine = isMine;
//         entity.Setup(item);
//         EntityAlignment();

//         return true;
//     }

//     public void InsertMyEmptyEntity(float xPos)
//     {
//         if (IsFullMyEntities)
//             return;

//         if (!ExistMyEmptyEntity)
//             myEntities.Add(myEmptyEntity);

//         Vector3 emptyEntityPos = myEmptyEntity.transform.position;
//         emptyEntityPos.x = xPos;
//         myEmptyEntity.transform.position = emptyEntityPos;

//         int _emptyEntityIndex = MyEmptyEntityIndex;
//         myEntities.Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));
//         if (MyEmptyEntityIndex != _emptyEntityIndex)
//             EntityAlignment();
//     }
// }
