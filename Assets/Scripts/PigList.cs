using System.Collections.Generic;
using control;

[UnityEngine.CreateAssetMenu(fileName = "Pigs", menuName = "Create pig list", order = 1)]
public class PigList : UnityEngine.ScriptableObject {
    public List<Flammable> pigs = new List<Flammable>();

    public void remove(Flammable flammable) {
        pigs.Remove(flammable);
    }

    public void add(Flammable flammable) {
        pigs.Add(flammable);
    }
}