namespace MauiApp2;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

public partial class MainPage : ContentPage
{
    private ObservableCollection<ToDoClass> items = new();
    private ToDoClass? selectedItem;
    private int nextId = 1;

    public MainPage()
    {
        InitializeComponent();
        todoLV.ItemsSource = items;
    }

    private void AddToDoItem(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text))
            return;

        items.Add(new ToDoClass
        {
            Id = nextId++,
            Title = titleEntry.Text,
            Detail = detailsEditor.Text
        });

        ClearInputs();
    }

    private void EditToDoItem(object sender, EventArgs e)
    {
        if (selectedItem == null)
            return;

        selectedItem.Title = titleEntry.Text;
        selectedItem.Detail = detailsEditor.Text;

        ResetEditMode();
    }

    private void CancelEdit(object sender, EventArgs e)
    {
        ResetEditMode();
    }

    private void DeleteToDoItem(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            var item = items.FirstOrDefault(x => x.Id == id);
            if (item != null)
                items.Remove(item);
        }
    }

    private void TodoLV_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        selectedItem = e.SelectedItem as ToDoClass;

        if (selectedItem == null)
            return;

        titleEntry.Text = selectedItem.Title;
        detailsEditor.Text = selectedItem.Detail;

        addBtn.IsVisible = false;
        editBtn.IsVisible = true;
        cancelBtn.IsVisible = true;
    }

    private void ResetEditMode()
    {
        selectedItem = null;
        todoLV.SelectedItem = null;

        addBtn.IsVisible = true;
        editBtn.IsVisible = false;
        cancelBtn.IsVisible = false;

        ClearInputs();
    }

    private void ClearInputs()
    {
        titleEntry.Text = string.Empty;
        detailsEditor.Text = string.Empty;
    }
}

public class ToDoClass : INotifyPropertyChanged
{
    private int _id;
    private string? _title;
    private string? _detail;

    public int Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public string? Title
    {
        get => _title;
        set => SetField(ref _title, value);
    }

    public string? Detail
    {
        get => _detail;
        set => SetField(ref _detail, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}