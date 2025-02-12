using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Data;

namespace Nozomi.Preprocessing.Abstracts
{
    public abstract class BaseProcessingService<T> : BaseHostedService<T> where T : class
    {
        public BaseProcessingService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        // [Obsolete]
        public NozomiResult<JToken> ProcessIdentifier(JToken token, string identifier)
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
                                    // var originalToken = token;

                                    if (comArrElArr.Length == 2)
                                    {
                                        // var lastElementProp = comArrElArr.Last();

                                        // Pump in the array, treat it as anonymous.
                                        var dataList = token.ToList();
                                        
                                        if (int.TryParse(comArrElArr[0], out var targetEl))
                                        {
                                            token = dataList
                                                .FirstOrDefault(obj => obj[targetEl] != null
                                                                       && obj[targetEl].ToString()
                                                                           .Equals(comArrElArr[1]));
                                        }
                                        else
                                        {
                                            token = dataList
                                                .FirstOrDefault(obj => obj[comArrElArr[0]] != null
                                                                       && obj[comArrElArr[0]].ToString()
                                                                           .Equals(comArrElArr[1]));
                                        }

                                        // let's work it out
                                        // update the token
                                        if (token != null)
                                        {
                                            return new NozomiResult<JToken>()
                                            {
                                                ResultType = NozomiResultType.Success,
                                                Data = token
                                            };
                                        }
                                        else
                                        // May or may not have been a proper lookup.
                                        // Sometimes the collection may or may not appear for this current request.
                                        {
                                            // Not a proper identifier
                                            _logger.LogWarning("[BaseProcessingService] " +
                                                             $"Can't parse array element {identifierEl} \n" +
                                                             $"Invalid element property {comArrElArr[0]} \n" +
                                                             $"Original Payload empty?: {false}");
                                            
                                            return new NozomiResult<JToken>()
                                            {
                                                ResultType = NozomiResultType.Failed,
                                                Message = "Could not locate target.",
                                                Data = null
                                            };
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
                                            return new NozomiResult<JToken>(NozomiResultType.Failed,
                                                "Invalid index.");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("[BaseProcessingService]: " +
                                                 " An un-captured error " +
                                                 $"occurred: {ex}");
                                return new NozomiResult<JToken>(NozomiResultType.Failed, "[BaseProcessingService]: " +
                                                                                         " An un-captured error " +
                                                                                         $"occurred: {ex}");
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
                        return new NozomiResult<JToken>(NozomiResultType.Failed, "Null identifier.");
                    }
                }
            }
            else
            {
                _logger.LogCritical("[BaseProcessingService]" +
                                    $" Update: Invalid token for {identifier}");
            }

