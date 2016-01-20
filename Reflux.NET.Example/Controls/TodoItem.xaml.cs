using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using Reflux.NET.Example.Annotations;
using Reflux.NET.Example.Stores;

namespace Reflux.NET.Example.Controls
{
    /// <summary>
    /// Interaction logic for TodoItemControl.xaml
    /// </summary>
    public partial class TodoItemControl : UserControl
    {       
        private int m_id;
        private TodoItemContext m_todoItemContext;

        public TodoItemControl()
        {
            InitializeComponent();
            m_todoItemContext = new TodoItemContext();
            DataContext = m_todoItemContext;
        }

        #region Properties

        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }
        
        public string Label
        { 
            set { m_todoItemContext.Label = value; }
        }

        public bool IsCompleted
        {
            set { m_todoItemContext.IsCompleted = value; }            
        }

        #endregion


        private void ButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            if (Actions.RemoveItem != null)
                Actions.RemoveItem(m_id);
        }

        private void LabelTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Actions.EditItem != null && !string.IsNullOrEmpty(LabelTextBox.Text))
                Actions.EditItem(m_id, LabelTextBox.Text);
        }

        private void IsCompletedCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (Actions.ToggleItem != null)
                Actions.ToggleItem(m_id);
        }
    }

    public class TodoItemContext : ContextBase
    {
       
        private string m_label;
        public string Label
        {
            get { return m_label; }
            set
            {
                if (m_label != value)
                {
                    m_label = value;
                    NotifyPropertyChanged("Label");
                }
            }
        }

        private bool m_isCompleted;

        public bool IsCompleted
        {
            get { return m_isCompleted; }
            set
            {
                if (m_isCompleted != value)
                {
                    m_isCompleted = value;
                    NotifyPropertyChanged("IsCompleted");
                }
            }
        }
    }
}
