using UnityEngine;

public interface IBayblade
{
    float MoveSpeed { get; }          // read-only move speed
    void AddRecoil(Vector3 recoil);   // method to apply recoil
    void RemoveSpin(float amount);    // method to reduce spin
    Transform Transform { get; }      // expose transform
}