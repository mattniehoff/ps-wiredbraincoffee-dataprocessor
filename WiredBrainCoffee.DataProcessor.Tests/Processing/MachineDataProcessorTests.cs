

using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessorTests : IDisposable
{

    private readonly FakeCoffeeCountStore coffeeCountStore;
    private readonly MachineDataProcessor machineDataProcessor;

    public MachineDataProcessorTests()
    {
        coffeeCountStore = new FakeCoffeeCountStore();
        machineDataProcessor = new MachineDataProcessor(coffeeCountStore);
    }

    public void Dispose()
    {
        // This runs after every test - could delete files created during the test
    }

    [Fact]
    public void ShouldClearPreviousCoffeeCount()
    {
        // Arrange
        var items = new[] {
            new MachineDataItem("Cappuccino", new DateTime(2022, 10, 27, 8, 0, 0)),
        };

        // Act
        machineDataProcessor.ProcessItems(items);
        machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, coffeeCountStore.SavedItems.Count);
        foreach (var item in coffeeCountStore.SavedItems)
        {
            Assert.Equal("Cappuccino", item.CoffeeType);
            Assert.Equal(1, item.Count);
        }
    }

    [Fact]
    public void ShouldIgnoreItemsThatAreNotNewer()
    {
        // Arrange
        var items = new[] {
            new MachineDataItem("Cappuccino", new DateTime(2022, 10, 27, 8, 0, 0)),
            new MachineDataItem("Cappuccino", new DateTime(2022, 10, 27, 7, 0, 0)),
            new MachineDataItem("Cappuccino", new DateTime(2022, 10, 27, 7, 10, 0)),
            new MachineDataItem("Cappuccino", new DateTime(2022, 10, 27, 9, 0, 0)),
            new MachineDataItem("Espresso", new DateTime(2022, 10, 27, 10, 0, 0)),
            new MachineDataItem("Espresso", new DateTime(2022, 10, 27, 10, 0, 0)),
        };

        // Act
        machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, coffeeCountStore.SavedItems.Count);

        var item = coffeeCountStore.SavedItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = coffeeCountStore.SavedItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }

    [Fact]
    public void ShouldSaveCountPerCoffeeType()
    {
        // Arrange
        var items = new[] {
            new MachineDataItem("Cappuccino", new DateTime(2022, 10, 27, 8, 0, 0)),
            new MachineDataItem("Cappuccino", new DateTime(2022, 10, 27, 9, 0, 0)),
            new MachineDataItem("Espresso", new DateTime(2022, 10, 27, 13, 0, 0)),
        };

        // Act
        machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, coffeeCountStore.SavedItems.Count);

        var item = coffeeCountStore.SavedItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = coffeeCountStore.SavedItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }

}
