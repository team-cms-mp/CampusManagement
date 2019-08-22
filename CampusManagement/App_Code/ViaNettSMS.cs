//-------------------------------------------------------------------------------------------
// Updated: 06.11.2012
// This source code can only be used and altered together with ViaNett's SMS system.
//
// Requirements:
// You need to have a ViaNett SMS account.
// Register at: http://sms.vianett.com/cat/485.aspx
// You need to add System.Web as a reference.
// 
// Support: smssupport@vianett.no.
//-------------------------------------------------------------------------------------------

/// <summary>
/// ViaNett SMS class provides an easy way of sending SMS messages through the HTTP API.
/// </summary>
using System.Web; // NB! need to add System.Web as a reference
using System.Net;

public class ViaNettSMS
{
    // Declarations
    private string username;
    private string password;

    /// <summary>
    /// Constructor with username and password to ViaNett gateway. 
    /// </summary>
    public ViaNettSMS(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
    /// <summary>
    /// Send SMS message through the ViaNett HTTP API.
    /// </summary>
    /// <returns>Returns an object with the following parameters: Success, ErrorCode, ErrorMessage</returns>
    /// <param name="msgsender">Message sender address. Mobile number or small text, e.g. company name</param>
    /// <param name="destinationaddr">Message destination address. Mobile number.</param>
    /// <param name="message">Text message</param>
    public Result sendSMS(string msgsender, string destinationaddr, string message)
    {
        // Declarations
        string url;
        string serverResult;
        long l;
        Result result;

        // Build the URL request for sending SMS.
        url = "http://smsc.vianett.no/ActiveServer/MT/?"
            + "username=" + HttpUtility.UrlEncode(username) 
            + "&password=" + HttpUtility.UrlEncode(password) 
            + "&destinationaddr=" + HttpUtility.UrlEncode(destinationaddr, System.Text.Encoding.GetEncoding("ISO-8859-1")) 
            + "&message=" + HttpUtility.UrlEncode(message, System.Text.Encoding.GetEncoding("ISO-8859-1")) 
            + "&refno=1";

        // Check if the message sender is numeric or alphanumeric.
        if (long.TryParse(msgsender, out l))
        {
            url = url + "&sourceAddr=" + msgsender;
        }
        else
        {
            url = url + "&fromAlpha=" + msgsender;
        }
        // Send the SMS by submitting the URL request to the server. The response is saved as an XML string.
        serverResult = DownloadString(url);
        // Converts the XML response from the server into a more structured Result object.
        result = ParseServerResult(serverResult);
        // Return the Result object.
        return result;
    }
    /// <summary>
    /// Downloads the URL from the server, and returns the response as string.
    /// </summary>
    /// <param name="URL"></param>
    /// <returns>Returns the http/xml response as string</returns>
    /// <exception cref="WebException">WebException is thrown if there is a connection problem.</exception>
    private string DownloadString(string URL)
    {
        using (System.Net.WebClient wlc = new System.Net.WebClient())
        {
            // Create WebClient instanse.
            try
            {
                // Download and return the xml response
                return wlc.DownloadString(URL);
            }
            catch (WebException ex)
            {
                // Failed to connect to server. Throw an exception with a customized text.
                throw new WebException("Error occurred while connecting to server. " + ex.Message, ex);
            }
        }
    }

    
    /// <summary>
    /// Parses the XML code and returns a Result object.
    /// </summary>
    /// <param name="ServerResult">XML data from a request through HTTP API.</param>
    /// <returns>Returns a Result object with the parsed data.</returns>
    private Result ParseServerResult(string ServerResult)
    {
        System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
        System.Xml.XmlNode ack;
        Result result = new Result();
        xDoc.LoadXml(ServerResult);
        ack = xDoc.GetElementsByTagName("ack")[0];
        result.ErrorCode = int.Parse(ack.Attributes["errorcode"].Value);
        result.ErrorMessage = ack.InnerText;
        result.Success = (result.ErrorCode == 0);
        return result;
    }

    /// <summary>
    /// The Result object from the SendSMS function, which returns Success(Boolean), ErrorCode(Integer), ErrorMessage(String).
    /// </summary>
    public class Result
    {
        public bool Success;
        public int ErrorCode;
        public string ErrorMessage;
    }
}