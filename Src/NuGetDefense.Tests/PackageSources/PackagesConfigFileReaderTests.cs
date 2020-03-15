﻿using FluentAssertions;
using NuGetDefense.PackageSources;
using Xunit;

namespace NuGetDefense.Tests.PackageSources
{
    public class PackagesConfigFileReaderTests
    {
        [Fact]
        public void TryReadFromFile_ReturnsFalse_WhenFileDoesNotExist()
        {
            PackagesConfigFileReader.TryReadFromFile(@"non-existing/packages.config", out var packages).Should().BeFalse();
            packages.Should().BeEmpty();
        }
        
        [Theory]
        [InlineData(@"TestFiles/packages.config")]
        [InlineData(@"TestFiles/test.csproj")]
        [InlineData("TestFiles")]
        public void TryReadFromFile_ReturnsTrue_WhenFileExists(string path)
        {
            PackagesConfigFileReader.TryReadFromFile(path, out var packages).Should().BeTrue();
            packages.Should().NotBeEmpty().And.HaveCount(4);
        }
    }
}