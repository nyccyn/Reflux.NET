using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RefluxNET;

namespace Reflux.NET.Example.Stores
{
    public class TodoListStore : Store<List<TodoItem>>
    {
        private int m_todoCounter = 0;
        private List<TodoItem> m_todoItems;

        public TodoListStore()
        {            
            m_todoItems = new List<TodoItem>();
            Actions.AddItem += OnAddItem;
            Actions.EditItem += OnEditItem;
            Actions.RemoveItem += OnRemoveItem;
            Actions.ClearCompleted += OnClearCompleted;
            Actions.ToggleItem += OnToggleItem;
            Actions.ToggleAllItems += OnToggleAllItems;
        }

        private void OnAddItem(string label)
        {
            m_todoItems.Add(new TodoItem
            {
                Id = m_todoCounter,
                Created = DateTime.Now,
                IsCompleted = false,
                Label = label
            });
            m_todoCounter++;
            Trigger(m_todoItems);
        }

        private void OnEditItem(int id, string label)
        {
            var item = m_todoItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.Label = label;
                Trigger(m_todoItems);
            }
        }

        private void OnRemoveItem(int id)
        {
            var item = m_todoItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                m_todoItems.Remove(item);
                Trigger(m_todoItems);
            }
        }

        private void OnClearCompleted()
        {
            var completedItems = m_todoItems.Where(i => i.IsCompleted);
            foreach (var completedItem in completedItems)
            {
                m_todoItems.Remove(completedItem);
            }
            Trigger(m_todoItems);
        }

        private void OnToggleAllItems(bool isCompleted)
        {
            foreach (var todoItem in m_todoItems)
            {
                todoItem.IsCompleted = isCompleted;
            }
            Trigger(m_todoItems);
        }

        private void OnToggleItem(int id)
        {
            var item = m_todoItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.IsCompleted = !item.IsCompleted;
                Trigger(m_todoItems);
            }
        }

        public override List<TodoItem> GetInitialState()
        {
            return m_todoItems;
        }

    }
}
