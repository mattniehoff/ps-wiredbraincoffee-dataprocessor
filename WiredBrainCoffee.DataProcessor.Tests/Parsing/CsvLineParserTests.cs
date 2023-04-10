namespace WiredBrainCoffee.DataProcessor.Parsing;

public class CsvLineParserTests
{

    [Fact]
    public void ShouldParseValidLine()
    {
        // Arrange
        string[] csvLines = new[] { "Espresso; 10 / 27 / 2022 8:01:16 AM" };

        // Act
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // Assert
        Assert.NotNull(machineDataItems);
        Assert.Single(machineDataItems);
        Assert.Equal("Espresso", machineDataItems.First().CoffeeType);
        Assert.Equal(new DateTime(2022, 10, 27, 8, 1, 16), machineDataItems.First().CreatedAt);
    }

    [Fact]
    public void ShouldSkipEmptyLines()
    {
        // Arrange
        string[] csvLines = new[] { string.Empty, " " };

        // Act
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // Assert
        Assert.NotNull(machineDataItems);
        Assert.Empty(machineDataItems);
    }

    [InlineData("Cappuccino", "Invalid csv line")]
    [InlineData("Cappuccino;invaliddate", "Invalid datetime in csv line")]
    [Theory]
    public void ShouldThrowExceptionForInvalidLine(string csvLine, string expectedMessagePrefix)
    {
        // Arrange        
        string[] csvLines = new[] { string.Empty, csvLine };

        // Act/Assert
        var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));

        Assert.Equal($"{expectedMessagePrefix}: {csvLine}", exception.Message);
    }

}
