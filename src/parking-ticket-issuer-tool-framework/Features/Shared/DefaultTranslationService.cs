using ParkingTicketIssuerTool.Features.Shared;
using ParkingTicketIssuerToolFramework.Features.Shared.Enums;

namespace ParkingTicketIssuerToolFramework.Features.Shared;

public class DefaultTranslationService : ITranslationService
{    
    public string Translate(string text)
    {
        if (Enum.GetNames<TranslationEnums>().Contains(text))
        {
            var enumIndex = (int)Enum.Parse(typeof(TranslationEnums), text);
            var enumData = (TranslationEnums)enumIndex;
            return enumData switch
            {
                TranslationEnums.PARKING_TICKET => "Parking Ticket",
                TranslationEnums.UNKNOWN_VEHICLE => "unknown",
                TranslationEnums.ISSUER => "Issuer:",
                TranslationEnums.DRIVER => "Driver:",
                TranslationEnums.VEHICLE => "Vehicle:",
                TranslationEnums.DATE => "Date:",
                TranslationEnums.UNKNOWN_LOCATION => "unknown location",
                TranslationEnums.OFFICER_ISSUED_TICKET => "Officer **{0}** issued a parking ticket to **{1}** on **{2}** for the vehicle **{3}** at **{4}**",
                TranslationEnums.SENTENCE => "Sentence",
                TranslationEnums.ADDITIONAL_INFORMATION => "Additional Information",
                TranslationEnums.PLEASE_CHECK_NEXT_PAGE => "Please check the next page for evidence.",
                TranslationEnums.ISSUED_BY => "Issued by **{0}** on **{1}**",
                _ => text

            };
        }
        return text;
    }
}