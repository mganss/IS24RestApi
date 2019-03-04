using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Search
{
    /// <summary>
    /// Represents a publishing channel for search.
    /// </summary>
    abstract public class Channel
    {
        private static Channel is24Channel = new IS24Channel();

        /// <summary>
        /// The IS24 channel.
        /// </summary>
        public static Channel IS24Channel => is24Channel;

        /// <summary>
        /// Gets the search parameters representing this channel.
        /// </summary>
        /// <returns>The parameter values.</returns>
        public abstract IEnumerable<KeyValuePair<string, string>> GetParameters();
    }

    /// <summary>
    /// The IS24 channel.
    /// </summary>
    public class IS24Channel : Channel
    {
        /// <summary>
        /// Gets the search parameters representing this channel.
        /// </summary>
        /// <returns>
        /// The parameter values.
        /// </returns>
        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("channel", "is24");
        }
    }

    /// <summary>
    /// Represents a group channel.
    /// </summary>
    public class GroupChannel : Channel
    {
        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public string Group { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupChannel"/> class.
        /// </summary>
        /// <param name="group">The group id.</param>
        public GroupChannel(string group)
        {
            Group = group;
        }

        /// <summary>
        /// Gets the search parameters representing this channel.
        /// </summary>
        /// <returns>
        /// The parameter values.
        /// </returns>
        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("channel", Group);
        }
    }

    /// <summary>
    /// Represents the homepage channel of a specific user.
    /// </summary>
    public class HomepageChannel: Channel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomepageChannel"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public HomepageChannel(string userName)
        {
            UserName = userName;
        }

        /// <summary>
        /// Gets the search parameters representing this channel.
        /// </summary>
        /// <returns>
        /// The parameter values.
        /// </returns>
        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("channel", "hp");
            yield return new KeyValuePair<string, string>("username", UserName);
        }
    }
}
