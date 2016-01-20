using System;
using System.Collections.Generic;
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

namespace Reflux.NET.Example.Controls
{
    /// <summary>
    /// Interaction logic for TodoHeader.xaml
    /// </summary>
    public partial class TodoHeader : UserControl
    {

        public TodoHeader()
        {
            InitializeComponent();
        }

        private void TodoTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(TodoTextBox.Text))
            {
                if (Actions.AddItem != null)
                    Actions.AddItem(TodoTextBox.Text);

                TodoTextBox.Text = string.Empty;
            }
        }
    }
}
