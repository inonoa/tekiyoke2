using System.Collections.Generic;
using UnityEngine;


public class HeroMutekiManager : MonoBehaviour
{
    HashSet<string> mutekiFilters = new HashSet<string>();
    
    public void AddMutekiFilter(string key)
    {
        mutekiFilters.Add(key);
    }
    public void RemoveMutekiFilter(string key)
    {
        mutekiFilters.Remove(key);
    }
    
    public bool CanBeDamaged => mutekiFilters.Count == 0;
}