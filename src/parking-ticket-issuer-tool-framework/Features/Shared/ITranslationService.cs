using ParkingTicketIssuerToolFramework.Features.Shared.Enums;

namespace ParkingTicketIssuerTool.Features.Shared;

/// <summary>
/// Service to translate text.
/// </summary>
public interface ITranslationService
{
    /// <summary>
    /// Translates the given Enum to a string.
    /// </summary>
    /// <param name="translation">The translation to use</param>
    /// <returns>The translated text</returns>
    public string Translate(TranslationEnums translation)
    {
        return Translate(translation.ToString());
    }

    /// <summary>
    /// Translates the given text to a string.
    /// </summary>
    /// <param name="text">The text which should be translated</param>
    /// <returns>The translated text</returns>
    string Translate(string text);
}