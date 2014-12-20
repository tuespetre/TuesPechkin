using System;

namespace TuesPechkin
{
    public interface IConverter
    {
        byte[] Convert(IDocument document);

        event BeginEventHandler Begin;

        event WarningEventHandler Warning;

        event ErrorEventHandler Error;

        event PhaseChangedEventHandler PhaseChanged;

        event ProgressChangedEventHandler ProgressChanged;

        event FinishEventHandler Finished;
    }
}
