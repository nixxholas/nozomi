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

                                        var lastElementProp = comArrElArr.Last();

                                        // Pump in the array, treat it as anonymous.
                                        var dataList = token.ToObject<List<JObject>>();

                                        token = dataList
                                            .FirstOrDefault(obj => obj.ContainsKey(comArrElArr[0])
                                                                   && obj.SelectToken(comArrElArr[0]).ToString()
                                                                       .Equals(comArrElArr[1]));

                                        // let's work it out
                                        // update the token
                                        if (token != null)
                                        {
                                            return token;
                                        }
                                        else
                                        {
                                            // Not a proper index/array
                                            _logger.LogError("[BaseProcessingService] " +
                                                             $"Can't parse array element {identifierEl} \n" +
                                                             $"Invalid element property {comArrElArr[0]}" +
                                                             $"Original Payload empty?: {originalToken == null}");
                                            return null;
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
                                            return null;
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
                        return null;
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