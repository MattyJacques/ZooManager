using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ThirdParty.Twitter
{
  [Serializable]
  public class UserTimeline
  {
    public Tweet[] Tweets;
  }

  [Serializable]
  public class Tweet
  {
    public string created_at;
    public long id;
    public string id_str;
    public string text;
    public bool truncated;
    public Entities entities;
    public ExtendedEntities extended_entities;
    public string source;
    public object in_reply_to_status_id;
    public object in_reply_to_status_id_str;
    public object in_reply_to_user_id;
    public object in_reply_to_user_id_str;
    public object in_reply_to_screen_name;
    public User user;
    public object geo;
    public object coordinates;
    public object place;
    public object contributors;
    public RetweetedStatus retweeted_status;
    public bool is_quote_status;
    public int retweet_count;
    public int favorite_count;
    public bool favorited;
    public bool retweeted;
    public bool possibly_sensitive;
    public string lang;
  }

  [Serializable]
  public class Hashtag
  {
    public string text;
    public List<int> indices;
  }

  [Serializable]
  public class UserMention
  {
    public string screen_name;
    public string name;
    public long id;
    public string id_str;
    public List<int> indices;
  }

  [Serializable]
  public class Size
  {
    public int w;
    public int h;
    public string resize;
  }

  [Serializable]
  public class Sizes
  {
    public Size large;
    public Size medium;
    public Size thumb;
    public Size small;
  }

  [Serializable]
  public class Medium
  {
    public long id;
    public string id_str;
    public List<int> indices;
    public string media_url;
    public string media_url_https;
    public string url;
    public string display_url;
    public string expanded_url;
    public string type;
    public Sizes sizes;
    public long source_status_id;
    public string source_status_id_str;
    public long source_user_id;
    public string source_user_id_str;
  }

  [Serializable]
  public class Entities
  {
    public List<Hashtag> hashtags;
    public List<object> symbols;
    public List<UserMention> user_mentions;
    public List<object> urls;
    public List<Medium> media;
  }

  [Serializable]
  public class ExtendedEntities
  {
    public List<Medium> media;
  }

  [Serializable]
  public class User
  {
    public long id;
    public string id_str;
  }

  [Serializable]
  public class RetweetedStatus
  {
    public string created_at;
    public long id;
    public string id_str;
    public string text;
    public bool truncated;
    public Entities entities;
    public ExtendedEntities extended_entities;
    public string source;
    public object in_reply_to_status_id;
    public object in_reply_to_status_id_str;
    public object in_reply_to_user_id;
    public object in_reply_to_user_id_str;
    public object in_reply_to_screen_name;
    public User user;
    public object geo;
    public object coordinates;
    public object place;
    public object contributors;
    public bool is_quote_status;
    public int retweet_count;
    public int favorite_count;
    public bool favorited;
    public bool retweeted;
    public bool possibly_sensitive;
    public string lang;
  }
}
