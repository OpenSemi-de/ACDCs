namespace ACDCs.Sensors.API.Interfaces;

public interface ISample<TResult> : ISample
{
    TResult Sample { get; set; }
}

public interface ISample
{
    DateTime Time { get; set; }
}
