using ChartDirector;
using Diplom.ViewModels;
using Microsoft.Extensions.Primitives;
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
            int z=0;
            int wight=0;
            int height = 0;

            if (visualizationViewModel.height == 0.0 || visualizationViewModel.weight == 0.0)
            {
                MessageBox.Show("Параметры объекта заданы неверно.");
            }
            else {

                try
                {
                    z = (int)(visualizationViewModel.height / visualizationViewModel.weight);
                    wight = 650 / z;
                    height = 650;
                }
                catch (DivideByZeroException)
                {
                    z = (int)(visualizationViewModel.weight / visualizationViewModel.height);
                    wight = 400;
                    height = 400 / z;
                }
                catch (Exception)
                {
                    MessageBox.Show("Параметры объекта заданы неверно.");
                }



                XYChart _chart = new XYChart(800, 800);
                _chart.addTitle("2D объекты", "Arial Italic", 15);
                _chart.setPlotArea(40, 40, height, wight, -1, -1, -1, unchecked((int)0xc0000000), -1);

                _chart.xAxis().setTitle("X, м", "Arial Bold Italic", 10);
                _chart.yAxis().setTitle("Y, м", "Arial Bold Italic", 10);
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
                ColorAxis cAxis = layer.setColorAxis(370, 100 + wight, Chart.TopCenter, 600, Chart.Top);
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
