# **Country List Api Project** - Kian Melhus

## Problem Description
The Country List API is an API that is used to determine what countries must be traveled through by a driver to reach their destination on the North American continent, starting in the United States. This API was developed to demonstrate my software engineering skills for the CH Robinson hiring team. The problem statement it is solving is, "to serve the business needs of CH Robinson to be able to calculate what customs documents a shipment will need when traveling through North America".

___
## Requirements
The requirements for this program were very light as it was mostly a creative exercise with no back-and-forth between stakeholders at CH Robinson.<br />
**formal requirements**
- endpoint of the form
  - ```www.yourdomain.com/{country}```
  - {country} is a 3 digit country code from North America. e.g. (MEX, HND, PAN)
- should return
  - ```["USA", ... , "country"]```

___
## How to use
This api is served from a public http endpoint.
You can reach this endpoint via
- ```https://countrylistkian.azurewebsites.net/api/countrylist/{country}```
The valid endpoints for the api are (not case sensitive):
- ```[CAN, USA, MEX, BLZ, GTM, SLV, HND, NIC, CRI, PAN]```

**Success**
<br />In the case of a success you will be returned a status code 200 and a json
object with the start and end destinations and an array with the shortest list
going from [USA -> Destination]<br />

```
{
    "start": "USA",
    "destination": "CAN",
    "path": [
        "USA",
        "CAN"
    ]
}
```
**Failure (not found)**
- If the destination country is not included in the map the API returns error code 400 and the message
  - "The destination {input} is not contained in North America"

- If the path does not exist from USA to the destination country the API returns error code 400 and the message
  - "No path from USA to {input} found in the map"

___
## Basic Design
The design of my program is a serverless cloud function that takes the country endpoint as input and performs breadth-first search to find the shortest path on the map of North America and returns the list that represents the path back to the user.

**Design Reasoning**

- **Cloud Function**
  - I chose to design this API as a simple cloud function that serviced the endpoint for countrylist/{country} because I thought it would be a frictionless lightweight option to deliver the requirements for this API.
  - My design also has the benefit that it could be easily scaled because the service provider can spin up as many instances as I need based on traffic.
    - This feature of scalability also in this instance allowed me to deploy this api endpoint for free in this case because it does not need to currently handle much traffic.
  - I've also had better experiences in terms of reliability when designing simple cloud functions, when developing an API through a classical web api service my application ended up with difficulties at the payment tier I chose with cold-start times.

- **Serverless**
  - I chose a serverless design because the only information the backend needed to calculate the list was the map of North America and the target country to be reached. This design choice was able to be efficiently computed by the api by hardcoding a graph that represents the map of North America, from this decision I saved significant time and compute costs of reaching out to a server.
  - Another reason for keeping the cloud function serverless was how simple this choice was to implement; I saved a lot of time and potential complications and errors by only using as many moving parts as was necessary to create a program that fulfilled the requirements; while still maintaining modust robustness and flexibility.

- **Breadth-first Search**
  - I chose to use breadth-first search because it solved the problem of finding a path and returning the path representing the list in a straightforward and extendible way.
  - I had the possible option of hardcoding or precomputing the paths because there were only 10 short paths that could be possibly returned. Even though this option would technically be very efficient, I decided against this option because it did not feel very elegant and would disrupt any future extensions that I could develop on top of this API. 
    - The precomputing option would be interesting since their are only 195 countries according to Google and not all of them are connected by land bridges; it wouldn't be unreasonable to store every possible connection in a server and then only be limited by the communication speed between the server and the API client.
  - To create the graph that was searched I chose to represent it as a Dictionary of countries that stored a list of countries reachable by that country, I came to this decision because since a Dictionary is implemented as a hash-map it would be able to quickly return the list of reachable nodes on the graph, since the graph was sparsely connected I left the reachable country lists as regular lists because they would not gain much performance benefits from the use of more hash maps.

**Assumptions**
  - This Program should return error code 400 and a short message if there was no way to return a path given the country code input to the API.
  - I don't need to make a special case for Alaska.

___
## Framework
The final framework I chose for implementing this service was and Azure Function written in C#. This decision was not made because I am already familiar with deploying Azure Functions and I thought it would be useful to demonstrate that familiarity to CH Robinson.
___
## Implementation
**File structuring**
- CountryList.cs:
  - This file contains the Azure HTTP trigger Function for the countrylist/{country} endpoint, it handles all of the request and response details and calls the bfs on the input and returns a JSON object containing the start point, the end point, and the path between those two points.
- CountrySearch.cs:
  - This is the file that contains the ```shortestPath``` function which handles the logic for the BFS search on the North American map between two points on the map and returns the List<string> path which represents the shortest path.
- NorthAmerica.cs:
  - This file contains the the class NorthAmerica which has one function ```initializeNorthAmerica()``` which returns the graph representing North America represented by a dictionary containing 3 digit country codes that have the value of a list of country codes representing the countries that can be reached from themselves.
- PathResponse.cs:
  - This file houses the class ```PathResponse``` which is used as the model for the uniform JSON response that the API returns from the countrylist/{country} endpoint with the string start, string destination, List<string> Path attributes.
___
## Review
**Summary**
I really enjoyed this project because it allowed me to piece together a lot of the knowledge and skills I have gained studying computer science over the last three and a half years. By the end of the project I had made a lot of realizations of different things I wish I had done differently and also a lot of different ideas for different stretch goals for future iterations.
- My code has the start location hardcoded into the application logic, if we opened up the http endpoint to also receive the start location, then the algorithm could easily compute the path from any point to any other point on the map, allowing more international usage of the API.
- The country search BFS function could have been made more extensible by implementing it as an interface that could take different maps, this would allow the program to be expanded to contain any connected land mass.
- If I stored the initial connection lists inside of a database I could set up a system where I could manage the different connections
- Another interesting extension that this problem could take on would be if the country map also had edge weights with distance and other information (such as required customs documents per country)
  - This would allow the search algorithm to be customized for many different parameters, such as true distance or number of customs documents.
- Another interesting stretch goal would be to collect more detailed telemetry about the request, response, and computation, and store this information through a telemetry storage system.
- In the future if any of these features would be added to the program the first task that would need to be completed would be to set up unit tests to maintain reliability of the previous features of the program.
- In the process of designing and implementing this API I learned more about how to create a full API as one azure function, I think this would be an interesting thing to do for this API if it was extended to cover more endpoints or resources.