using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nozomi.Preprocessing.Abstracts
{
    public abstract class BaseProcessingService<T> : BaseHostedService<T> where T : class
    {
        public BaseProcessingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public JToken ProcessIdentifier(JToken token, string identifier)
        {
            // Identifier processing
            if (!string.IsNullOrEmpty(identifier) && token != null && token.Type != JTokenType.Null)
            {
                var identifierArr = identifier.Split("/"); // Split the string if its nesting
                var lastIdentifier = identifierArr.LastOrDefault(); // get the last to identify if its the last

                foreach (var identifierEl in identifierArr)
                {
                    if (identifierEl != null)
                    {
                        // CHECK CURRENT TYPE
                        // Identify if its an array or an object
                        if (token is JArray)
                        {
                            try
                            {
                                // Is it the last?
                                if (identifierEl != lastIdentifier)
                                {
                                    // Parse the identifierEl to an integer for index access
                                    if (int.TryParse(identifierEl, out int index))
                                    {
                                        // Pump in the array, treat it as anonymous.
                                        var dataList = token.ToObject<List<JObject>>();

                                        // let's work it out
                                        // update the token
                                        if (index >= 0 && index < dataList.Count)
                                        {
                                            // Traverse the array
                                            token =
                                                JToken.Parse(JsonConvert.SerializeObject(dataList[index]));
                                        }
                                    }
                                }
                                // Yes its the last
                                else
                                {
                                    // See if there's any property we need to refer to.
                                    var comArrElArr = identifierEl.Split("=>");

                                    // An array of objects. Let's find the key comArrElArr[0] where
                                    // the value equals comArrElArr[1]
                                    if (comArrElArr.Length == 2)
                                    {
                                        #if DEBUG
                                        var childrensTest = token.Children();
                                        #endif
                                        var originalToken = token;

                                        // If this is an array
                                        if (token.Type.Equals(JTokenType.Array) 
                                            && int.TryParse(comArrElArr[0], out var index))
                                        {
                                            token = token
                                                    // Apparently, .ToString() matters alot so take note.
                                                .FirstOrDefault(tok => tok[index].ToString()
                                                    .Equals(comArrElArr[1],
                                                        StringComparison.InvariantCultureIgnoreCase));
                                        }
                                        else
                                        {
                                            // https://stackoverflow.com/questions/7216917/json-net-has-key-method
                                            token = token.Children()
                                                .FirstOrDefault(tok => tok.SelectToken(comArrElArr[0]).ToString()
                                                    .Equals(comArrElArr[1]));
                                        }

                                        // Null check
                                        if (token == null)
                                        {
                                            _logger.LogError("[BaseProcessingService] " +
                                                             $"Invalid key value pair {identifierEl} \n" + 
                                                             $"Original Payload empty?: {originalToken != null}");
                                            return false;
                                        }
                                    }
                                    // A standard array
                                    else if (comArrElArr.Length == 1)
                                    {
                                        // Parse the identifierEl to an integer for index access
                                        if (int.TryParse(identifierEl, out int index))
                                        {
                                            // Pump in the array, treat it as anonymous.
                                            var dataList = token.ToObject<List<JObject>>();

                                            // let's work it out
                                            // update the token
                                            if (index >= 0 && index < dataList.Count)
                                            {
                                                // Traverse the array and move up to the parent
                                                token =
                                                    JToken.Parse(JsonConvert.SerializeObject(dataList[index])).Parent;
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogError("[BaseProcessingService]" +
                                                             $" Update: Invalid array element {identifierEl}");
                                            return false;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        else if (token is JObject)
                        {
                            // Pump in the object
                            var obj = token.ToObject<JObject>();

                            // Is it the last?
                            if (identifierEl != lastIdentifier)
                            {
                                // let's work it out
                                // update the token
                                token = obj.SelectToken(identifierEl);
                            }
                            // Yes its the last
                            else
                            {
                                // let's work it out
                                // update the token
                                token = obj.SelectToken(identifierEl).Parent;
                            }
                        }
                        // iterate JValue like a JObject
                        else if (token is JValue)
                        {
                            // Pump in the object
                            JObject obj = token.ToObject<JObject>();

                            // Is it the last?
                            if (identifierEl != lastIdentifier)
                            {
                                // let's work it out
                                // update the token
                                token = obj.SelectToken(identifierEl);
                            }
                            // Yes its the last
                            else
                            {
                                // let's work it out
                                // update the token
                                token = obj.SelectToken(identifierEl).Parent;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                _logger.LogCritical("[BaseProcessingService]" +
                                    $" Update: Invalid token for {identifier}");
            }

            return token;
        }
    }
}