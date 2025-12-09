using System.Collections.Generic;
using SectInventory.Enum;
using SectInventory.Struct;


namespace SectInventory {
    public class SectInventory {
        private Dictionary<EResourceType, ResourceData> _inventory;

        public SectInventory(List<ResourceWrapper> _inventoryWrappers) {
            _inventory = new Dictionary<EResourceType, ResourceData>();
            foreach (ResourceWrapper wrapper in _inventoryWrappers) {
                _inventory.TryAdd(wrapper.type, wrapper.data);
            }
        }

        public int TryAddingToInventory(EResourceType _resourceType, int _amount) {
            int _amountToReturn = -1;
            ResourceData _data = _inventory[_resourceType];
            
            if (_data.amount + _amount > _data.maxAmount) {
                _amountToReturn = _amount - (_data.maxAmount -  _data.amount);
                _data.amount = _data.maxAmount;
            } else {
                _data.amount += _amount;
                _amountToReturn = 0;
            }
            
            _inventory[_resourceType] = _data;
            return _amountToReturn;
        }

        public bool TryGettingFromInventory(EResourceType _resourceType, int _amount) {
            if (!_inventory.TryGetValue(_resourceType, out var _value)) return false;
            if(_value.amount < _amount) return false;
            
            ResourceData _data = _inventory[_resourceType];
            _data.amount -= _amount;
            _inventory[_resourceType] = _data;
            
            return true;
        }
    }
}

