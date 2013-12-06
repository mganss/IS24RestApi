namespace IS24RestApi
{
    /// <summary>
    /// Describes the basic behavior of a REST resource
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Gets the underlying <see cref="IIS24Connection"/> for executing the requests
        /// </summary>
        IIS24Connection Connection { get; }
    }
}