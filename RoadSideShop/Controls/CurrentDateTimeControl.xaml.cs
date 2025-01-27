namespace RoadSideShop.Controls;

public partial class CurrentDateTimeControl : ContentView, IDisposable
{

    private readonly PeriodicTimer _timer;
    public CurrentDateTimeControl()
    {
        InitializeComponent();
        dayTimeLabel.Text = DateTime.Now.ToString("dddd, hh:mm:ss tt");
        dateLabel.Text = DateTime.Now.ToString("MMM dd, yyyy");

        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        UpdateTimer();


    }

    public void Dispose() => _timer?.Dispose();


    private async void UpdateTimer()
    {
        while (await _timer.WaitForNextTickAsync())
        {
            dayTimeLabel.Text = DateTime.Now.ToString("dddd, hh:mm:ss tt");
            dateLabel.Text = DateTime.Now.ToString("MMM dd, yyyy");
        }
    }
}
