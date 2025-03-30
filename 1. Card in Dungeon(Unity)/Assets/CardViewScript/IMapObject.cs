using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapObject
{
    void OnClicked();    
    void OnUnclicked();
    void SetParentNode(TestMapNode node); 
    void RedSignOn(); 
    void GreenSignOn(); 

    void MinSignOn();

    void MinSignOff();
    Transform Transform { get; }
}
