using System;

namespace Scripts.Utils.SheetParser;

public abstract class GenericParsingContext<TSaver> : ParsingContext where TSaver : ParseSaver
{
    protected TSaver Saver => _parseSaver as TSaver;
    
    protected GenericParsingContext(ParseSaver saver) : base(saver)
    {
    }
}