            return new NozomiResult<JToken>()
            {
                ResultType = NozomiResultType.Success,
                Data = token
            };
        }

        // public bool ProcessIdentifier(JsonElement jsonDoc, string identifier)
        // {
        //     // Identifier processing
        //     if (!string.IsNullOrEmpty(identifier) && jsonDoc.ValueKind != JsonValueKind.Null)
        //     {
        //         var identifierArr = identifier.Split("/"); // Split the string if its nesting
        //         var lastIdentifier = identifierArr.LastOrDefault(); // get the last to identify if its the last
        //
        //         foreach (var identifierEl in identifierArr)
        //         {
        //             if (identifierEl != null)
        //             {
        //                 // CHECK CURRENT TYPE
        //                 // Identify if its an array or an object
        //                 if (jsonDoc.ValueKind is JsonValueKind.Array)
        //                 {
        //                     try
        //                     {
        //                         // Is it the last?
        //                         if (identifierEl != lastIdentifier) // If true, then it ain't the last
        //                         {
        //                             // Parse the identifierEl to an integer for index access
        //                             if (int.TryParse(identifierEl, out var index))
        //                             {
        //                                 // Pump in the array, treat it as anonymous.
        //                                 var dataList = jsonDoc.EnumerateArray();
        //
        //                                 // let's work it out
        //                                 // update the token
        //                                 if (index >= 0 && index < dataList.Count())
        //                                 {
        //                                     // Traverse the array
        //                                     jsonDoc = jsonDoc.EnumerateArray().ToArray()[index];
        //                                     // jsonDoc =
        //                                     //     JToken.Parse(JsonConvert.SerializeObject(dataList[index]));
        //                                 }
        //                             }
        //                         }
        //                         // Yes its the last
        //                         else
        //                         {
        //                             // See if there's any property we need to refer to.
        //                             var comArrElArr = identifierEl.Split("=>");
        //
        //                             // An array of objects. Let's find the key comArrElArr[0] where
        //                             // the value equals comArrElArr[1]
        //                             var originalToken = jsonDoc;
        //                             if (comArrElArr.Length == 2)
        //                             {
        //                                 // var lastElementProp = comArrElArr.Last();
        //
        //                                 // If this is an array
        //                                 if (int.TryParse(comArrElArr[0], out var targetEl))
        //                                 {
        //                                     // Pump in the array, treat it as anonymous.
        //                                     var dataList = jsonDoc.EnumerateArray();
        //                                     
        //                                     // jsonDoc = dataList
        //                                     //     .FirstOrDefault(obj => obj[targetEl] != null
        //                                     //                            && obj[targetEl].ToString()
        //                                     //                                .Equals(comArrElArr[1]));
        //                                     var isTargetArray = dataList.Current[targetEl]
        //                                         .ValueEquals(comArrElArr[1]);
        //
        //                                     if (isTargetArray)
        //                                         jsonDoc = dataList.Current;
        //                                     else
        //                                     {
        //                                         _logger.LogError($"{_hostedServiceName} Update: Invalid " +
        //                                                          $"ending array identifier {identifier}");
        //                                     }
        //                                 }
        //                                 else
        //                                 {
        //                                     // Pump in the object, treat it as anonymous.
        //                                     var dataList = jsonDoc.EnumerateObject();
        //                                     
        //                                     // jsonDoc = dataList
        //                                     //     .FirstOrDefault(obj => obj[comArrElArr[0]] != null
        //                                     //                            && obj[comArrElArr[0]].ToString()
        //                                     //                                .Equals(comArrElArr[1]));
        //                                     
        //                                     // Obtain the identifying property first
        //                                     var isTargetObject = dataList
        //                                         .SingleOrDefault(e => e.NameEquals(comArrElArr[0]))
        //                                         .Value.ValueEquals(comArrElArr[1]);
        //
        //                                     if (isTargetObject)
        //                                         jsonDoc = dataList.;
        //                                 }
        //
        //                                 return true;
        //                             }
        //                             // A standard array
        //                             else if (comArrElArr.Length == 1)
        //                             {
        //                                 // Parse the identifierEl to an integer for index access
        //                                 if (int.TryParse(identifierEl, out int index))
        //                                 {
        //                                     // Pump in the array, treat it as anonymous.
        //                                     var dataList = jsonDoc.EnumerateArray();
        //
        //                                     // let's work it out
        //                                     // update the token
        //                                     if (index >= 0 && index < dataList.Count())
        //                                     {
        //                                         // Traverse the array and move up to the parent
        //                                         jsonDoc =
        //                                             JToken.Parse(JsonConvert.SerializeObject(dataList[index])).Parent;
        //                                     }
        //                                 }
        //                                 else
        //                                 {
        //                                     _logger.LogError("[BaseProcessingService]" +
        //                                                      $" Update: Invalid array element {identifierEl}");
        //                                     return false;
        //                                 }
        //                             }
        //                         }
        //                     }
        //                     catch (Exception ex)
        //                     {
        //                         _logger.LogError("[BaseProcessingService]: " +
        //                                          " An un-captured error " +
        //                                          $"occurred: {ex}");
        //                         return false;
        //                     }
        //                 }
        //                 else if (jsonDoc.ValueKind is JsonValueKind.Object)
        //                 {
        //                     // Pump in the object
        //                     var obj = jsonDoc;
        //
        //                     // Is it the last?
        //                     if (identifierEl != lastIdentifier)
        //                     {
        //                         // let's work it out
        //                         // update the token
        //                         jsonDoc = obj.GetProperty(identifierEl);
        //                     }
        //                     // Yes its the last
        //                     else
        //                     {
        //                         // let's work it out
        //                         // update the token
        //                         jsonDoc = obj.SelectToken(identifierEl).Parent;
        //                     }
        //                 }
        //             }
        //             else
        //             {
        //                 return false;
        //             }
        //         }
        //     }
        //     else
        //     {
        //         _logger.LogCritical("[BaseProcessingService]" +
        //                             $" Update: Invalid token for {identifier}");
        //         return false;
        //     }
        //
        //     return true;
        // }
    }
}