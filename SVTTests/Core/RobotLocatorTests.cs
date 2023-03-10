namespace SVTTests;

using Moq;
using SVT.Core;
using SVT.Models;

public class RobotLocatorTests 
{
    private void AssertResult(Robot expectedRobot, double expectedDistance, ClosestResponseModel result) {
        Assert.NotNull(result);
        Assert.Equal(expectedRobot.RobotId, result.RobotId);
        Assert.Equal(expectedRobot.BatteryLevel, result.BatteryLevel);
        Assert.Equal(expectedDistance, result.DistanceToGoal);
    }

    [Fact]
    public void EmptyRobotListReturnsNull()
    {
        var result = RobotLocator.FindClosestRobot(new List<SVT.Models.Robot>(), 1, 1);

        Assert.Null(result);
    }

    [Fact]
    public void WithOneRobotReturnsRobot()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 100, X = 1, Y = 1 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne }, 0, 0);

        AssertResult(robotOne, Math.Round(Math.Sqrt(2), 2), result);
    }

    [Fact]
    public void WithMultipleRobotsReturnsClosest()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 100, X = 1, Y = 1 };
        var robotTwo = new Robot { RobotId = "2", BatteryLevel = 100, X = 2, Y = 2 };
        var robotThree = new Robot { RobotId = "3", BatteryLevel = 100, X = 3, Y = 3 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne, robotTwo, robotThree }, 3, 4);

        AssertResult(robotThree, 1, result);
    }

    [Fact]
    public void WithSubOneDistanceReturnsClosest()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 100, X = 1.1, Y = 1.1 };
        var robotTwo = new Robot { RobotId = "2", BatteryLevel = 100, X = 1.2, Y = 1.2 };
        var robotThree = new Robot { RobotId = "3", BatteryLevel = 100, X = 1.21, Y = 1.21 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne, robotTwo, robotThree }, 1.201, 1.202);

        AssertResult(robotTwo, Math.Round(Math.Sqrt(0.000005), 2), result);
    }

    [Fact]
    public void SameDistanceReturnsHighestBattery()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 99, X = 1, Y = 1 };
        var robotTwo = new Robot { RobotId = "2", BatteryLevel = 100, X = 1, Y = 1 };
        var robotThree = new Robot { RobotId = "3", BatteryLevel = 98, X = 1, Y = 1 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne, robotTwo, robotThree }, 0, 0);

        AssertResult(robotTwo, Math.Round(Math.Sqrt(2), 2), result);
    }

    [Fact]
    public void SameDistanceSameBatteryReturnsFirst()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 100, X = 2, Y = 2 };
        var robotTwo = new Robot { RobotId = "2", BatteryLevel = 100, X = 2, Y = 2 };
        var robotThree = new Robot { RobotId = "3", BatteryLevel = 100, X = 2, Y = 2 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne, robotTwo, robotThree }, 1, 1);

        AssertResult(robotOne, Math.Round(Math.Sqrt(2), 2), result);
    }

    [Fact]
    public void OrthagonallyPositionedXReturnsClosest()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 100, X = 1, Y = 1 };
        var robotTwo = new Robot { RobotId = "2", BatteryLevel = 100, X = 4, Y = 1 };
        var robotThree = new Robot { RobotId = "3", BatteryLevel = 100, X = 5, Y = 1 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne, robotTwo, robotThree }, 2, 1);

        AssertResult(robotOne, 1, result);
    }

    [Fact]
    public void OrthagonallyPositionedYReturnsClosest()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 100, X = 1, Y = 1 };
        var robotTwo = new Robot { RobotId = "2", BatteryLevel = 100, X = 1, Y = 4 };
        var robotThree = new Robot { RobotId = "3", BatteryLevel = 100, X = 1, Y = 5 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne, robotTwo, robotThree }, 1, 3);

        AssertResult(robotTwo, 1, result);
    }

    [Fact]
    public void LowBatteryStillReturnedIfClosest()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 1, X = 0, Y = 0 };
        var robotTwo = new Robot { RobotId = "2", BatteryLevel = 100, X = 5, Y = 5 };
        var robotThree = new Robot { RobotId = "3", BatteryLevel = 100, X = 6, Y = 6 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne, robotTwo, robotThree }, 2, 2);

        AssertResult(robotOne, Math.Round(Math.Sqrt(8), 2), result);
    }

    [Fact]
    public void ZeroBatteryIsIgnoredWhenClosest()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 0, X = 0, Y = 0 };
        var robotTwo = new Robot { RobotId = "2", BatteryLevel = 100, X = 5, Y = 5 };
        var robotThree = new Robot { RobotId = "3", BatteryLevel = 100, X = 6, Y = 6 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne, robotTwo, robotThree }, 2, 2);

        AssertResult(robotTwo, Math.Round(Math.Sqrt(18), 2), result);
    }

    [Fact]
    public void AllRobotsZeroBatteryReturnsNull()
    {
        var robotOne = new Robot { RobotId = "1", BatteryLevel = 0, X = 1, Y = 1 };
        var robotTwo = new Robot { RobotId = "2", BatteryLevel = 0, X = 4, Y = 4 };
        var robotThree = new Robot { RobotId = "3", BatteryLevel = 0, X = 5, Y = 5 };
        var result = RobotLocator.FindClosestRobot(new List<Robot>() { robotOne, robotTwo, robotThree }, 2, 2);

        Assert.Null(result);
    }
}