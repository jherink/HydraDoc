﻿using Stitch.Chart.Axis;
using Stitch.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    public class ScatterGroup<T1, T2> : List<ScatterPoint<T1, T2>>, ICloneable where T1 : IComparable<T1>
                                                                               where T2 : IComparable<T2>
    {
        public readonly string Name;

        public string Color { get; private set; }

        public ScatterGroup( string name ) { Name = name; }

        public void SetColor( string color )
        {
            Color = color;
            foreach (var pt in this) pt.Color = color;
        }

        public object Clone()
        {
            var clone = new ScatterGroup<T1, T2>( Name );
            foreach (var pt in this) clone.Add( pt.Clone() as ScatterPoint<T1, T2> );
            return clone;
        }
    }

    public class ScatterPoint<T1, T2> : ICloneable where T1 : IComparable<T1>
                                                   where T2 : IComparable<T2>
    {
        public T1 LabeledValue { get; set; }
        public T2 MeasuredValue { get; set; }
        public string Color { get; set; }
        public ScatterPoint() { }
        public ScatterPoint( T1 labeledValue, T2 measuredValue )
        {
            LabeledValue = labeledValue;
            MeasuredValue = measuredValue;
        }

        public object Clone()
        {
            return new ScatterPoint<T1, T2>( LabeledValue, MeasuredValue );
        }
    }

    public class ScatterChart<T1, T2> : AxisChart<T1, T2> where T1 : IComparable<T1>
                                                          where T2 : IComparable<T2>
    {

        private Dictionary<string, ScatterGroup<T1, T2>> ScatterGroups = new Dictionary<string, ScatterGroup<T1, T2>>();

        #region Constructors

        public ScatterChart() : this( 900, 500 )
        {

        }

        public ScatterChart( int width, int height ) : base( width, height )
        {
        }

        #endregion

        #region Public Methods

        public void AddPoint( string barGroup, T1 labeledValue, T2 measuredValue, string color = "" )
        {
            AddPoint( barGroup, new ScatterPoint<T1, T2>( labeledValue, measuredValue ) { Color = color } );
        }

        public void AddPoint( string barGroup, ScatterPoint<T1, T2> point )
        {
            GetScatterGroup( barGroup ).Add( point );
        }

        public void SetScatterGroupColor( string scatterGroup, string color )
        {
            GetScatterGroup( scatterGroup ).SetColor( color );
        }

        public ScatterGroup<T1, T2> GetScatterGroup( string scatterGroup )
        {
            if (!ScatterGroups.ContainsKey( scatterGroup ))
            {
                ScatterGroups.Add( scatterGroup, new ScatterGroup<T1, T2>( scatterGroup ) );
            }
            return ScatterGroups[scatterGroup];
        }

        #endregion

        protected override void RenderAxisChartImpl()
        {
            // labeled axis is on the bottom.
            var horizontalAxisLocations = new double[] { };
            var verticalAxisLocations = new double[] { };
            var horizontalIntervals = AxisHelper.SuggestIntervals( GetChartableAreaWidth() );
            var verticalIntervals = AxisHelper.SuggestIntervals( Height );
            //Sort();
            var horizontalSet = GetHorizontalSet().ToList();
            var verticalSet = GetVerticalSet().ToList();
            if (LabeledAxis.IncludeDefault) horizontalSet.Insert( 0, default( T1 ) );
            if (MeasuredAxis.IncludeDefault) verticalSet.Insert( 0, default( T2 ) );
            LabeledAxis.SetTicks( LabeledAxisTickAlgorithm.SuggestTicks( horizontalSet, horizontalIntervals ) );
            MeasuredAxis.SetTicks( MeasuredAxisTickAlgorithm.SuggestTicks( verticalSet, verticalIntervals ) );

            var horizontalClone = LabeledAxis.Clone() as IAxis<T1>;

            // Because of how SVG renders we must reverse the measured values first.
            var measuredClone = MeasuredAxis.Clone() as IAxis<T2>;
            measuredClone.SetTicks( measuredClone.Ticks.Reverse() );

            AxisHelper.RenderAxis( measuredClone, horizontalClone, GetChartableAreaWidth(), Height - GetLegendBottomOffset() - GetLegendTopOffset(),
                                       GetLegendLeftOffset(), GetTitleHeight() + GetLegendTopOffset(),
                                       VerticalAxisGroup, HorizontalAxisGroup,
                                       out verticalAxisLocations, out horizontalAxisLocations );

            var baseLineY = verticalAxisLocations.Max();
            var baseLineX = horizontalAxisLocations.Min();
            var horizSpace = verticalAxisLocations.Min();
            var chartableWidth = horizontalAxisLocations.Max() - horizontalAxisLocations.Min();
            var chartableHeight = verticalAxisLocations.Max() - verticalAxisLocations.Min();
            var i = 1;
            foreach (var group in ScatterGroups)
            {
                var svgGroup = new SVGGroup();
                foreach (var point in group.Value)
                {                    
                    var x = LabeledAxisTickAlgorithm.Subtract( horizontalSet, point.LabeledValue, LabeledAxisTickAlgorithm.Min( horizontalSet ) );
                    var y = MeasuredAxisTickAlgorithm.Subtract( verticalSet, point.MeasuredValue, MeasuredAxisTickAlgorithm.Min( verticalSet ) );
                    var pctX = LabeledAxisTickAlgorithm.Percentage( horizontalSet, point.LabeledValue );
                    var pctY = MeasuredAxisTickAlgorithm.Percentage( verticalSet, point.MeasuredValue );
                    var px = baseLineX + pctX * chartableWidth;
                    var py = (chartableHeight - pctY * chartableHeight) + horizSpace;

                    var svgPt = new SVGCircle()
                    {
                        R = ChartTextStyle.FontSize / 3,
                        Fill = point.Color,
                        Cx = px,
                        Cy = py
                    };

                    svgPt.ClassList.Add( GetChartTheme( i ) );

                    //if (lineI++ == 0)
                    //{ // move to start point if first line point.
                    //    if (!LabeledAxis.IncludeDefault && MeasuredAxis.IncludeDefault)
                    //    {
                    //        svgLine.MoveTo( baseLineX, py );
                    //    }
                    //    else
                    //    {
                    //        svgLine.MoveTo( baseLineX, baseLineY );
                    //    }
                    //}
                    svgGroup.Add( svgPt );
                }
                i++;
                ChartGroup.Add( svgGroup );
                //var svgLine = new SVGPath()
                //{
                //    StrokeWidth = 2,
                //    FillOpacity = 1,
                //    Stroke = line.Color,
                //    Fill = "none"
                //};
                ////svgLine.ClassList.Add( GetChartStrokeTheme( i++ ) );
                //svgLine.ClassList.Add( GetChartTheme( i++ ) );
                //int lineI = 0;

                //foreach (var point in line.Values)
                //{
                    

                //    svgLine.LineTo( px, py );
                //}
                //ChartGroup.Add( svgLine );
            }
        }        

        private IEnumerable<T1> GetHorizontalSet()
        {
            var set = new List<T1>();
            foreach (var group in ScatterGroups)
            {
                set.AddRange( group.Value.Select( t => t.LabeledValue ) );
            }
            return set.Distinct();
        }

        private IEnumerable<T2> GetVerticalSet()
        {
            var set = new List<T2>();
            foreach (var group in ScatterGroups)
            {
                set.AddRange( group.Value.Select( t => t.MeasuredValue ) );
            }
            return set.Distinct();
        }

        public override double GetLegendLeftOffset()
        {
            return LegendPosition == LegendPosition.Left ? GraphicsHelper.MeasureStringWidth( ScatterGroups.Select( t => t.Value.Name ), ChartTextStyle ) : 0;
        }

        public override double GetLegendRightOffset()
        {
            return LegendPosition == LegendPosition.Right ? GraphicsHelper.MeasureStringWidth( ScatterGroups.Select( t => t.Value.Name ), ChartTextStyle ) : 0;
        }

        protected override IEnumerable<Tuple<string, string>> GetLegendItems()
        {
            return ScatterGroups.Select( t => new Tuple<string, string>( t.Value.Name, t.Value.Color ) );
        }

        public override object Clone()
        {
            var clone = base.Clone() as ScatterChart<T1, T2>;
            clone.ScatterGroups = new Dictionary<string, ScatterGroup<T1, T2>>();
            foreach (var g in ScatterGroups)
            {
                clone.ScatterGroups.Add( g.Key, g.Value.Clone() as ScatterGroup<T1, T2> );
            }
            return clone;
        }
    }
}
