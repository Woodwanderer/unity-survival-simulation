using System.Xml.Serialization;
using UnityEngine;

public interface IAction
{
    void Start();
    void Tick(float dt);
    void Cancel();
    bool IsFinished {  get; }
}
