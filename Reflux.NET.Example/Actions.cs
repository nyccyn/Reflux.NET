using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reflux.NET.Example
{
    public static class Actions
    {

        public static Action<string> AddItem;

        public static Action<int, string> EditItem;

        public static Action<int> RemoveItem;

        public static Action<int> ToggleItem;

        public static Action<bool> ToggleAllItems;

        public static Action ClearCompleted;

    }
}
