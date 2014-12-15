using System;
using TuesPechkin.EventHandlers;

namespace TuesPechkin
{
    public interface IConverter
    {
        byte[] Convert(HtmlDocument document);

        event BeginEventHandler Begin;

        event WarningEventHandler Warning;

        event ErrorEventHandler Error;

        event PhaseChangedEventHandler PhaseChanged;

        event ProgressChangedEventHandler ProgressChanged;

        event FinishEventHandler Finished;
    }
}
