using System;
using System.Collections.Generic;

/*
    Class used to initialize a graph to represent
    the map of North America, it is represented
    by a hash map of countries returning the list
    of the countries they are connected to
*/
namespace NAMap
{
    public static class NorthAmerica
    {
        public static Dictionary<string, List<string>> initializeNorthAmerica()
        {
            Dictionary<string, List<string>> countryMap = new Dictionary<string, List<string>>();
            // Initialize the map of North America
            // Canada borders the United States
            countryMap.Add("CAN", new List<string>{"USA"});
            // The United States borders Canada and Mexico
            countryMap.Add("USA", new List<string>{"CAN", "MEX"});
            // Mexico borders the United States, Guatemala, and Belize
            countryMap.Add("MEX", new List<string>{"USA", "GTM", "BLZ"});
            // Belize borders Mexico and Guatemala
            countryMap.Add("BLZ", new List<string>{"MEX", "GTM"});
            // Guatemala borders Mexico, Belize, El Salvador, and Honduras
            countryMap.Add("GTM", new List<string>{"MEX", "BLZ", "SLV", "HND"});
            // El Salvador borders Guatemala and Honduras
            countryMap.Add("SLV", new List<string>{"GTM", "HND"});
            // Honduras borders Guatemala, El Salvador, and Nicaragua
            countryMap.Add("HND", new List<string>{"GTM", "SLV", "NIC"});
            // Nicaragua borders Honduras and Costa Rica
            countryMap.Add("NIC", new List<string>{"HND", "CRI"});
            // Costa Rica borders Nicaragua and Panama
            countryMap.Add("CRI", new List<string>{"NIC", "PAN"});
            // Panama borders Costa Rica
            countryMap.Add("PAN", new List<string>{"CRI"});
            return countryMap;
        }
    }
}