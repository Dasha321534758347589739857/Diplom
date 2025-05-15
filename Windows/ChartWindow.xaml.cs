using ChartDirector;
using Diplom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Diplom.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Page
    {
        private List<double> dataX = new();
        private List<double> dataY = new();
        private List<double> dataZ = new();
        private readonly VisualizationViewModel visualizationViewModel;
        public ChartWindow(VisualizationViewModel visualizationViewModel)
        {
            InitializeComponent();
            this.visualizationViewModel = visualizationViewModel;
        }
        private void WPFChartViewer1_ViewPortChanged(object sender, WPFViewPortEventArgs e)
        {
            if (e.NeedUpdateChart)
                drawChart((WPFChartViewer)sender);
        }

        public void drawChart(WPFChartViewer viewer)
        {
            XYChart _chart = new XYChart(600, 600);
            _chart.addTitle("2D объекты", "Arial Italic", 15);
            _chart.setPlotArea(65, 40, 360, 360, -1, -1, -1, unchecked((int)0xc0000000), -1);

            _chart.xAxis().setTitle("X", "Arial Bold Italic", 10);
            _chart.yAxis().setTitle("Y", "Arial Bold Italic", 10);
            _chart.xAxis().setLabelStyle("Arial Bold");
            _chart.yAxis().setLabelStyle("Arial Bold");
            _chart.xAxis().setColors(Chart.Transparent);
            _chart.yAxis().setColors(Chart.Transparent);

            ContourLayer layer = _chart.addContourLayer(
                dataX.ToArray(),
                dataY.ToArray(),
                dataZ.ToArray()
            );
            _chart.getPlotArea().moveGridBefore(layer);
            ColorAxis cAxis = layer.setColorAxis(245, 455, Chart.TopCenter, 330, Chart.Top);
            cAxis.setBoundingBox(Chart.Transparent, Chart.LineColor);
            cAxis.setTitle("Радиусы поражения", "Arial Bold Italic", 10);
            cAxis.setLabelStyle("Arial Bold");
            cAxis.setLinearScale(0, 105, 5);
            viewer.Chart = _chart;
            viewer.ImageMap = _chart.getHTMLImageMap(
                "",
                "",
                "title='<*cdml*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}'"
            );
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var data = visualizationViewModel.ParseData();
            dataX = data.DataX;
            dataY = data.DataY;
            dataZ = data.DataZ;
            WPFChartViewer1.updateViewPort(true, false);
        }

    }
}
