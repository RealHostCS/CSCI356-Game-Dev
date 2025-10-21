using UnityEngine;

public interface IBayblade
{
    float MoveSpeed { get; }          
    void AddRecoil(Vector3 recoil);  
    void RemoveSpin(float amount);    
    Transform Transform { get; }     
}