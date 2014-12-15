namespace TuesPechkin.EventHandlers
{
    public delegate void PhaseChangedEventHandler(IConverter converter, HtmlDocument document, int phaseNumber, string phaseDescription);
}