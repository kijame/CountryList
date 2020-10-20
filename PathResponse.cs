using System;
using System.Collections.Generic;

/*
    Class used to store the format of the JSON object for a successful
    response.
*/
namespace JsonResponse
{
    public class PathResponse
    {
        public string Start {get; set; }
        public string Destination {get; set; }
        public List<string> Path {get; set; }
    }
}