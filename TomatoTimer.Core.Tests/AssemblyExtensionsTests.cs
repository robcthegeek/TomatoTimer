﻿using System.IO;
using System.Reflection;
using System;
using Xunit;

namespace TomatoTimer.Core.Tests
{
    public abstract class AssemblyExtensionsTest
    {
        protected Assembly Assembly;

        protected AssemblyExtensionsTest()
        {
            Assembly = GetAssembly();
        }

        protected abstract Assembly GetAssembly();
    }

    public class when_assembly_null : AssemblyExtensionsTest
    {
        protected override Assembly GetAssembly()
        {
            return null;
        }

        [Fact(Skip = "Tests Pending Review")]
        public void directory_returns_string_empty()
        {
            Assert.Equal(string.Empty, Assembly.Directory());
        }
    }

    public class when_assembly_system_core : AssemblyExtensionsTest
    {
        protected override Assembly GetAssembly()
        {
            // We'll Use System.Core for the Tests (Which Contains the Action Delegate)..
            // Loaded from the GAC
            // "C:\\Windows\\assembly\\GAC_MSIL\\System.Core\\3.5.0.0__b77a5c561934e089"
            var asm = Assembly.GetAssembly(typeof(Action));
            return asm;
        }

        private const string GAC_Address = @"C:\Windows\assembly\GAC_MSIL\System.Core\3.5.0.0__b77a5c561934e089\";

        [Fact(Skip = "Tests Pending Review")]
        public void directory_returns_a_string_value()
        {
            Assert.False(string.IsNullOrEmpty(Assembly.Directory()));
        }

        [Fact(Skip = "Tests Pending Review")]
        public void directory_returns_gac_address()
        {
            var s = Assembly.Directory();
            Assert.Equal(GAC_Address, s);
        }
    }

    public class when_assembly_in_test_project : AssemblyExtensionsTest
    {
        protected override Assembly GetAssembly()
        {
            return Assembly.GetAssembly(typeof (AssemblyExtensionsTest));
        }

        private const string DLL_Name = "TomatoTimer.Tests";

        [Fact(Skip = "Tests Pending Review")]
        public void directory_returns_tests_bin_debug()
        {
            var path = Assembly.Directory();
            Assert.Contains(@"tests\bin\debug", path.ToLower());
        }

        [Fact(Skip = "Tests Pending Review")]
        public void executing_directory_ends_with_directoryseperatorchar()
        {
            var path = Assembly.Directory();
            Assert.True(path.EndsWith(Path.DirectorySeparatorChar.ToString()));
        }

        [Fact(Skip = "Tests Pending Review")]
        public void can_locate_dll_based_on_directory()
        {
            var path = Assembly.Directory();
            Assert.True(File.Exists(path + Assembly.ManifestModule.Name));
        }
    }
}