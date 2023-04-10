

using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing
{
    public class FakeCoffeeCountStore : ICoffeeCountStore
    {

        public void Save(CoffeeCountItem item)
        {
            SavedItems.Add(item);
        }

        public List<CoffeeCountItem> SavedItems { get; } = new();

    }
}
