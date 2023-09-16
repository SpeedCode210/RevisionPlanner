using RevisionPlanner.Models;

namespace RevisionPlanner.Controls;

public partial class LessonsControl : Grid
{
	private readonly DatabaseConnector _db;
	private readonly AppSettings _settings;
	public LessonsControl(DatabaseConnector db, AppSettings settings)
	{
		InitializeComponent();
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
		var btn = (ImageButton)sender;
		var dispLesson = (DisplayLesson)btn.BindingContext;
		dispLesson.Lesson.LastRevised = DateTime.Now;
		await _db.UpdateLessonAsync(dispLesson.Lesson);
		Reload();
    }
}