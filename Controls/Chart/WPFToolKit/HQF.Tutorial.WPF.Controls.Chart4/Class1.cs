using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using myTuple = System.Tuple<int, System.Windows.Point>;

namespace HQF.Tutorial.WPF.Controls.Chart4
{
    public class Zoom
    {

        private Rectangle _Rectangle = null;
        private myTuple _PointA = null;
        private myTuple _PointB = null; // only used for the fingers, not for the mouse
        private readonly Chart _Chart;

        public Zoom(Chart xChart)
        {
            _Chart = xChart;
        } //

        #region draw new rectangle on button or finger down

        public void OnMouseLeftButtonDown(object xSender, MouseButtonEventArgs e)
        {
            Canvas lCanvas = xSender as Canvas;
            if (lCanvas == null) return;
            Point lPointA = e.GetPosition(lCanvas);

            DrawNewRectangle(lCanvas, lPointA, -1);
        } //

        public void OnTouchDown(object xSender, TouchEventArgs e)
        {
            Canvas lCanvas = xSender as Canvas;
            if (lCanvas == null) return;

            TouchPoint lTouchPoint = e.GetTouchPoint(lCanvas);
            if (lTouchPoint == null) return;

            myTuple lTuple = _PointA;
            if (lTuple != null)
            {
                if (lTuple.Item1 == e.TouchDevice.Id) return; // this was finger 1, not going to happen anyway as it cannot touchdown twice
                Point lPointA = lTuple.Item2;

                // store second finger; we don't care about its ID, so it could also be finger 3, 4 or 5 ...
                Point lPointB = lTouchPoint.Position;
                _PointB = new myTuple(e.TouchDevice.Id, lPointB);
                RedrawRectangle(lPointA, lPointB);
                return;
            }

            // first finger
            DrawNewRectangle(lCanvas, lTouchPoint.Position, lTouchPoint.TouchDevice.Id);
            return;
        } //

        private void DrawNewRectangle(Canvas xCanvas, Point xPoint, int xPointId)
        {
            if (_Rectangle != null) return;

            _Rectangle = new Rectangle();

            Point lPointA = new Point(xPoint.X, xPoint.Y); // clone
            _PointA = new myTuple(xPointId, lPointA);
            xCanvas.Children.Add(_Rectangle);

            Canvas.SetLeft(_Rectangle, xPoint.X);
            Canvas.SetTop(_Rectangle, xPoint.Y);

            _Rectangle.Height = 1.0;
            _Rectangle.Width = 1.0;
            _Rectangle.Opacity = 0.3;
            _Rectangle.Fill = new SolidColorBrush(Colors.SteelBlue);
            _Rectangle.Stroke = new SolidColorBrush(Colors.DarkBlue);
            _Rectangle.StrokeDashArray = new DoubleCollection(new double[] { 0.5, 1.5 });
            _Rectangle.StrokeDashCap = PenLineCap.Round;
            _Rectangle.StrokeThickness = 2.0;
        } //
        #endregion


        #region resize rectangle on any movement

        public void OnMouseMove(object xSender, MouseEventArgs e)
        {
            if (!(xSender is Canvas)) return;
            myTuple lTuple = _PointA;
            if (lTuple == null) return;
            Point lPointA = lTuple.Item2;
            Point lPointB = e.GetPosition((Canvas)xSender);
            RedrawRectangle(lPointA, lPointB);
        } //

        DateTime _LastOnTouchMove;
        public void OnTouchMove(object xSender, TouchEventArgs e)
        {
            if (DateTime.Now.Subtract(_LastOnTouchMove).TotalMilliseconds < 300) return;  // throttle
            _LastOnTouchMove = DateTime.Now;

            Canvas lCanvas = xSender as Canvas;
            if (lCanvas == null) return;

            TouchPoint lTouchPoint = e.GetTouchPoint(lCanvas);
            if (lTouchPoint == null) return;

            myTuple lTuple = _PointA;
            if (lTuple == null) return;

            Point lPointA = lTuple.Item2;
            if (e.TouchDevice.Id == lTuple.Item1)
            {
                // this is the finger we were touching down first
                lPointA = lTouchPoint.Position;
                _PointA = new myTuple(e.TouchDevice.Id, lPointA);
            }

            lTuple = _PointB;
            if (lTuple == null) return; // no second finger

            Point lPointB = lTuple.Item2;
            if (e.TouchDevice.Id == lTuple.Item1)
            {
                // this was the second finger
                lPointB = lTouchPoint.Position;
                _PointB = new myTuple(e.TouchDevice.Id, lPointB);
            }

            RedrawRectangle(lPointA, lPointB);
        } //

