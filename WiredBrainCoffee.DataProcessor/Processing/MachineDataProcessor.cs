using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing
{
    public class MachineDataProcessor
    {

        private readonly Dictionary<string, int> _countPerCoffeeType = new();
        private readonly ICoffeeCountStore coffeeCountStore;
        private MachineDataItem? previousItem;

        public MachineDataProcessor(ICoffeeCountStore coffeeCountStore)
        {
            this.coffeeCountStore = coffeeCountStore;
        }

        private bool IsNewerThanPreviousItem(MachineDataItem dataItem)
        {
            return previousItem is null || previousItem.CreatedAt < dataItem.CreatedAt;
        }

        private void ProcessItem(MachineDataItem dataItem)
        {
            if (!IsNewerThanPreviousItem(dataItem))
            {
                return;
            }

            if (!_countPerCoffeeType.ContainsKey(dataItem.CoffeeType))
            {
                _countPerCoffeeType.Add(dataItem.CoffeeType, 1);
            }
            else
            {
                _countPerCoffeeType[dataItem.CoffeeType]++;
            }

            previousItem = dataItem;
        }

        private void SaveCountPerCoffeeType()
        {
            foreach (var entry in _countPerCoffeeType)
            {
                coffeeCountStore.Save(new CoffeeCountItem(entry.Key, entry.Value));
            }
        }

        public void ProcessItems(MachineDataItem[] dataItems)
        {
            previousItem = null;
            _countPerCoffeeType.Clear();

            foreach (var dataItem in dataItems)
            {
                ProcessItem(dataItem);
            }

            SaveCountPerCoffeeType();
        }

    }
}
