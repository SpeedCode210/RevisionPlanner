using RevisionPlanner.Controls;
using RevisionPlanner.Models;

namespace RevisionPlanner;

public partial class MainPage : ContentPage
{
    public DatabaseConnector Db { get; set; }

    private readonly LessonsControl _lessonsControl;
    private readonly CategoriesControl _categoriesControl; 

    public MainPage()
	{
		InitializeComponent();
        Db = new DatabaseConnector();
        _lessonsControl = new LessonsControl(Db, new AppSettings());
        _mainView.Children.Add(_lessonsControl);
        _categoriesControl = new CategoriesControl(Db);
        _lessonsControl.IsVisible = true;
        _lessonsControl.IsEnabled = true;
        _categoriesControl.IsEnabled = false;
        _categoriesControl.IsVisible = false;
        _mainView.Children.Add(_categoriesControl);

    }

    private void LessonsButton_Clicked(object sender, EventArgs e)
    {
        _lessonsControl.IsVisible = true;
        _lessonsControl.IsEnabled = true;
        _categoriesControl.IsEnabled = false;
        _categoriesControl.IsVisible = false;
    }

    private void PlusButton_Clicked(object sender, EventArgs e)
    {

    }

    private void CategoriesButton_Clicked(object sender, EventArgs e)
    {
        _lessonsControl.IsVisible = false;
        _lessonsControl.IsEnabled = false;
        _categoriesControl.IsEnabled = true;
        _categoriesControl.IsVisible = true;
    }
}

