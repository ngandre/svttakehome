namespace SVT.Models;

public class ClosestResponseModel {
    public string RobotId { get; set; }
    public double DistanceToGoal { get; set; }
    public int BatteryLevel { get; set; }
}