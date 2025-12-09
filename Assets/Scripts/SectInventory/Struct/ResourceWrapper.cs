using System;
using SectInventory.Enum;

namespace SectInventory.Struct
{
    [Serializable]
    public struct ResourceWrapper {
        public EResourceType type;
        public ResourceData data;
    }
}