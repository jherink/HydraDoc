﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace Stitch.Tests
{
    public static class SHA1Comparison
    {
        public static bool Equal( string fileA, string fileB )
        {
            return ComputeHash( fileA ) == ComputeHash( fileB );
        }

        private static string ComputeHash( string file )
        {
            string hash = string.Empty;
            if (File.Exists( file ))
            {
                using (var sha1 = new SHA1Managed())
                {
                    var content = File.ReadAllText( file ).Replace( @"\r\n", @"\n" );
                    var sha1Computed = sha1.ComputeHash( System.Text.Encoding.UTF8.GetBytes( content ) );
                    hash = BitConverter.ToString( sha1Computed );
                    
                }
            }
            return hash;
        }

        /// <summary>
        /// Git converts files to Unix.  This causes regressions to fail on Windows. 
        /// Convert all to Unix for standard comparisons.
        /// </summary>
        /// <returns></returns>
        //private static byte[] Dos2Unix( string file )
        //{
        //    const byte CR = 0x0D;
        //    const byte LF = 0x0A;
        //    byte[] data = File.ReadAllBytes( file );

        //}
    }
}