        private void RedrawRectangle(Point xPointA, Point xPointB)
        {
            Rectangle lRectangle = _Rectangle;
            if (lRectangle == null) return;

            Canvas.SetTop(lRectangle, Math.Min(xPointA.Y, xPointB.Y));
            lRectangle.Height = Math.Abs(xPointA.Y - xPointB.Y);

            Canvas.SetLeft(lRectangle, Math.Min(xPointA.X, xPointB.X));
            lRectangle.Width = Math.Abs(xPointA.X - xPointB.X);
        } //

        #endregion

        #region remove rectangle and Zoom
        public void OnMouseLeftButtonUp(object xSender, MouseButtonEventArgs e)
        {
            Rectangle lRectangle = _Rectangle;
            RemoveRectangle(xSender);
            ProcessZoom(lRectangle);
        } //

        public void OnTouchUp(object xSender, TouchEventArgs e)
        {
            Rectangle lRectangle = _Rectangle;
            if (lRectangle == null) return; // do not process any results when the first finger was gone already
            RemoveRectangle(xSender);
            ProcessZoom(lRectangle);
        } //

        private void RemoveRectangle(object xSender)
        {
            Canvas lCanvas = xSender as Canvas;
            if (lCanvas == null) return;
            lCanvas.Children.Remove(_Rectangle);
            _PointA = null;
            _PointB = null;
            _Rectangle = null;
        } //

        public void ProcessZoom(Rectangle xRectangle)
        {
            if (xRectangle == null) return;
            Point lFrom = new Point(Canvas.GetLeft(xRectangle), Canvas.GetTop(xRectangle));
            Point lTo = new Point(lFrom.X + xRectangle.Width, lFrom.Y + xRectangle.Height);

            foreach (IAxis lAxis in _Chart.ActualAxes)
            {
                if (lAxis is LinearAxis)
                {
                    LinearAxis lLinearAxis = lAxis as LinearAxis;
                    AxisPointLinear a = AxisPointFactory.getAxisPoint(_Chart, lLinearAxis, lFrom) as AxisPointLinear;
                    AxisPointLinear b = AxisPointFactory.getAxisPoint(_Chart, lLinearAxis, lTo) as AxisPointLinear;
                    lLinearAxis.Minimum = Math.Min(a.MouseAxisValueAbsolute, b.MouseAxisValueAbsolute);
                    lLinearAxis.Maximum = Math.Max(a.MouseAxisValueAbsolute, b.MouseAxisValueAbsolute);
                    continue;
                }

                if (lAxis is DateTimeAxis)
                {
                    DateTimeAxis lDateTimeAxis = lAxis as DateTimeAxis;
                    AxisPointDateTime a = AxisPointFactory.getAxisPoint(_Chart, lDateTimeAxis, lFrom) as AxisPointDateTime;
                    AxisPointDateTime b = AxisPointFactory.getAxisPoint(_Chart, lDateTimeAxis, lTo) as AxisPointDateTime;
                    lDateTimeAxis.Minimum = a.MouseAxisValueAbsolute < b.MouseAxisValueAbsolute ? a.MouseAxisValueAbsolute : b.MouseAxisValueAbsolute;
                    lDateTimeAxis.Maximum = a.MouseAxisValueAbsolute > b.MouseAxisValueAbsolute ? a.MouseAxisValueAbsolute : b.MouseAxisValueAbsolute;
                    continue;
                }
            }
        } //
        #endregion

        #region reset Zoom
        public void OnKeyDown(object xSender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;

            ProcessZoomReset();
        } // 

        public void ProcessZoomReset()
        {
            foreach (IAxis lAxis in _Chart.ActualAxes)
            {
                if (lAxis is LinearAxis)
                {
                    LinearAxis lLinearAxis = lAxis as LinearAxis;
                    lLinearAxis.Minimum = null;
                    lLinearAxis.Maximum = null;
                    continue;
                }

                if (lAxis is DateTimeAxis)
                {
                    DateTimeAxis lDateTimeAxis = lAxis as DateTimeAxis;
                    lDateTimeAxis.Minimum = null;
                    lDateTimeAxis.Maximum = null;
                    continue;
                }
            }
        } //
        #endregion

    } // class
}
