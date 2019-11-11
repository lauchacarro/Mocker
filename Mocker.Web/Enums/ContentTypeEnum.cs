using System.ComponentModel;

namespace Mocker.Web.Enums
{
    public enum ContentTypeEnum
    {
        [Description("application/json")]
        ApplicationJson,

        [Description("application/x-www-form-urlencoded")]
        ApplicationXWWWFormUrlencoded,

        [Description("application/xhtml+xml")]
        ApplicationXHtmlXml,

        [Description("application/xml")]
        ApplicationXml,

        [Description("application/javascript")]
        ApplicationJavascript,

        [Description("multipart/form-data")]
        MultipartFormData,

        [Description("text/css")]
        TextCss,

        [Description("text/csv")]
        TextCsv,

        [Description("text/html")]
        TextHtml,

        [Description("text/json")]
        TextJson,

        [Description("text/plain")]
        TextPlain,

        [Description("text/xml")]
        TextXml

    }
}
