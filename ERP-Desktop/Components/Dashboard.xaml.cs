using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ERP_Desktop.DBModels;
using ERP_Desktop.Models;
using ERP_Desktop.Services;
using LiveCharts;
using LiveCharts.Wpf;

namespace ERP_Desktop.Components
{
    public partial class Dashboard : UserControl
    {
        private readonly InvoiceService _invoiceService;
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public Dashboard()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _invoiceService = new InvoiceService(context);

            // Initialize chart settings
            SeriesCollection = new SeriesCollection();
            Labels = Array.Empty<string>();
            YFormatter = value => value.ToString("F2");

            // Load data initially
            LoadDashboardData();
            DataContext = this;
        }

        private async void LoadDashboardData()
        {
            try
            {
                var today = DateTime.Today;
                var monthStart = new DateTime(today.Year, today.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                // Fetch daily and monthly sales reports
                var dailySalesData = await _invoiceService.FetchDailySalesReportAsync(today, today);
                var monthlySalesData = await _invoiceService.FetchDailySalesReportAsync(monthStart, monthEnd);

                // Update dashboard tiles
                UpdateDashboardTiles(dailySalesData, monthlySalesData);

                // Populate the chart with monthly sales data
                UpdateChart(monthlySalesData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void UpdateDashboardTiles(System.Collections.Generic.List<DailySalesReport> dailySalesData, System.Collections.Generic.List<DailySalesReport> monthlySalesData)
        {
            // Calculate totals for daily and monthly data
            var dailySalesTotal = dailySalesData.Sum(s => s.TotalSales);
            var dailyProfitTotal = dailySalesData.Sum(s => s.TotalProfit);
            var monthlySalesTotal = monthlySalesData.Sum(s => s.TotalSales);
            var monthlyProfitTotal = monthlySalesData.Sum(s => s.TotalProfit);

            // Update the text of each TextBlock inside the tiles
            ((TextBlock)((StackPanel)DailySalesTile.Content).Children[1]).Text = dailySalesTotal.ToString("F2");
            ((TextBlock)((StackPanel)DailyProfitTile.Content).Children[1]).Text = dailyProfitTotal.ToString("F2");
            ((TextBlock)((StackPanel)MonthlySalesTile.Content).Children[1]).Text = monthlySalesTotal.ToString("F2");
            ((TextBlock)((StackPanel)MonthlyProfitTile.Content).Children[1]).Text = monthlyProfitTotal.ToString("F2");
        }

        private void UpdateChart(System.Collections.Generic.List<DailySalesReport> monthlySalesData)
        {
            // Clear previous data
            SeriesCollection.Clear();

            // Create a line series for total sales
            var salesSeries = new LineSeries
            {
                Title = "Total Sales",
                Values = new ChartValues<decimal>(monthlySalesData.Select(s => s.TotalSales))
            };

            // Create a line series for total profit
            var profitSeries = new LineSeries
            {
                Title = "Total Profit",
                Values = new ChartValues<decimal>(monthlySalesData.Select(s => s.TotalProfit))
            };

            // Add both series to the SeriesCollection
            SeriesCollection.Add(salesSeries);
            SeriesCollection.Add(profitSeries);

            // Set the Labels for the x-axis to display the dates
            Labels = monthlySalesData.Select(s => s.Date.ToString("dd MMM")).ToArray();

            // Update the chart's axis formatting if needed
            YFormatter = value => value.ToString("F2"); // Format y-axis values to 2 decimal places
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDashboardData();
        }
    }
}
