using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void CaseInsensitiveEquals_NoWhiteSpaceMixedCase()
        {
            "Word".CaseInsensitiveEquals("WorD").Should().BeTrue();
        }

        [Fact]
        public void CaseInsensitiveEquals_NoWhiteSpace()
        {
            "Word".CaseInsensitiveEquals("WORD").Should().BeTrue();
        }

        [Fact]
        public void CaseInsensitiveEquals_TrailingWhiteSpace()
        {
            "Word    ".CaseInsensitiveEquals("WORD    ").Should().BeTrue();
        }

        [Fact]
        public void CaseInsensitiveEquals_LeadingWhiteSpace()
        {
            "    Word".CaseInsensitiveEquals("    WORD").Should().BeTrue();
        }

        [Fact]
        public void CaseInsensitiveEquals_WhiteSpace()
        {
            "    Word     ".CaseInsensitiveEquals("    WORD     ").Should().BeTrue();
        }

        [Fact]
        public void CaseInsensitiveEquals_OnlyWhiteSpace()
        {
            "    ".CaseInsensitiveEquals("    ").Should().BeTrue();
        }

        [Fact]
        public void CaseInsensitiveEquals_Null()
        {
            string word = null;

            word.CaseInsensitiveEquals(null).Should().BeTrue();
        }

        [Fact]
        public void CaseInsensitiveEquals_Empty()
        {
            string.Empty.CaseInsensitiveEquals(string.Empty).Should().BeTrue();
        }

        [Fact]
        public void ToUpperCase_NoWhiteSpace()
        {
            "Word".ToUpperCase().Should().Be("WORD");
        }

        [Fact]
        public void ToUpperCase_TrailingWhiteSpace()
        {
            "Word    ".ToUpperCase().Should().Be("WORD    ");
        }

        [Fact]
        public void ToUpperCase_LeadingWhiteSpace()
        {
            "    Word".ToUpperCase().Should().Be("    WORD");
        }

        [Fact]
        public void ToUpperCase_WhiteSpace()
        {
            "    Word     ".ToUpperCase().Should().Be("    WORD     ");
        }

        [Fact]
        public void ToUpperCase_OnlyWhiteSpace()
        {
            "    ".ToUpperCase().Should().Be("    ");
        }

        [Fact]
        public void ToUpperCase_Null()
        {
            string word = null;

            word.ToUpperCase().Should().BeNull();
        }

        [Fact]
        public void ToUpperCase_Empty()
        {
            string.Empty.ToUpperCase().Should().Be(string.Empty);
        }
    }
}
