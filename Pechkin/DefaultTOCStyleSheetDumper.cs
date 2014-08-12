using System;
using System.IO;
using System.Text;

namespace TuesPechkin
{
    public class DefaultTOCStyleSheetDumper
    {
        private readonly TableOfContentsSettings _settings;
        private readonly string _filePath;
        private const string _defaultCaption = "Table of content";
        private const int _defaultFontScale = 1;
        private const string _defaultIndentation = "2";

        public DefaultTOCStyleSheetDumper(TableOfContentsSettings settings)
        {
            _settings = settings;
            _filePath = Path.Combine(PechkinBindings.BasePath(), "toc.xsl");
        }

        public string MakeDefaultTOCStyleSheetFile()
        {
            SaveFile();
            return _filePath;
        }

        private void SaveFile()
        {
            if (File.Exists(_filePath)) return;

            var sb = new StringBuilder();
            sb.AppendFormat(_main, _settings.CaptionText ?? _defaultCaption, _settings.FontScale ?? _defaultFontScale,
                                   _settings.Indentation ?? _defaultIndentation, _settings.UseDottedLines ? _dottedLines : String.Empty);

            sb.AppendFormat(_list, _settings.ProduceForwardLinks ? _forwardLinks : String.Empty);

            File.WriteAllBytes(_filePath, Encoding.UTF8.GetBytes(sb.ToString()));
        }


        private const string _main = @"<?xml version=""1.0"" encoding=""UTF-8""?>
  <xsl:stylesheet version=""2.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:outline=""http://wkhtmltopdf.org/outline"" xmlns=""http://www.w3.org/1999/xhtml"">
    <xsl:output doctype-public=""-//W3C//DTD XHTML 1.0 Strict//EN"" doctype-system=""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"" indent=""yes"" />
    <xsl:template match=""outline:outline"">
      <html>
        <head>
          <title>{0}</title>
          <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
          <style>
            h1 {{
              text-align: center;
              font-size: 20px;
              font-family: arial;
            }}
            span {{ float: right; }}
            li {{ list-style: none; }}
            ul {{
              font-size: 20px;
              font-family: arial;
            }}
            ul {{ padding-left: 0em; }}
            ul ul {{ font-size: {1}%; }}
            ul ul {{ padding-left: {2}px; }}
            a {{ text-decoration:none; color: black; }}
            {3}
          </style>
        </head>
        <body> 
          <h1>{0}</h1>
          <ul>
            <xsl:apply-templates select=""outline:item/outline:item""/>
          </ul>
        </body>
      </html>
    </xsl:template>";

        private const string _dottedLines = @"div { border-bottom: 1px dashed rgb(200,200,200); }";

        private const string _list = @" 
    <xsl:template match=""outline:item"">
      <li>
        <xsl:if test=""@title!=''"">
          <div>
            <a>
              {0}
              <xsl:if test=""@backLink"">
                <xsl:attribute name=""name"">
                  <xsl:value-of select=""@backLink""/>
                </xsl:attribute>
              </xsl:if>
              <xsl:value-of select=""@title"" />
            </a>
            <span>
              <xsl:value-of select=""@page"" />
            </span>
          </div>
        </xsl:if>
        <ul>
          <xsl:comment>added to prevent self-closing tags in QtXmlPatterns</xsl:comment>
          <xsl:apply-templates select=""outline:item""/>
        </ul>
      </li>
    </xsl:template>
  </xsl:stylesheet>";

        private const string _forwardLinks = @" 
    <xsl:if test=""@link"">
      <xsl:attribute name=""href"">
        <xsl:value-of select=""@link""/>
      </xsl:attribute>
    </xsl:if>";
    }
}