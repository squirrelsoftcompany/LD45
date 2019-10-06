using System.Collections.Generic;
using control;
using UnityEngine;

[CreateAssetMenu(fileName = "Pigs", menuName = "Create pig list", order = 1)]
public class PigList : ScriptableObject {
    public int bodyCount;
    public List<Flammable> pigs = new List<Flammable>();

    private void OnEnable() {
        bodyCount = 0;
    }

    public void remove(Flammable flammable) {
        pigs.Remove(flammable);
    }

    public void add(Flammable flammable) {
        pigs.Add(flammable);
        flammable.name = "Flammable (" + bodyCount + ")";
        bodyCount++;
    }
}