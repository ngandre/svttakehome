namespace SVT.Core;

using SVT.Models;
public static class RobotLocator {

    public static ClosestResponseModel? FindClosestRobot(IEnumerable<Robot> robots, double targetX, double targetY) {
        var closest = robots.Where(r => r.BatteryLevel > 0).Select(r => new ClosestResponseModel {
            RobotId = r.RobotId,
            DistanceToGoal = GetDistance(targetX, targetY, r.X, r.Y),
            BatteryLevel = r.BatteryLevel
        }).Min(Comparer<ClosestResponseModel>.Create((a, b) => {
            // choose greatest battery life if both robots are within 10 distance units of payload or there's a tie
            if((a.DistanceToGoal <= 10.0 && b.DistanceToGoal <= 10.0 && a.BatteryLevel != b.BatteryLevel) || a.DistanceToGoal == b.DistanceToGoal) {
                return a.BatteryLevel.CompareTo(b.BatteryLevel) * -1;
            }
            return a.DistanceToGoal.CompareTo(b.DistanceToGoal);
        }));

        if (closest == null) return null;

        closest.DistanceToGoal = Math.Round(closest.DistanceToGoal, 2);

        return closest;
    }

    private static double GetDistance(double aX, double aY, double bX, double bY) {
        return Math.Sqrt(Math.Pow(aX - bX, 2) + Math.Pow(aY - bY, 2));
    }
}