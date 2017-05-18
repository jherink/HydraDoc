﻿using Stitch.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Chart
{
    public abstract class ChartBase : SVG, IChart
    {
        #region IChart Implementation

        public LegendPosition LegendPosition { get; set; } = LegendPosition.None;

        public ITextStyle TitleTextStyle { get; set; }

        public string ChartTitle { get { return SvgTitle.Text; } set { SvgTitle.Text = value; } }

        public ITextStyle ChartTextStyle { get; set; }

        double IChart.Width
        {
            get
            {
                return Width;
            }

            set
            {
                Width = value;
            }
        }

        double IChart.Height
        {
            get
            {
                return Height;
            }

            set
            {
                Height = value;
            }
        }

        #endregion

        #region SVGMembers 

        protected readonly SVGGroup TitleGroup = new SVGGroup();
        protected readonly SVGText SvgTitle = new SVGText();
        protected readonly SVGGroup Legend = new SVGGroup();
        protected readonly SVGGroup ChartGroup = new SVGGroup();

        #endregion

        protected ChartBase( int height, int width ) : base( height, width )
        {
            StyleList.Add( "overflow: visible;" );
            //StyleList.Add( "width", "100%" );
            Children.Add( TitleGroup );

            SvgTitle = new SVGText();
            TitleTextStyle = new TextStyle( SvgTitle );
            ChartTextStyle = new TextStyle( this );

            TitleGroup.Add( SvgTitle );
            Children.Add( ChartGroup );
            Children.Add( Legend );

            ChartTextStyle.FontSize = 15;
            ChartTextStyle.ApplyStyle( new SVGText() );
            StyleList.Add( "margin-top", "30px" );
            StyleList.Add( "margin-bottom", "30px" );
        }

        protected double GetTitleHeight()
        {
            return !string.IsNullOrWhiteSpace( ChartTitle ) ? TitleTextStyle.FontSize * 4 : 0;
        }

        protected string GetChartTheme( int id )
        {
            return $"stitch-chart-theme-{(id % 23)}";
        }

        protected string GetChartTextTheme( int id )
        {
            return $"stitch-chart-text-theme-{(id % 23)}";
        }

        protected abstract void RenderChart();

        /// <summary>
        /// Compile a list of tuples consisting of the text to show in the legend and the color to render the legend indicator in.
        /// </summary>
        /// <returns>Compile a list of tuples consisting of the text to show in the legend and the color to render the legend indicator in.</returns>
        //protected abstract IEnumerable<Tuple<string, string>> CompileLegendContent();

        protected void RenderChartTitle()
        {
            if (!string.IsNullOrWhiteSpace( ChartTitle ))
            {
                SvgTitle.X = Width / 2;
                SvgTitle.Y = 1.5 * TitleTextStyle.FontSize;
                SvgTitle.StyleList.Add( "text-anchor", "middle" );
            }
        }

        protected abstract void RenderLegend();

        protected SVGGroup CreateLegendLine( string label, string color, int id )
        {
            var g = new SVGGroup();

            var text = new SVGText() { Text = label };

            var circle = new SVGCircle()
            {
                R = ChartTextStyle.FontSize / 2,
                Fill = color,
                Stroke = "none"
            };

            circle.ClassList.Add( GetChartTheme( id ) );

            g.Children.Add( circle );
            g.Children.Add( text );

            return g;
        }

        public override string Render()
        {
            RenderChartTitle();
            RenderLegend();
            RenderChart();
            return base.Render();
        }
    }
}