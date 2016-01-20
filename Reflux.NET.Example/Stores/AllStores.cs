using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reflux.NET.Example.Stores
{
    public static class AllStores
    {
        private static TodoListStore m_todoListStore;
        public static TodoListStore TodoListStore { get
        {
            return m_todoListStore ?? (m_todoListStore = new TodoListStore());
        }}
    }
}
