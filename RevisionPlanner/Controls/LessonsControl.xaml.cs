using RevisionPlanner.Models;

namespace RevisionPlanner.Controls;

public partial class LessonsControl : Grid
{
	private readonly DatabaseConnector _db;
	private readonly MainPage _parent;
    private readonly AppSettings _settings;
	public LessonsControl(MainPage page, DatabaseConnector db, AppSettings settings)
	{
		InitializeComponent();
		_parent = page;
		_db = db;
		_settings = settings;
        Reload();
	}

	public async void Reload()
	{
		var lessons = await _db.GetLessonsAsync();
		var categories = await _db.GetCategoriesAsync();
		_scroll.ItemsSource = lessons.GetDisplay(categories, _settings.TrainingSchedule);
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        if (MainPage.ClickLocked) return;
        var btn = (ImageButton)sender;
		var dispLesson = (DisplayLesson)btn.BindingContext;
		dispLesson.Lesson.LastRevised = DateTime.Now;
		await _db.UpdateLessonAsync(dispLesson.Lesson);
		Reload();
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (MainPage.ClickLocked) return;
        var btn = (ImageButton)sender;
        var dispLesson = (DisplayLesson)btn.BindingContext;
        bool answer = await _parent.DisplayAlert("Lesson Removal", "Would you really want to remove the lesson?", "Yes", "No");
		if (answer)
		{
			await _db.DeleteLessonAsync(dispLesson.Lesson);
            _parent.Reload();
        }
    }
}