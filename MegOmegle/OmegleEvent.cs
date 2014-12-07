/* OmegleEvent.cs
 * 
 * Parses and encapsulates a response from the server after an event query.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MegOmegle
{
    class OmegleEvent
    {
        public string oEvent;
        public string[] values;

        /// <summary>
        /// A key-values pair for an event returned by the server.
        /// </summary>
        /// <param name="raw">The raw JSON-ish string for the event.</param>
        public OmegleEvent(string raw)
        {
            if (!String.IsNullOrEmpty(raw))
            {
                //Get event data
                MatchCollection elements = Regex.Matches(raw, "\"(.*?)\""); //Now I have 2 problems
                oEvent = elements[0].Groups[1].Value;

                //Get event values/arguments/whatever
                values = new string[elements.Count - 1];
                for (int i = 1; i < elements.Count; i++)
                    values[i - 1] = elements[i].Groups[1].Value;
            }
            else
            {
                oEvent = "";
                values = null;
            }
        }

        /// <summary>
        /// Parses a JSON string for Omegle events and returns a list of them.
        /// </summary>
        /// <param name="json">The raw JSON string returned by Omegle.</param>
        /// <returns>A list of <see cref="OmegleEvent"/>s returned by the server.</returns>
        public static List<OmegleEvent> eventsFromJSON(string json)
        {
            List<OmegleEvent> events = new List<OmegleEvent>();

            //Parse and return events from JSON string
            json = json.Substring(1, json.Length - 2); //trim the []
            foreach (Match m in Regex.Matches(json, "\\[(.*?)\\]")) //Everybody stand back!
                events.Add(new OmegleEvent(m.Groups[1].Value));
            return events;
        }
    }
}
