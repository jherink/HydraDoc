﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HydraDoc.Graph;

namespace HydraDoc.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void LineGraphTest()
        {
            var doc = new HydraDocument();
            var graph = new LineGraph();
            graph.Start = 0;
            graph.AddData( "Temperatures In NY City", 43, "1" );
            graph.AddData( "Temperatures In NY City", 53, "2" );
            graph.AddData( "Temperatures In NY City", 50, "3" );
            graph.AddData( "Temperatures In NY City", 57, "4" );
            graph.AddData( "Temperatures In NY City", 59, "5" );
            graph.AddData( "Temperatures In NY City", 69, "6" );
            graph.SetLineColor( "Temperatures In NY City", "#0074d9" );
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "LineGraphTest", doc );
        }

        [TestMethod]
        public void LineGraph2Test()
        {
            var doc = new HydraDocument();
            var graph = new LineGraph();
            graph.AddData( "line", 120, "0" );
            graph.AddData( "line", 60, "1" );
            graph.AddData( "line", 80, "2" );
            graph.AddData( "line", 20, "3" );
            graph.AddData( "line", 80, "4" );
            graph.AddData( "line", 80, "5" );
            graph.AddData( "line", 60, "6" );
            graph.AddData( "line", 100, "7" );
            graph.AddData( "line", 90, "8" );
            graph.AddData( "line", 80, "9" );
            graph.AddData( "line", 110, "10" );
            graph.AddData( "line", 10, "11" );
            graph.AddData( "line", 70, "12" );
            graph.AddData( "line", 100, "13" );
            graph.AddData( "line", 100, "14" );
            graph.AddData( "line", 40, "15" );
            graph.AddData( "line", 0, "16" );
            graph.AddData( "line", 100, "17" );
            graph.AddData( "line", 100, "18" );
            graph.AddData( "line", 120, "19" );
            graph.AddData( "line", 60, "20" );
            graph.AddData( "line", 70, "21" );
            graph.AddData( "line", 80, "22" );
            graph.SetLineColor( "line", "#0074d9" );
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "LineGraph2Test", doc );
        }


        [TestMethod]
        public void LineGraph3Test()
        {
            var doc = new HydraDocument();
            var graph = new LineGraph();
            graph.AddData( "line", 24000, "2001" );
            graph.AddData( "line", 22500, "2002" );
            graph.AddData( "line", 19700, "2003" );
            graph.AddData( "line", 17500, "2004" );
            graph.AddData( "line", 14500, "2005" );
            graph.AddData( "line", 10000, "2006" );
            graph.AddData( "line", 5800, "2007" );           
            graph.SetLineColor( "line", "green" );
           
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "LineGraph3Test", doc );
        }

        [TestMethod]
        public void LineGraph4Test()
        {
            var doc = new HydraDocument();
            var graph = new LineGraph();
            graph.Start = 0;
            graph.AddData( "Temperatures In NY City", 10, "1" );
            graph.AddData( "Temperatures In NY City", 22, "2" );
            graph.AddData( "Temperatures In NY City", 33, "3" );
            graph.AddData( "Temperatures In NY City", 14, "4" );
            graph.AddData( "Temperatures In NY City", 42, "5" );
            graph.AddData( "Temperatures In NY City", 29, "6" );

            graph.AddData( "Temperatures In Omaha", 43, "1" );
            graph.AddData( "Temperatures In Omaha", 53, "2" );
            graph.AddData( "Temperatures In Omaha", 50, "3" );
            graph.AddData( "Temperatures In Omaha", 57, "4" );
            graph.AddData( "Temperatures In Omaha", 59, "5" );
            graph.AddData( "Temperatures In Omaha", 69, "6" );

            graph.SetLineColor( "Temperatures In NY City", "red" );
            graph.SetLineColor( "Temperatures In Omaha", "blue" );
            graph.SetLineThickness( "Temperatures In Omaha", 5 );
            graph.SetLineThickness( "Temperatures In NY City", 5 );
            doc.Add( graph );

            IntegrationHelpers.SaveToTemp( "LineGraph4Test", doc );
        }

        [TestMethod]
        public void ScatterPlotTest()
        {
            var doc = new HydraDocument();
            var graph = new ScatterPlot();
            graph.GraphHeight = 500;
            graph.Color = "red";
            graph.AddData( 2008, 7.2 );
            graph.AddData( 2009, 8.1 );
            graph.AddData( 2010, 7.7 );
            graph.AddData( 2011, 6.8 );
            graph.AddData( 2012, 11.7 );

            doc.Add( graph );
            //doc.Add( graph.CreateAxis() );

            IntegrationHelpers.SaveToTemp( "ScatterPlotTest", doc );
        }
    }
}
