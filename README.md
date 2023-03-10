# SVT Take Home Assessment 

## Running the Project

Locally launch the service in https under port 5001 via:
```bash
dotnet run --launch-profile 'https' --project ./SVT/SVT.csproj
```
or http under port 5000 via:
```bash
dotnet run --launch-profile 'http' --project ./SVT/SVT.csproj
```

## Usage
You can obtain the closest robot via the following curl statement:
```
curl -X POST https://localhost:5001/api/robots/closest \
-H 'Content-Type: application/json' \
-d '{ "loadId": 123,  "x": 0,  "y": 0 }'
```
or  for http
```
curl -X POST http://localhost:5000/api/robots/closest \
-H 'Content-Type: application/json' \
-d '{ "loadId": 123,  "x": 0,  "y": 0 }'
```
Model validators should require all three fields, e.g. this should return a 400
```
curl -X POST http://localhost:5000/api/robots/closest \
-H 'Content-Type: application/json' \
-d '{ "loadId": 123 }'
```

## Running Unit Tests
```bash
dotnet test ./SVTTests/SVTTests.csproj
```

## Solution Notes

### Finding the Closest Robot
Went with a fairly straight forward approach for finding the closest robot; calculate the distance between the payload and each active (battery > 0) robot to determine the closest. In the event there's a tie, or both robots are within 10 distance units of the goal, use the battery as a tie breaker, and if there's still a tie, pick the first one. The reason for taking this approach is simplicity and the sample set not being large enough to be too concerned with performance. The solution should be linear (o(n)), but there could be some improvements down the road should the number of robots drastically increase, or performance needs to be extremly tight. Some things I'd look into in the future would be storing the robot's locations in a datastructure meant for spacial operations; something like a quad tree or a RTree. I could imagine a solution where robots are being tracked in real time and their positions are streamed to services that hold them in one of the previously mentioned datastructures (in memory). Having the data stored like this could enable searches like the one in this project to be made in O(log n) time. I think a downside of using something like an RTree would be situations where it needs to be rebalanced upon insert/update, but average case should still be o(log n).

### If I had more time
Lots of general clean up, but I took a little bit of time to jog my memory of all things .net (it's been a couple years since I've worked in it) and decided to split off the core business logic so I could more easily write some unit tests. There could probably be some improvments there, and it'd also be good to have some tests targeting the controller and model validation specifically. Lastly, I'd add some logging to help capture unexpected scenarios and edge cases.