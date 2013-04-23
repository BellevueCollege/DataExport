using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Common.Logging;

namespace DataExport.Web
{
  public static class Utility
  {
    private static ILog _log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Replaces string templates, which can be included in configuration settings
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    /// <remarks>
    /// This method is intended to be used as a <see cref="MatchEvaluator"/> delegate for
    /// calls to <see cref="Regex.Replace(string,System.Text.RegularExpressions.MatchEvaluator)">Regex.Replace()</see>.
    /// </remarks>
    /// <seealso cref="DeliveryStrategy.Destination"/>
    public static string ReplaceTemplate(Match match)
    {
      // remove surrounding braces (which denote the template)
      string value = match.Value.Trim("{}".ToCharArray());
      // then divide into type & template
      string[] parts = value.Split('|');

      if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
      {
        _log.Warn(msg => msg("'{0}' is not a valid string template.", value));
      }
      else
      {
        switch (parts[0].ToUpper())
        {
          case "TIMESTAMP":
            try
            {
              return DateTime.Now.ToString(parts[1]);
            }
            catch (Exception ex)
            {
              _log.Warn(msg => msg("Attempting to format Template '{0}' resulted in an Exception.", value), ex);
            }
            break;

          default:
            _log.Warn(msg => msg("Unrecognized string template: '{0}'", value));
            break;
        }
      }

      return value;
    }
  }
}