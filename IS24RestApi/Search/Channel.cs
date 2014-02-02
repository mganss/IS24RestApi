using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS24RestApi.Search
{
    abstract public class Channel
    {
        public static Channel IS24Channel = new IS24Channel();
        public abstract IEnumerable<KeyValuePair<string, string>> GetParameters();
    }

    public class IS24Channel : Channel
    {
        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("channel", "is24");
        }
    }

    public class GroupChannel : Channel
    {
        public string Group { get; set; }

        public GroupChannel(string group)
        {
            Group = group;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("channel", Group);
        }
    }

    public class HomepageChannel: Channel
    {
        public string UserName { get; set; }

        public HomepageChannel(string userName)
        {
            UserName = userName;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            yield return new KeyValuePair<string, string>("channel", "hp");
            yield return new KeyValuePair<string, string>("username", UserName);
        }
    }
}
