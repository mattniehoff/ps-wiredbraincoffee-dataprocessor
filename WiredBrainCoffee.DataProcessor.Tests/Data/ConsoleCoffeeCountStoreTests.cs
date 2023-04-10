using System;
using System.Linq;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Data
{
    public class ConsoleCoffeeCountStoreTests
    {

        [Fact]
        public void ShouldWriteOutputToConsole()
        {
            // Arrange
            var coffeeCountItem = new CoffeeCountItem("Cappuccino", 5);
            var stringWriter = new StringWriter();
            var consoleCoffeeCountStore = new ConsoleCoffeeCountStore(stringWriter);

            // Act
            consoleCoffeeCountStore.Save(coffeeCountItem);

            // Assert
            var result = stringWriter.ToString();
            Assert.Equal($"{coffeeCountItem.CoffeeType}:{coffeeCountItem.Count}{Environment.NewLine}", result);
        }

    }
}
