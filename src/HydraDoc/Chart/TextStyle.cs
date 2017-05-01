﻿using HydraDoc.Elements.Interface;

namespace HydraDoc.Chart
{
    public interface ITextStyle
    {
        bool Bold { get; set; }

        bool Italic { get; set; }

        int FontSize { get; set; }

        string Color { get; set; }

        string FontName { get; set; }

    }

    public class TextStyle : ITextStyle
    {
        private readonly IElement AppliedElement;

        public TextStyle( IElement appliedElement )
        {
            AppliedElement = appliedElement;
        }

        public bool Bold
        {
            get
            {
                var value = string.Empty;
                return AppliedElement.StyleList.TryGetValue( "font-weight", out value ) && value == "bold";
            }
            set
            {
                AppliedElement.StyleList.Add( "font-weight", value ? "bold" : "normal" );
            }
        }

        public bool Italic
        {
            get
            {
                var value = string.Empty;
                return AppliedElement.StyleList.TryGetValue( "font-style", out value ) && value == "italic";
            }
            set
            {
                AppliedElement.StyleList.Add( "font-style", value ? "italic" : "normal" );
            }
        }

        public int FontSize
        {
            get
            {
                var value = string.Empty;
                if (AppliedElement.StyleList.TryGetValue( "font-size", out value ))
                {
                    return int.Parse( value.Substring( 0, value.Length - 2 ) );
                }
                return 14;
            }
            set
            {
                AppliedElement.StyleList.Add( "font-size", $"{value}px" );
            }
        }

        public string Color
        {
            get
            {
                var value = string.Empty;
                return AppliedElement.StyleList.TryGetValue( "color", out value ) ? value : string.Empty;
            }
            set
            {
                AppliedElement.StyleList.Add( "color", value );
            }
        }

        public string FontName
        {
            get
            {
                var value = string.Empty;
                return AppliedElement.StyleList.TryGetValue( "font-family", out value ) ? value : string.Empty;
            }
            set
            {
                AppliedElement.StyleList.Add( "font-family", value );
            }
        }
    }

}
