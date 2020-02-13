<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html"/>

    <xsl:template match="/">
    <html>
      <body>
        <table border ="1">
          <TR>
            <th>Name</th>
            <th>Equatorial radius</th>
            <th>Mass in earth mass units</th>
            <th>Number of natural satellites</th>
          </TR>
          <xsl:for-each select="Planets/Planet">
            <tr>
              <td>
                <xsl:value-of select="@Name"/>
              </td>
              <td>
                <xsl:value-of select="@EquatorialRadius"/>
              </td>
              <td>
                <xsl:value-of select="@MassInEarthMass"/>
              </td>
              <td>
                <xsl:value-of select="@NumberOfNaturalSatellites"/>
              </td>
            </tr>
            </xsl:for-each>
          </table>
         </body>
      </html>
    </xsl:template>
</xsl:stylesheet>
