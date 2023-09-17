using RevisionPlanner.Models;

namespace RevisionPlanner.Controls;

public partial class CategoriesControl : Grid
{
	private readonly DatabaseConnector _db;
	public CategoriesControl(DatabaseConnector db)
	{
		InitializeComponent();
		_db = db;
        Reload();
	}

	public async void Reload()
	{
        var lessons = await _db.GetLessonsAsync();
        var categories = await _db.GetCategoriesAsync();
		_scroll.ItemsSource = categories.GetDisplay(lessons);
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (MainPage.ClickLocked) return;
        var btn = (ImageButton)sender;
        var dispCat = (DisplayCategory)btn.BindingContext;
        await _db.DeleteCategoryAsync(dispCat.Category);
        Reload();

    }
}