namespace IS24RestApi
{
    /// <summary>
    /// The <see cref="RealEstateItem"/> wraps the <see cref="RealEstate"/> data item together with the resource 
    /// responsible for the attachments for this real estate
    /// </summary>
    public class RealEstateItem : IRealEstate
    {
        /// <summary>
        /// Get the <see cref="IRealEstate.RealEstate"/> instance with data delivered from IS24
        /// </summary>
        public RealEstate RealEstate { get; private set; }

        /// <summary>
        /// Gets the <see cref="IAttachmentResource"/> for the real estate retrieved
        /// </summary>
        public IAttachmentResource Attachments { get; private set; }

        /// <summary>
        /// Creates a new <see cref="RealEstateItem"/> instance
        /// </summary>
        /// <param name="realEstate"><see cref="RealEstate"/> data item</param>
        /// <param name="connection">The <see cref="IIS24Connection"/> used for querying the API</param>
        public RealEstateItem(RealEstate realEstate, IIS24Connection connection)
        {
            RealEstate = realEstate;
            Attachments = new AttachmentResource(realEstate, connection);
        }
    }
}