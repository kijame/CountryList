using System;
using System.Collections.Generic;

using NAMap;

namespace Helper
{
    public static class CountrySearch
    {
        public static List<string> shortestPath(string dest)
        {
            var path = new List<string>();
            var start = "USA";
            // predecessor graph to compute path at end
            Dictionary<string, string> parent = new Dictionary<string, string>();
            Dictionary<string, List<string>> countryMap = NorthAmerica.initializeNorthAmerica();
            Queue<string> bfsQ = new Queue<string>();

            // edge case for if we started at destination
            if(start == dest)
            {
                path.Add(start);
                return path;
            }

            // initializes parent graph to be empty
            foreach(KeyValuePair<string, List<string>> entry in countryMap)
            {
                parent[entry.Key] = "";
            }

            bfsQ.Enqueue(start);
            parent[start] = start;
            while(bfsQ.Count != 0)
            {
                var curr = bfsQ.Dequeue();
                // check if we found destination
                if(curr == dest)
                {
                    break;
                }
                foreach(string next in countryMap[curr])
                {
                    // if the nodes has not already been reached then store it in queue
                    if(parent[next] == "")
                    {
                        bfsQ.Enqueue(next);
                        parent[next] = curr;
                    }
                }
            }

            // compute the path from parent graph if one exists
            string it = parent[dest];
            // checks if the destination was reached
            // this allows us to know if we receive the empty list
            // the locations were valied but the path didn't exist
            if(it != "")
            {
                path.Add(dest);
                while(it != start)
                {
                    path.Add(it);
                    it = parent[it];
                }
                path.Add(start);
            }
            path.Reverse();

            return path;
        }
    }
}