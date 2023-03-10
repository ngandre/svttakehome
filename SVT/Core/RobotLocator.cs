namespace SVT.Core;

using SVT.Models;
public static class RobotLocator {

    public static ClosestResponseModel? FindClosestRobot(IEnumerable<Robot> robots, double targetX, double targetY) {
        var closest = robots.Where(r => r.BatteryLevel > 0).Min(Comparer<Robot>.Create((a, b) => {
            // get each robot's distance to payload
            var aDistance = Math.Sqrt(Math.Pow(a.X - targetX, 2) + Math.Pow(a.Y - targetY, 2));
            var bDistance = Math.Sqrt(Math.Pow(b.X - targetX, 2) + Math.Pow(b.Y - targetY, 2));

            // if distance to payload is the same, compare battery life
            if(aDistance == bDistance) {
                return a.BatteryLevel.CompareTo(b.BatteryLevel) * -1;
            }
            return aDistance.CompareTo(bDistance);
        }));
        if (closest == null) {
            return null;
        }

        return new ClosestResponseModel {
            RobotId = closest.RobotId,
            DistanceToGoal = Math.Round(GetDistance(targetX, targetY, closest.X, closest.Y), 2),
            BatteryLevel = closest.BatteryLevel
        };
    }

    private static double GetDistance(double aX, double aY, double bX, double bY) {
        return Math.Sqrt(Math.Pow(aX - bX, 2) + Math.Pow(aY - bY, 2));
    }
}