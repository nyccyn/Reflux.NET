# Reflux .NET
This is a simple implementation of Reflux in .NET framework and WPF.
Implemnting reflux in .NET is actually pretty straight forward because we can use build-in concepts like Action<T> and events.
- [Actions](#actions)
- [Stores](#stores)
- [Components](#components)

### Actions
So we don't need to do anything special to create our actions, just create a static Actions.cs class that have Action<T> (or just Action) members.
```c#
public static class Actions
{
    public static Action<string> AddItem;
    public static Action<int, string> EditItem;
    public static Action<int> RemoveItem;
    public static Action<int> ToggleItem;
    public static Action<bool> ToggleAllItems;
    public static Action ClearCompleted;
}
```

Now we can simply register to those actions and trigger them.
```c#
// Register to an action
Actions.AddItem += OnAddItem;
private void OnAddItem(string label)
{
    ...
}

// Trigger an action
if (Actions.AddItem != null)
    Actions.AddItem(TodoTextBox.Text);
```

### Stores
To create a store I created a base generic class (the generic type is the store's state) that give us all the basics functions: Listen, Trigger and GetInitialState
```c#
public abstract class Store<T>
{
    private event Action<T> StoreTriggered;
    protected void Trigger(T state)
    {
        if (StoreTriggered != null)
            StoreTriggered(state);
    }
    public void Listen(Action<T> callback)
    {
        StoreTriggered += callback;
    }
    public abstract T GetInitialState();
}
```
And now we just need to inherit from Store<T>:
```c#
public class TodoListStore : Store<List<TodoItem>>
{
    private int m_todoCounter = 0;
    private List<TodoItem> m_todoItems;
    
    public TodoListStore()
    {            
        m_todoItems = new List<TodoItem>();
        // Register to an action
        Actions.AddItem += OnAddItem;
        ...
    }
    
    private void OnAddItem(string label)
    {
        ...
        // Trigger the store state
        Trigger(m_todoItems);
    }
    
    public override List<TodoItem> GetInitialState()
    {
        return m_todoItems;
    }
}
```
And we can listen to the store's state changed event.
```c#
todoListStore.Listen(OnTodoListStoreChanged);
private void OnTodoListStoreChanged(List<TodoItem> todoItems)
{
    ...
}
```

### Components
To keep the components concept of React I used UserControls with code behind (which is big no-no when working with MVVM).

The equivalent to React compenets' state in WPF is DataContext, so we can create a specific class that implment INotifyPropertyChanged and attach it to our UserControl's DataContext. When we bind a UI Control to one of the DataContext's properties we need to change the mode, like in React, to OneWay binding.

To trigger a change from the UI Control we can simply use built-in events.
```c#
public partial class TodoItemControl : UserControl
{       
    private int m_id;
    private TodoItemContext m_todoItemContext;

    public TodoItemControl()
    {
        InitializeComponent();
        // Set the context class
        m_todoItemContext = new TodoItemContext();
        DataContext = m_todoItemContext;
    }

    public int Id
    {
        get { return m_id; }
        set { m_id = value; }
    }
    
    public string Label
    { 
        // Update the UserControl "state"
        set { m_todoItemContext.Label = value; }
    }

    public bool IsCompleted
    {
        set { m_todoItemContext.IsCompleted = value; }            
    }

    // Register to UI Control event and trigger an action
    private void ButtonRemove_OnClick(object sender, RoutedEventArgs e)
    {
        if (Actions.RemoveItem != null)
            Actions.RemoveItem(m_id);
    }

    private void LabelTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ...
    }

    private void IsCompletedCheckBox_Click(object sender, RoutedEventArgs e)
    {
       ...
    }
}
// The context class
public class TodoItemContext : INotifyPropertyChanged
{
    private string m_label;
    public string Label
    {
        get { return m_label; }
        set
        {
            if (m_label != value)
            {
                // Trigger NotifyPropertyChanged
                m_label = value;
                NotifyPropertyChanged("Label");
            }
        }
    }

    private bool m_isCompleted;
    public bool IsCompleted
    {
        ...
    }
}

// In the XAML it's important to change the binding mode to one way
<TextBox x:Name="LabelTextBox" TextChanged="LabelTextBox_OnTextChanged" Text="{Binding Label, Mode=OneWay}"/>
```