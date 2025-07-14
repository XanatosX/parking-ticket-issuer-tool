# Parking Ticket Issuer Tool

This tool does allow you to create parking tickets, this tool should be used for the game [Foxhole][foxhole] only. The tool will create an PDF you can send to your regiment member to tell them what they did wrong.

## Requirements

- Dotnet 9.0
- Windows / Linux

## Used Libraries and Assets

## Create from source

### Requirements

- [Dotnet 9.0 SDK][dotnet-9-sdk]

### Build

To create the tool from source you need to execute the following commands:

```
dotnet restore
dotnet build -c Release
```

After the build is done you can find the tool in the `bin/Release/net9.0/publish` folder. Execute the binary to start it.

## Installation

1. Go to the [Releases][releases] page and download the latest release.
2. Select the correct version for your operating system.
3. Extract the archive.
4. Copy the content to a folder of your choice.
5. Run the binary.


### Libraries

- [Avalonia][avalonia]
- [QuestPDF][quest-pdf]
- [QuestPDF.Markdown][quest-pdf-markdown]

### Assets

#### Fonts

- [Mechanical Font][mechanical-font]

[foxhole]: https://www.foxholegame.com/
[avalonia]: https://avaloniaui.net/
[quest-pdf]: https://www.questpdf.com/
[quest-pdf-markdown]: https://github.com/christiaanderidder/QuestPDF.Markdown
[dotnet-9-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/9.0
[mechanical-font]: https://www.fontspace.com/mechanical-font-f22368
[releases]: https://github.com/XanatosX/parking-ticket-issuer-tool/releases/latest
