using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using CtcApi;
using DataExport.Web;

namespace DataExport
{
  public abstract class DeliveryStrategy
  {
    protected const string TEMPLATE_PATTERN = @"{.+\|.+}";
    // property backers
    private string _destination;

    /// <summary></summary>
    public abstract ApplicationContext Context { get; set; }

    /// <summary></summary>
    public abstract byte[] Source { get; set; }

    /// <summary>
    /// Full path to the location where the data is to be delivered to.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   The <b>Destination</b> property supports <i>string templates</i>. These templates can be included in
    ///   the destination string and will be replaced with the appropriate values. All templates must be in the
    ///   following format:
    ///   </para>
    ///   <example>
    ///     <b>{</b><i>type</i><b>|</b><i>template</i><b>}</b>
    ///   </example>
    ///   <para>
    ///   The following templates are currently supported:
    ///   </para>
    ///   <list type="table">
    ///     <item>
    ///       <term>Timestamp|<i>format</i></term>
    ///       <description>
    ///       Replaced with the current <see cref="DateTime"/>, formatted according to the <a
    ///       href="http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx">Custom Date and Time Format String</a>
    ///       specified by <i>format</i>.
    ///       </description>
    ///     </item>
    ///   </list>
    /// </remarks>
    [XmlAttribute("destination")]
    public string Destination
    {
      get { return _destination; }
      set
      {
        if (Regex.IsMatch(value, TEMPLATE_PATTERN))
        {
          _destination = Regex.Replace(value, TEMPLATE_PATTERN, Utility.ReplaceTemplate);
        }
        else
        {
          _destination = value;
        }
      }
    }

    /// <summary></summary><param name="writeMode"></param><returns></returns>
    public abstract bool Put(DeliveryWriteMode writeMode = DeliveryWriteMode.Exception);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] ConvertToSource(string data)
    {
      return Encoding.UTF8.GetBytes(data);
    }
  }

  /// <summary>
  /// Defines what action to take if the target already exists
  /// </summary>
  public enum DeliveryWriteMode
  {
    /// <summary>
    /// Replace the existing target, if it exists
    /// </summary>
    Overwrite,

    /// <summary>
    /// Throw an Exception if the target exists
    /// </summary>
    Exception,

    /// <summary>
    /// Do nothing (do not overwrite the target)
    /// </summary>
    Ignore,
  }

}