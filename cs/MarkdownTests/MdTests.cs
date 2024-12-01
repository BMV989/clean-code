using System.Diagnostics;
using Markdown;
using FluentAssertions;
namespace MarkdownTests;

[TestFixture]
[TestOf(typeof(Md))]
public class MdTests
{
    [TestCaseSource(nameof(ConvertTagsTests))] 
    [TestCaseSource(nameof(MdSpecTests))]
    public void Render_ShouldWorkCorrectly(string input, string expected) => Md.Render(input).Should().Be(expected);

    [TestCase(100, 10)]
    [TestCase(100, 100)]
    [TestCase(100, 1000)]
    public void Render_ShouldWorkLinearly(int times, int inputScale)
    {
        const string input = "# Заголовок c _курсивным текстом_ и __полужирным текстом__";
        var scaledInput = string.Join(Environment.NewLine, Enumerable.Repeat(input, inputScale));
        var timeWithDefaultInput = MeasureRenderTime(input, times);
        var timeWithScaledInput = MeasureRenderTime(scaledInput, times);
        var avgWithDefaultInput = timeWithDefaultInput / times;
        var avgWithScaledInput = timeWithScaledInput / (inputScale * times);
        
        avgWithDefaultInput.Should().BeCloseTo(avgWithScaledInput, TimeSpan.FromTicks(3300));
    }


    private static TimeSpan MeasureRenderTime(string input, int times = 1)
    {
        var timer = new Stopwatch();

        for (var i = 0; i < times; i++)
        {
            timer.Start();
            Md.Render(input);
            timer.Stop();
        }
        
        return timer.Elapsed;
    }
    public static IEnumerable<TestCaseData> ConvertTagsTests
    {
        get
        {
            yield return new TestCaseData(
                    $"# Заголовок{Environment.NewLine}#Заголовок", $"<h1> Заголовок</h1>{Environment.NewLine}<h1>Заголовок</h1>")
                .SetName("Render_ShouldConvertHeaderTag")
                .SetCategory(nameof(ConvertTagsTests));
            yield return new TestCaseData(
                    $"_курсивный текст_{Environment.NewLine}_курсивный текст_",
                    $"<em>курсивный текст</em>{Environment.NewLine}<em>курсивный текст</em>")
                .SetName("Render_ShouldConvertItalicTag")
                .SetCategory(nameof(ConvertTagsTests));
            yield return new TestCaseData(
                    $"__полужирный текст__{Environment.NewLine}__полужирный текст__",
                    $"<strong>полужирный текст</strong>{Environment.NewLine}<strong>полужирный текст</strong>")
                .SetName("Render_ShouldConvertBoldTag")
                .SetCategory(nameof(ConvertTagsTests));
            yield return new TestCaseData(
                    "_чем\\_ 100_ __раз_ услышать.__",
                    "_чем_ 100_ __раз_ услышать.__")
                .SetName("Render_ShouldConvertEscapeTag")
                .SetCategory(nameof(ConvertTagsTests));
            yield return new TestCaseData(
                    "# Заголовок c _курсивным текстом_ и __полужирным текстом__",
                    "<h1> Заголовок c <em>курсивным текстом</em> и <strong>полужирным текстом</strong></h1>")
                .SetName("Render_ShouldConvertAllTagsInHeader")
                .SetCategory(nameof(ConvertTagsTests)); 
        }
    }

    public static IEnumerable<TestCaseData> MdSpecTests
    {
        get
        { 
            yield return new TestCaseData(
                    $"#Это заголовок, а это (#) - нет.{Environment.NewLine}И это #тоже# не ##заголовок##",
                    $"<h1>Это заголовок, а это (#) - нет.</h1>{Environment.NewLine}И это #тоже# не ##заголовок##")
                .SetName("Render_ShouldIgnoreSingleTags_WhenNotStartsWithNewLine")
                .SetCategory("BasicSpec");
            yield return new TestCaseData(
                    $"Это _заголовок1_ ,а не заголовок __1 уровня__{Environment.NewLine}_4 Life CJ, _Grove __123__ Street_ 4 Life_",
                    $"Это _заголовок1_ ,а не заголовок __1 уровня__{Environment.NewLine}_4 Life CJ, <em>Grove __123__ Street</em> 4 Life_")
                .SetName("Render_ShouldIgnorePairTags_WhenPlacedWithNumbers")
                .SetCategory("MdSpec");
            yield return new TestCaseData(
                    $"Подчерки могут выделять часть слова{Environment.NewLine}Но в раз_ных сло_вах н__е мог__ут",
                    $"Подчерки могут выделять часть слова{Environment.NewLine}Но в раз_ных сло_вах н__е мог__ут")
                .SetName("Render_ShouldIgnorePairTags_WhenPartsOfDifferentWordsMarked")
                .SetCategory("MdSpec");
            yield return new TestCaseData(
                    "В случае __пересечения _двойных__ и одинарных_ подчерков ни _один из __них не_ считается__ выделением",
                    "В случае __пересечения _двойных__ и одинарных_ подчерков ни _один из __них не_ считается__ выделением")
                .SetName("Render_ShouldIgnorePairTags_WhenIntersection")
                .SetCategory("MdSpec");
            yield return new TestCaseData(
                    "Если внутри подчерков пустая строка ____, то они остаются символами подчерка",
                    "Если внутри подчерков пустая строка ____, то они остаются символами подчерка")
                .SetName("Render_ShouldIgnorePairTags_WhenTextIsEmpty")
                .SetCategory("MdSpec");
        }
    }
}

