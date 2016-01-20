using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Reflux.NET.Example.Stores;

namespace Reflux.NET.Example.Controls
{
    /// <summary>
    /// Interaction logic for TodoMain.xaml
    /// </summary>
    public partial class TodoMain : UserControl
    {
        private TodoMainContext m_todoMainContext;

        public TodoMain()
        {
            InitializeComponent();            
            m_todoMainContext = new TodoMainContext();
            DataContext = m_todoMainContext;
            AllStores.TodoListStore.Listen(OnTodoListStoreChanged);
        }

        private void OnTodoListStoreChanged(List<TodoItem> todoItems)
        {
            // Remove items that didn't come from the list
            var itemsToRemove = m_todoMainContext.TodoItems.Where(i => !todoItems.Select(ti => ti.Id).Contains(i.Id)).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                m_todoMainContext.TodoItems.Remove(itemToRemove);
            }

            // Add or modify all others
            foreach (var todoItem in todoItems)
            {
                var todoItemControl = m_todoMainContext.TodoItems.FirstOrDefault(i => i.Id == todoItem.Id);
                if (todoItemControl == null)
                {
                    m_todoMainContext.TodoItems.Add(new TodoItemControl
                    {
                        Id = todoItem.Id,
                        Label = todoItem.Label,
                        IsCompleted = todoItem.IsCompleted
                    });
                }
                else
                {
                    todoItemControl.Label = todoItem.Label;
                    todoItemControl.IsCompleted = todoItem.IsCompleted;
                }
            }
        }

        private void ToggleAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (Actions.ToggleAllItems != null && ToggleAllCheckBox.IsChecked.HasValue)
                Actions.ToggleAllItems(ToggleAllCheckBox.IsChecked.Value);
        }
    }

    public class TodoMainContext : ContextBase
    {
        private ObservableCollection<TodoItemControl> m_todoItemControls;
        public ObservableCollection<TodoItemControl> TodoItems
        {
            get { return m_todoItemControls ?? (m_todoItemControls = new ObservableCollection<TodoItemControl>()); }
        }
    }
}
