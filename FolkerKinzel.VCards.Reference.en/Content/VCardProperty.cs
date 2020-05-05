public abstract class VCardProperty<T>
{
    public string? Group { get; set; }

    public ParameterSection Parameters { get; }

    public virtual T Value { get; protected set; }
}
