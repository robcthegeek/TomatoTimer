using System.IO;
using System.Reflection;
using System;
using Leonis.TomatoTimer.Core;
using Xunit;

namespace TomatoTimer.Core.Tests
{
    public abstract class AssemblyExtensionsTest
    {
        protected Assembly assembly;

        public AssemblyExtensionsTest()
        {
            assembly = GetAssembly();
        }

        protected abstract Assembly GetAssembly();
    }

    public class when_assembly_null : AssemblyExtensionsTest
    {
        protected override Assembly GetAssembly()
        {
            return null;
        }

        [Fact]
        public void directory_returns_string_empty()
        {
            Assert.Equal(string.Empty, assembly.Directory());
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

        [Fact]
        public void directory_returns_a_string_value()
        {
            Assert.False(string.IsNullOrEmpty(assembly.Directory()));
        }

        [Fact]
        public void directory_returns_gac_address()
        {
            var s = assembly.Directory();
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

        [Fact]
        public void directory_returns_tests_bin_debug()
        {
            var path = assembly.Directory();
            Assert.Contains(@"tests\bin\debug", path.ToLower());
        }

        [Fact]
        public void executing_directory_ends_with_directoryseperatorchar()
        {
            var path = assembly.Directory();
            Assert.True(path.EndsWith(Path.DirectorySeparatorChar.ToString()));
        }

        [Fact]
        public void can_locate_dll_based_on_directory()
        {
            var path = assembly.Directory();
            Assert.True(File.Exists(path + assembly.ManifestModule.Name));
        }
    }
}