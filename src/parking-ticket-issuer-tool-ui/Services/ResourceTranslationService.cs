using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using ParkingTicketIssuerTool.Features.Shared;
using ParkingTicketIssuerToolFramework.Features.Shared;
using ParkingTicketIssuerToolFramework.Features.Shared.Enums;

namespace ParkingTicketIssuerToolUI.Services;

public class ResourceTranslationService : ITranslationService
{
    private static string resourceFile = "ParkingTicketIssuerToolUI.Properties.Resources";

    private readonly ITranslationService fallbackService;

    private readonly ResourceManager resourceManager;


    public ResourceTranslationService()
    {
        fallbackService = new DefaultTranslationService();
        var assembly = Assembly.GetExecutingAssembly();
        resourceManager = new ResourceManager(resourceFile, assembly);
    }

    public string Translate(string text)
    {
        try
        {
            if (Enum.GetNames<TranslationEnums>().Contains(text))
            {
                text = text.ToLower();
                StringBuilder newText = new StringBuilder(text);
                newText[0] = char.ToUpper(newText[0]);
                for (int i = 0; i < text.Length; i++)
                {
                    if (newText[i] == '_')
                    {
                        i++;
                        newText[i] = char.ToUpper(newText[i]);
                    }
                }

                text = $"Framework_{newText}";
            }
            var returnData = resourceManager.GetString(text);
            if (!string.IsNullOrEmpty(returnData))
            {
                return returnData;
            }
        }
        catch (Exception)
        {
            // Use Fallback
        }
        return fallbackService.Translate(text);
    }
}