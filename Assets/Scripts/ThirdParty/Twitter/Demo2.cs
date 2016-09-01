using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ThirdParty.Twitter
{
  class Demo2 : MonoBehaviour
  {
    // You need to save access token and secret for later use.
    // You can keep using them whenever you need to access the user's Twitter account. 
    // They will be always valid until the user revokes the access to your application.
    private const string TwitterUserId = "3056989378";
    private const string TwitterScreenName = "SuperIsHere";
    private const string TwitterUserToken = "3056989378-cMFqalsKk8ZKEbiQopsrNNf9pPwmXxwYW4gbDeg";
    private const string TwitterSecret = "2NjQhXXeLoNybd8HiQN5lsZReJ2sCyIluMlNvbDuW7vTo";
    
    // Use this for initialization
    private void Start()
    {
      var tokenResponse = new AccessTokenResponse();
      tokenResponse.UserId = TwitterUserId;
      tokenResponse.ScreenName = TwitterScreenName;
      tokenResponse.Token = TwitterUserToken;
      tokenResponse.TokenSecret = TwitterSecret;

      StartCoroutine(API.GetTimeline("rand", "OSHVMJHuDmW5xeWX7VYVBWubW",
        "3rDvhRhSO76WJrniGmvYKv6OVtRuQmpSmjns9w40JxLCxFx6lF", tokenResponse));
    }

    // Update is called once per frame
    private void Update()
    {
    }
  }
}
