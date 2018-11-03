using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ACIPL.Template.Core.Logging
{
    /// <summary>
    ///     Arguments class
    /// </summary>
    public class CommandLineArguments
    {
        // Constructor
        public CommandLineArguments(IEnumerable<string> args)
        {
            Parameters = new Dictionary<string, string>();
            var spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string parameter = null;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
            foreach (string txt in args)
            {
                // Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
                string[] parts = spliter.Split(txt, 3);
                switch (parts.Length)
                {
                        // Found a value (for the last parameter found (space separator))
                    case 1:
                        {
                            if (parameter != null)
                            {
                                if (!Parameters.ContainsKey(parameter))
                                {
                                    parts[0] = remover.Replace(parts[0], "$1");
                                    Parameters.Add(parameter, parts[0]);
                                }

                                parameter = null;
                            }
                        }

                        break;

                        // Found just a parameter
                    case 2:
                        {
                            // The last parameter is still waiting. With no value, set it to true.
                            if (parameter != null)
                            {
                                if (!Parameters.ContainsKey(parameter))
                                {
                                    Parameters.Add(parameter, "true");
                                }
                            }

                            parameter = parts[1];
                        }

                        break;

                        // Parameter with enclosed value
                    case 3:
                        {
                            // The last parameter is still waiting. With no value, set it to true.
                            if (parameter != null)
                            {
                                if (!Parameters.ContainsKey(parameter))
                                {
                                    Parameters.Add(parameter, "true");
                                }
                            }

                            parameter = parts[1];

                            // Remove possible enclosing characters (",')
                            if (!Parameters.ContainsKey(parameter))
                            {
                                parts[2] = remover.Replace(parts[2], "$1");
                                Parameters.Add(parameter, parts[2]);
                            }

                            parameter = null;
                        }

                        break;
                }
            }

            // In case a parameter is still waiting
            if (parameter != null)
            {
                if (!Parameters.ContainsKey(parameter))
                {
                    Parameters.Add(parameter, "true");
                }
            }
        }

        // Variables
        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        ///     Retrieve a parameter value if it exists
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string this[string param]
        {
            get { return Parameters.ContainsKey(param) ? Parameters[param] : null; }
        }
    }
}