using RevisionPlanner.Controls;
using RevisionPlanner.Models;
using System.ComponentModel;
using CommunityToolkit.Maui.Alerts;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;

namespace RevisionPlanner;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    public static bool ClickLocked { get; private set; } = false;

    public new event PropertyChangedEventHandler PropertyChanged;

    public DatabaseConnector Db { get; set; }

    private readonly LessonsControl _lessonsControl;
    private readonly CategoriesControl _categoriesControl;

    public Color SelectedColor
    {
        get => _selectedColor; set
        {
            _selectedColor = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBrush)));
        }
    }
    private Color _selectedColor = Colors.Red;
    public Brush SelectedBrush { get => new SolidColorBrush(_selectedColor); }

    public MainPage()
    {
        InitializeComponent();
        Db = new DatabaseConnector();
        _lessonsControl = new LessonsControl(this, Db, new AppSettings());
        _mainView.Children.Add(_lessonsControl);
        _categoriesControl = new CategoriesControl(Db);
        _lessonsControl.IsVisible = true;
        _lessonsControl.IsEnabled = true;
        _categoriesControl.IsEnabled = false;
        _categoriesControl.IsVisible = false;
        _mainView.Children.Add(_categoriesControl);

        async void Run() {
            var list = await Db.GetCategoriesAsync();
            list.Insert(0, Category.Default);
            _lessonCategoryPicker.ItemsSource = list;
            _lessonCategoryPicker.SelectedIndex = 0;
        }
        Run();
    }

    private void LessonsButton_Clicked(object sender, EventArgs e)
    {
        if (ClickLocked) return;
        _lessonsControl.IsVisible = true;
        _lessonsControl.IsEnabled = true;
        _categoriesControl.IsEnabled = false;
        _categoriesControl.IsVisible = false;
    }

    private async void PlusButton_Clicked(object sender, EventArgs e)
    {
        if (ClickLocked) return;

        if (_lessonsControl.IsVisible)
        {
            ClickLocked = true;
            var list = await Db.GetCategoriesAsync();
            list.Insert(0, Category.Default);
            _lessonCategoryPicker.ItemsSource = list;
            _lessonCategoryPicker.SelectedIndex = 0;
            _lessonAdd.IsEnabled = true;
            _lessonAdd.IsVisible = true;
            for (var i = 0; i < 10; i++)
            {
                _lessonAdd.Opacity += 0.1;
                await Task.Delay(12);
            }
        }
        else if (_categoriesControl.IsVisible)
        {
            ClickLocked = true;
            _categoryAdd.IsEnabled = true;
            _categoryAdd.IsVisible = true;
            for (var i = 0; i < 10; i++)
            {
                _categoryAdd.Opacity += 0.1;
                await Task.Delay(12);
            }
        }
    }

    private void CategoriesButton_Clicked(object sender, EventArgs e)
    {
        if (ClickLocked) return;
        _lessonsControl.IsVisible = false;
        _lessonsControl.IsEnabled = false;
        _categoriesControl.IsEnabled = true;
        _categoriesControl.IsVisible = true;
    }

    protected override bool OnBackButtonPressed()
    {
        async void Run()
        {
            if (_categoryAdd.IsVisible)
            {
                for (var i = 0; i < 10; i++)
                {
                    _categoryAdd.Opacity -= 0.1;
                    await Task.Delay(12);
                }
                _categoryAdd.IsEnabled = false;
                _categoryAdd.IsVisible = false;
                ClickLocked = false;
            }
            else if(_lessonAdd.IsVisible) {
                for(var i = 0; i < 10; i++)
                {
                    _lessonAdd.Opacity -= 0.1;
                    await Task.Delay(12);
                }
                _lessonAdd.IsEnabled = false;
                _lessonAdd.IsVisible = false;
                ClickLocked = false;

            }
        }

        Run();
        return true;
    }

    private void CategoryName_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (_categoryName is null) return;
        _categoryAddButton.IsEnabled = !string.IsNullOrEmpty(_categoryName?.Text);
    }

    private void LessonName_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (_lessonName is null) return;
        _lessonAddButton.IsEnabled = !string.IsNullOrEmpty(_lessonName?.Text);
    }

    private async void CategoryAddButton_Clicked(object sender, EventArgs e)
    {
        if (_categoryAdd.IsVisible)
        {
            await Db.AddCategoryAsync(new()
            {
                Color = _colorPicker.PickedColor.ToArgbHex(),
                Description = _categoryDescription.Text,
                Name = _categoryName.Text
            });

            _categoriesControl.Reload();

            CancellationTokenSource cancellationTokenSource = new ();

            string text = "The category has been added successfully !";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);


            for (var i = 0; i < 10; i++)
            {
                _categoryAdd.Opacity -= 0.1;
                await Task.Delay(12);
            }
            _categoryAdd.IsEnabled = false;
            _categoryAdd.IsVisible = false;
            ClickLocked = false;
            _categoryDescription.Text = "";
            _categoryName.Text = "";
        }

    }

    private async void LessonAddButton_Clicked(object sender, EventArgs e)
    {
        if (_lessonAdd.IsVisible)
        {
            await Db.AddLessonAsync(new()
            {
                Category = ((Category)_lessonCategoryPicker.SelectedItem).Id,
                Name = _lessonName.Text,
                CreationDate = _lessonDate.Date,
                LastRevised = _lessonDate.Date
            });

            _lessonsControl.Reload();
            _categoriesControl.Reload();

            CancellationTokenSource cancellationTokenSource = new();

            string text = "The lesson has been added successfully !";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);


            for (var i = 0; i < 10; i++)
            {
                _categoryAdd.Opacity -= 0.1;
                await Task.Delay(12);
            }
            _lessonAdd.IsEnabled = false;
            _lessonAdd.IsVisible = false;
            ClickLocked = false;
            _lessonName.Text = "";
        }

    }

    public void Reload()
    {
        _lessonsControl.Reload();
        _categoriesControl.Reload();
    }
}

