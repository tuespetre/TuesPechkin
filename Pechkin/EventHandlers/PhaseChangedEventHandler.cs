namespace TuesPechkin.EventHandlers
{
    public delegate void PhaseChangedEventHandler(IConverter converter, IDocument document, int phaseNumber, string phaseDescription);
}