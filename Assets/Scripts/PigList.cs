using System.Collections.Generic;
using control;
using UnityEngine;

[CreateAssetMenu(fileName = "Pigs", menuName = "Create pig list", order = 1)]
public class PigList : ScriptableObject {
    public List<Flammable> pigs = new List<Flammable>();

    public Stats.Stats levelStats, gameStats;

    private void OnEnable() { }

    public void remove(Flammable flammable) {
        pigs.Remove(flammable);
    }

    public void add(Flammable flammable, bool isABody) {
        pigs.Add(flammable);
        flammable.name = "Flammable (" + levelStats.bodyCount + ")";

        if (isABody) {
            levelStats.bodyCount++;
            gameStats.bodyCount++;
        }
    }
}